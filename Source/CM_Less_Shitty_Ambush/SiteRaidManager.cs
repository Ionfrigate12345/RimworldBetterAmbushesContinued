using System.Linq;
using CM_Less_Shitty_Ambush.Global;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace CM_Less_Shitty_Ambush
{
    internal class SiteRaidManager : WorldComponent
    {
        public SiteRaidManager(World world) : base(world)
        {
        }

        public override void WorldComponentTick()
        {
            base.WorldComponentTick();
            var tickCount = Find.TickManager.TicksGame;
            if (tickCount % GenDate.TicksPerHour * 2.4 != 1200) //Run 10x per day
            {
                return;
            }
            //Trigger chance per 1/10 days (2.4 in-game hours). The maximum possible value is 1 (100% chance to trigger each 2.4 hours)
            float chancePerOneTenthDays = LessShittyAmbushMod.settings.tempMapAdditionalAvgRaidsPer10Days / 100.0f;
            if(Rand.Range(0, 1000) > chancePerOneTenthDays * 1000)
            {
                return;
            }

            var siteMaps = Find.Maps.Where(
                m => m.Parent != null
                && !(m.Parent is Settlement && m.ParentFaction == Faction.OfPlayer) //Not player settlement
                && !Utils.IsSOS2OrRimNauts2SpaceMap(m) //Not SOS2 or Rimnauts2 maps
            ).ToList();
            if (!siteMaps.Any())
            {
                return;
            }
            var selectedMap = siteMaps.RandomElement();

            var eligibleHostileFactions = Find.FactionManager.AllFactions.Where(
                fac => fac.HostileTo(Faction.OfPlayer)
                && !fac.defeated
                && (fac == Faction.OfMechanoids
                    ||
                    (ModsConfig.AnomalyActive && fac == Faction.OfEntities)
                    ||
                    (fac.hidden == false && fac.leader != null && fac.leader.RaceProps.Humanlike)
                ) //Either mech, anomalies or non hidden human factions
            ).ToList();

            //Find the hostile faction on the map
            Faction selectedHostileFaction = null;
            if (GenHostility.AnyHostileActiveThreatToPlayer(selectedMap))
            {
                Faction onMapRandomHostileFaction = Utils.FindRandomHostileFactionOnMap(selectedMap, eligibleHostileFactions);
                if (onMapRandomHostileFaction != null && Rand.Range(0, 1000) <= 800)
                {
                    //Has a chance to be enemy reinforcement from the same enemy faction on the map
                    selectedHostileFaction = onMapRandomHostileFaction;
                    Find.LetterStack.ReceiveLetter("CM_Less_Shitty_Ambush_SettingTempMapEnemyReinforcementMessage".Translate(),
                        "CM_Less_Shitty_Ambush_SettingTempMapEnemyReinforcementMessage".Translate(),
                        LetterDefOf.NegativeEvent
                    );
                }
                else
                {
                    selectedHostileFaction = eligibleHostileFactions.RandomElement();
                }
            }
            else
            {
                selectedHostileFaction = eligibleHostileFactions.RandomElement();
            }

            float defaultThreatPoints = StorytellerUtility.DefaultThreatPointsNow(selectedMap);
            float newThreatPoints = defaultThreatPoints * LessShittyAmbushMod.settings.tempMapMultiplier;
            Logger.MessageFormat("(Less Shitty Ambush) Muliplying site raid points: {0} * {1} = {2}", defaultThreatPoints, LessShittyAmbushMod.settings.tempMapMultiplier, newThreatPoints);
            if (LessShittyAmbushMod.settings.usePlayerMainColonyThreat)
            {
                int colonyThreatAmbushPoints = Utils.GetAmbushThreatPointsByPlayerMainColonyMapWealth(LessShittyAmbushMod.settings.enemyFactionPlayerMainColonyThreatMultiplier);
                newThreatPoints += colonyThreatAmbushPoints;
                Logger.MessageFormat("(Less Shitty Ambush) Adding player colony points as additional temp map raid strength: {0}", colonyThreatAmbushPoints);
            }

            var incidentWorker = IncidentDefOf.RaidEnemy;
            if (ModsConfig.AnomalyActive && selectedHostileFaction == Faction.OfEntities)
            {
                incidentWorker = Utils.RandomIncidentDefForAnomalyRaid();
            }

            if (!Utils.RunIncident(incidentWorker, selectedMap, newThreatPoints))
            {
                Log.Error("[Less Shitty Ambush] Failed while trying to invoke IncidentWorker_RaidEnemy for faction:" + selectedHostileFaction.def.label);
                return;
            }
        }
    }
}
