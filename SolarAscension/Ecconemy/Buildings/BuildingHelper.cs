using System.Collections.Generic;
using System.Linq;

public static class BuildingHelper {



    public static bool PlaceMultipleBuildings(Building[] buildingList, PlayerBilanzInfo playerBilanzInfo) {

        List<int> id = new List<int>();

        foreach (Building b in buildingList) {
            id.Add(b.ID);
        }

        if (playerBilanzInfo.ConsumeBuildRessources(id.ToArray()) == false) {
            return false;
        }

        foreach (Building b in buildingList) {
            b.BuildingPlaced(playerBilanzInfo, null, true);
        }
        return true;
    }
    public static bool PlaceMultipleBuildings(Building[] buildingList, string playerID) {

        PlayerBilanzInfo playerBilanz = EconemySystemInfo.Instanz.PlayerList.FirstOrDefault(x => x.PlayerID == playerID);
        if (playerBilanz == null) {
            return false;
        }
        return PlaceMultipleBuildings(buildingList, playerBilanz);
    }

    public static bool CheckHasBuildRessources(int ID, PlayerBilanzInfo playerBilanz) {

        return CheckHasBuildRessources(playerBilanz, ID);

    }

    public static bool CheckHasBuildRessources(int ID, string playerID) {

        PlayerBilanzInfo playerBilanz = EconemySystemInfo.Instanz.PlayerList.FirstOrDefault(x => x.PlayerID == playerID);
        if (playerBilanz == null) {
            return false;
        }

        return CheckHasBuildRessources(playerBilanz, ID);
    }
    public static bool CheckHasBuildRessources(string playerID, int IDOne, int IDTwo = -1) {
        PlayerBilanzInfo playerBilanz = EconemySystemInfo.Instanz.PlayerList.FirstOrDefault(x => x.PlayerID == playerID);
        if (playerBilanz == null) {
            return false;
        }
        return CheckHasBuildRessources(playerBilanz, IDOne, IDTwo);
    }

    public static bool CheckHasBuildRessources(PlayerBilanzInfo playerBilanz, int IDOne, int IDTwo = -1) {
        int[] ID = new int[] { IDOne, IDTwo };


        return CheckHasBuildRessources(playerBilanz, ID);
    }
    public static bool CheckHasBuildRessources(PlayerBilanzInfo playerBilanz, int[] ID) {

        Dictionary<Ressources, RessourcesValue> buildCostsList = GetBuildCostList(ID);

        if (buildCostsList == null) {
            return false;
        }

        bool hasAllRessources = true;
        foreach (RessourcesValue value in buildCostsList.Values) {
            RessourcesValue have = playerBilanz.GetRessourcesValue(value.Ressources);

            if (have.Value < value.Value) {
                hasAllRessources = false;
                break;
            }
        }


        return hasAllRessources;
    }
    public static Dictionary<Ressources, RessourcesValue> GetBuildCostList(int[] ID) {
        Dictionary<Ressources, RessourcesValue> buildCostsList = new Dictionary<Ressources, RessourcesValue>();

        foreach (int id in ID) {
            if (id == -1) {
                continue;
            }
            BuildingDescription description = EconemySystemInfo.Instanz.GetBuildingProductionDescription(id);
            if (description == null) {
                return null;
            }
            foreach (RessourcesValue costValue in description.RessourceCostList) {
                if (buildCostsList.TryGetValue(costValue.Ressources, out RessourcesValue value) == false) {
                    value = new RessourcesValue(costValue.Ressources);
                    buildCostsList.Add(costValue.Ressources, value);
                }
                value.Value = value.Value + costValue.Value;
            }
        }

        return buildCostsList;
    }

    public static bool TrySetBuildingID(int ID, out Building building) {
        BuildingDescription description = EconemySystemInfo.Instanz.GetBuildingProductionDescription(ID);

        if (description is BuildingProductionDescription) {
            building = new BuildingProduction(description);
            //building.SetBuildingID(description);
            return true;
        }

        if (description is BuildingMiscellaneousDescription) {
            building = new BuildingMiscellaneous(description);
            //building.SetBuildingID(description);
            return true;
        }
        if (description is BuildingPopulationDescription) {
            building = new BuildingPopulation(description);
            //building.SetBuildingID(description);
            return true;
        }
        if (description is BuildingUtilityDescription) {
            building = new BuildingUtility(description);
            //building.SetBuildingID(description);
            return true;
        }
        if (description is BuildingContainerDescription) {
            building = new BuildingContainer(description);
            //building.SetBuildingID(description);
            return true;
        }




        building = null;
        return false;
    }
}