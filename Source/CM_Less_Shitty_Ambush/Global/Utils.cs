using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace CM_Less_Shitty_Ambush.Global
{
    public class Utils
    {
        public static int GetAmbushThreatPointsByPlayerMainColonyMapWealth(float factorPercentage)
        {
            var playerHomeMap = GetPlayerMainColonyMap();
            if(playerHomeMap ==  null)
            {
                return 0;
            }
            return GetRandomThreatPointsByPlayerWealth(playerHomeMap, factorPercentage);
        }

        public static int GetRandomThreatPointsByPlayerWealth(
            Map map,
            float factorPercentage //Percentage of threat from this player colony
            )
        {
            float threatAvg = StorytellerUtility.DefaultThreatPointsNow(map);
            return (int)(threatAvg * factorPercentage);
        }

        public static Map GetPlayerMainColonyMap(bool excludeSOS2Rimnauts2SpaceMaps = true, bool requirePlayerHome = true)
        {
            var playerHomes = (from map in Find.Maps
                               where (requirePlayerHome == false || map.IsPlayerHome)
                               && (excludeSOS2Rimnauts2SpaceMaps == false || !IsSOS2OrRimNauts2SpaceMap(map))
                               select map).OrderByDescending(map => map.PlayerWealthForStoryteller).ToList();

            return playerHomes.Count > 0 ? playerHomes.First() : null;
        }
        
        //判断该地图是否为SOS2的太空地图。
        public static bool IsSOS2SpaceMap(Map map)
        {
            if (map.Biome.defName.Contains("OuterSpace"))
            {
                return true;
            }
            return false;
        }

        public static bool IsRimNauts2SpaceMap(Map map)
        {
            return map.Biome.defName.StartsWith("RimNauts2_");
        }

        public static bool IsSOS2OrRimNauts2SpaceMap(Map map)
        {
            return IsSOS2SpaceMap(map) || IsRimNauts2SpaceMap(map);
        }

        public static bool RunIncident(IncidentDef incidentDef, float points = 0)
        {
            var incidentParms = StorytellerUtility.DefaultParmsNow(incidentDef.category, Find.World);
            if(points > 0)
            {
                incidentParms.points = points;
            }
            if (incidentDef.pointsScaleable)
            {
                var storytellerComp = Find.Storyteller.storytellerComps.First(comp =>
                    comp is StorytellerComp_OnOffCycle || comp is StorytellerComp_RandomMain);
                incidentParms = storytellerComp.GenerateParms(incidentDef.category, incidentParms.target);
            }

            return incidentDef.Worker.TryExecute(incidentParms);
        }

        public static Faction FindRandomHostileFactionOnMap(Map map, List<Faction> eligibleHostileFactions)
        {
            Faction onMapHostileFaction = null;
            var hostilePawns = map.mapPawns.AllPawnsSpawned.Where(
                pawn => pawn.Faction.HostileTo(Faction.OfPlayer)
                && eligibleHostileFactions.Contains(pawn.Faction)
            );
            Pawn randomHostilePawn = null;
            if (hostilePawns.Any())
            {
                randomHostilePawn = hostilePawns.RandomElement();
            }
            if (randomHostilePawn != null)
            {
                onMapHostileFaction = randomHostilePawn.Faction;
            }
            return onMapHostileFaction;
        }
    }
}
