using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class BuildingContainer : Building {
    [SerializeField] private List<Building> _buildingSlots;
    [SerializeField] private List<SlotDefiniton> _slots;

    public override RessourceDistributionStruct InputValue {
        get {
            return base.InputValue;
        }

        set {
            base.InputValue = value;
            foreach (Building building in _buildingSlots) {
                building.InputValue = value;

            }

        }
    }


    public override bool MissingDistributionRessource {
        get {
            MissingDistributionRessource = false;
            foreach (Building building in _buildingSlots) {

                if (building.MissingDistributionRessource == true) {
                    MissingDistributionRessource = true;
                    break;
                }
            }
            return base.MissingDistributionRessource;
        }

        set {
            base.MissingDistributionRessource = value;
        }
    }


    public override bool RequiresDistributionRessource {
        get {

            return base.RequiresDistributionRessource;
        }

        protected set {
            base.RequiresDistributionRessource = value;
        }
    }

    public List<Building> BuildingSlots {
        get => _buildingSlots;
    }

    public List<SlotDefiniton> Slots {
        get => _slots;
    }

    public BuildingContainer(BuildingDescription desc) : base(desc) {

        _buildingSlots = new List<Building>();

    }

    public bool GetPopulationStats(SlotDefiniton popSlot, out List<NeedsAndWants> returnNeed, out float totalHappines, out List<RessourcesValue> population) {
        float maxHappines = 0;
        float currentHappines = 0;
        totalHappines = 0;
        returnNeed = new List<NeedsAndWants>();
        Dictionary<int, (NeedsAndWants, int)> fullfilstate = new Dictionary<int, (NeedsAndWants, int)>();
        Dictionary<Ressources, RessourcesValue> popCalc = new Dictionary<Ressources, RessourcesValue>();
        population = new List<RessourcesValue>();


        if (popSlot.BuildingIDs.Count == 0) {
            return false;
        }
        foreach (int id in popSlot.BuildingIDs) {
            List<Building> buildingList = GetBuildingListInContainer(id);

            if (buildingList == null) {
                return false;
            }

            foreach (Building building in buildingList) {
                if (building is BuildingPopulation) {
                    BuildingPopulation pop = (BuildingPopulation)building;
                    maxHappines = maxHappines + pop.MaxHappines;
                    currentHappines = currentHappines + pop.CurrentHappines;


                    if (popCalc.TryGetValue(pop.Population.Ressources, out RessourcesValue value) == false) {

                        value = new RessourcesValue(pop.Population.Ressources, pop.Population.Value);

                        popCalc.Add(value.Ressources, value);

                    }
                    else {
                        value.Value = value.Value + pop.Population.Value;
                    }

                    foreach (NeedsAndWants need in pop.NeedsAndWants) {
                        if (fullfilstate.TryGetValue(need.ID, out (NeedsAndWants, int) fullfilstateNeed) == false) {
                            fullfilstateNeed.Item2 = 1;
                            fullfilstateNeed.Item1 = need.Clone();
                            fullfilstateNeed.Item1.Fulfillvalue = need.Fulfillvalue;
                            fullfilstate.Add(fullfilstateNeed.Item1.ID, fullfilstateNeed);
                        }
                        else {
                            fullfilstateNeed.Item2 = fullfilstateNeed.Item2 + 1;
                            fullfilstateNeed.Item1.Fulfillvalue = fullfilstateNeed.Item1.Fulfillvalue + need.Fulfillvalue;
                        }
                    }


                }
                else {
                    return false;
                }
            }
        }

        foreach (RessourcesValue value in popCalc.Values) {
            population.Add(value);
        }

        foreach ((NeedsAndWants, int) pair in fullfilstate.Values) {
            pair.Item1.Fulfillvalue = pair.Item1.Fulfillvalue / pair.Item2;
            returnNeed.Add(pair.Item1);
        }

        totalHappines = currentHappines / maxHappines;




        return true;
    }



    public override RessourcesProduction GetProductionInformationPerMinute(bool useEfficiency = false, int index = 0) {

        RessourcesProduction returnProduction = new RessourcesProduction();

        Dictionary<Ressources, RessourcesValue> returnValuesConsumption = new Dictionary<Ressources, RessourcesValue>();
        Dictionary<Ressources, RessourcesValue> returnValuesProductionn = new Dictionary<Ressources, RessourcesValue>();

        int counter = 0;

        foreach (Building building in _buildingSlots) {

            RessourcesProduction pro = building.GetProductionInformationPerMinute(useEfficiency, index);

            if (pro != null) {
                foreach (RessourcesValue value in pro.ConsumptionList) {
                    if (returnValuesConsumption.TryGetValue(value.Ressources, out RessourcesValue consumption)) {
                        consumption.Value = consumption.Value + value.Value;
                        consumption.Stored = consumption.Stored + value.Stored;
                    }
                    else {
                        consumption = new RessourcesValue(value.Ressources, value.Value);
                        consumption.Stored = value.Stored;
                        returnValuesConsumption.Add(consumption.Ressources, consumption);
                        returnProduction.ConsumptionList.Add(consumption);
                    }
                }
                foreach (RessourcesValue value in pro.ProductionList) {
                    if (returnValuesProductionn.TryGetValue(value.Ressources, out RessourcesValue production)) {
                        production.Value = production.Value + value.Value;
                        production.Stored = production.Stored + value.Stored;
                    }
                    else {
                        production = new RessourcesValue(value.Ressources, value.Value);
                        production.Stored = value.Stored;
                        returnValuesProductionn.Add(production.Ressources, production);
                        returnProduction.ProductionList.Add(production);
                    }
                }
                returnProduction.Efficiency = returnProduction.Efficiency + pro.Efficiency;
                counter = counter + 1;
            }

        }


        if (counter != 0) {
            returnProduction.Efficiency = returnProduction.Efficiency / counter;
        }




        return returnProduction;
    }





    protected override bool SetBuildingID(BuildingDescription desc) {

        base.SetBuildingID(desc);

        BuildingContainerDescription description = (BuildingContainerDescription)desc;

        _slots = description.BuildingSlots.ConvertAll(x => x.Clone());





        return true;
    }


    public bool AddBuildingToBuildingSlot(Building addBuilding) {




        SlotDefiniton slotDefiniton = null;
        foreach (SlotDefiniton def in _slots) {
            if (def.CheckSlot(addBuilding.ID)) {
                if ((def.Used + 1) > def.Slots) {
                    continue;
                }
                slotDefiniton = def;
                break;
            }
        }

        if (slotDefiniton == null) {
            return false;
        }


        slotDefiniton.Used = slotDefiniton.Used + 1;
        _buildingSlots.Add(addBuilding);

        RequiresDistributionRessource = RequiresDistributionRessource | addBuilding.RequiresDistributionRessource;


        return true;

    }

    public void RemoveBuildingFromBuilidngSlot(Building removeBuilding) {

        foreach (SlotDefiniton def in _slots) {
            if (def.CheckSlot(removeBuilding.ID)) {
                def.Used = def.Used - 1;
                break;
            }
        }

        _buildingSlots.Remove(removeBuilding);

        RequiresDistributionRessource = false;

        foreach (Building building in _buildingSlots) {

            RequiresDistributionRessource = RequiresDistributionRessource | building.RequiresDistributionRessource;

        }


    }
    public List<Building> GetBuildingListInContainer(int iD) {

        List<Building> list = new List<Building>();


        foreach (Building building in _buildingSlots) {
            if (building.ID == iD) {
                list.Add(building);

            }
        }
        if (list.Count != 0) {
            return list;
        }
        return null;
    }

    public override bool BuildingRemoved() {

        if (base.BuildingRemoved()) {

            for (int i = _buildingSlots.Count - 1; i >= 0; i--) {
                Building building = _buildingSlots[i];
                building.BuildingRemoved();
            }

            return true;
        }
        else {
            return false;
        }
    }


    public override bool CompleteBuildingPlacement() {
        //throw new System.NotImplementedException();

        return false;
    }

    public override bool CompleteBuildingRemoval() {

        return false;
    }

    public override void BuildingTick() {

    }

    public override void CheckPriority() {
        if (CurrentPriority != Priority) {
            int dif = CurrentPriority - Priority;
            foreach (Building building in _buildingSlots) {
                building.CurrentPriority = building.CurrentPriority + dif;
            }
            Priority = CurrentPriority;
        }
    }

    public override bool IsActive {
        get {
            return base.IsActive;
        }

        set {
            foreach (Building building in _buildingSlots) {
                building.IsActive = value;
            }
            base.IsActive = value;
        }
    }

    public override bool BuildingPlaced(PlayerBilanzInfo playerBilanz, BuildingContainer parent = null, bool buildWithoutRessource = false) {

        if (base.BuildingPlaced(playerBilanz, parent, buildWithoutRessource) == true) {
            playerBilanz.DistributionChanged();
            return true;
        }
        return false;
    }
}
