using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "BuildingBalanceDescriptionData", menuName = "ScriptableObjects/BuildingBalanceDescriptionData", order = 2)]
public class BuildingBalanceDescription : ScriptableObject {

    [SerializeField] private List<BuildingProductionDescription> _productionBuildings;
    [SerializeField] private List<BuildingMiscellaneousDescription> _miscellaneousBuildings;
    [SerializeField] private List<BuildingContainerDescription> _containerBuildings;
    [SerializeField] private List<BuildingPopulationDescription> _populationBuildings;
    [SerializeField] private List<BuildingUtilityDescription> _utilityBuildings;


    public List<BuildingProductionDescription> ProductionBuildings {
        get {
            return _productionBuildings;
        }

        set {
            _productionBuildings = value;
        }
    }

    public List<BuildingMiscellaneousDescription> MiscellaneousBuildings {
        get {
            return _miscellaneousBuildings;
        }

        set {
            _miscellaneousBuildings = value;
        }
    }

    public List<BuildingPopulationDescription> PopulationBuildings {
        get {
            return _populationBuildings;
        }

        set {
            _populationBuildings = value;
        }
    }

    public List<BuildingUtilityDescription> UtilityBuildings {
        get {
            return _utilityBuildings;
        }

        set {
            _utilityBuildings = value;
        }
    }

    public List<BuildingContainerDescription> ContainerBuildings {
        get {
            return _containerBuildings;
        }

        set {
            _containerBuildings = value;
        }
    }





#if UNITY_EDITOR

    [ContextMenu("Save Data")]
    public void SaveAsset() {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }

    [ContextMenu("Import Production Buildings")]
    public void ImportProductionBuildings() {


        Dictionary<int, BuildingProductionDescription> buildingList = new Dictionary<int, BuildingProductionDescription>();

        string path = EditorUtility.OpenFilePanel("select import CSV", "", "csv");

        if (path != "" && path != null) {
            string text = File.ReadAllText(path);

            string[] textArray = Regex.Split(text, "\r\n");

            int length = textArray.Length;

            for (int i = 1; i < length;) {

                string[] row = Regex.Split(textArray[i], ";");


                if (row.Length < 2) {
                    i = i + 1;
                    continue;
                }

                BuildingProductionDescription building;

                if (buildingList.TryGetValue(int.Parse(row[0]), out building)) {

                }
                else {
                    building = new BuildingProductionDescription();
                }

                building.ID = int.Parse(row[0]);
                building.Name = row[1];
                if (row[2] != "") {
                    building.Priority = int.Parse(row[2]);
                }

                RessourcesValue value;
                if (row[3] != "" && row[4] != "") {
                    value = new RessourcesValue((Ressources)int.Parse(row[3]), float.Parse(row[4]));
                    building.RessourceCostList.Add(value);
                }

                if (row[5] != "") {
                    RessourcesProduction production;

                    production = building.ProductionRessources.FirstOrDefault(x => x.ID == row[5]);
                    if (production == null) {
                        production = new RessourcesProduction();
                        building.ProductionRessources.Add(production);
                    }



                    production.ID = row[5];
                    if (row[6] != "" && row[7] != "") {
                        value = new RessourcesValue((Ressources)int.Parse(row[6]), float.Parse(row[7]));
                        production.ConsumptionList.Add(value);
                    }

                    if (row[8] != "" && row[9] != "") {
                        value = new RessourcesValue((Ressources)int.Parse(row[8]), float.Parse(row[9]));
                        production.ProductionList.Add(value);
                    }
                }
                buildingList[building.ID] = building;


                i = i + 1;
            }

            _productionBuildings = new List<BuildingProductionDescription>();


            foreach (BuildingProductionDescription data in buildingList.Values) {
                _productionBuildings.Add(data);

            }


            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

    }

    [ContextMenu("Import Miscellaneous Buildings")]
    public void ImportMiscellaneousBuildings() {

        Dictionary<int, BuildingMiscellaneousDescription> buildingList = new Dictionary<int, BuildingMiscellaneousDescription>();

        string path = EditorUtility.OpenFilePanel("select import CSV", "", "csv");

        if (path != "" && path != null) {
            string text = File.ReadAllText(path);

            string[] textArray = Regex.Split(text, "\r\n");

            int length = textArray.Length;

            for (int i = 1; i < length;) {

                string[] row = Regex.Split(textArray[i], ";");


                if (row.Length < 2) {
                    i = i + 1;
                    continue;
                }

                BuildingMiscellaneousDescription building;

                if (buildingList.TryGetValue(int.Parse(row[0]), out building)) {

                }
                else {
                    building = new BuildingMiscellaneousDescription();
                }

                building.ID = int.Parse(row[0]);
                building.Name = row[1];
                if (row[2] != "") {
                    building.Priority = int.Parse(row[2]);
                }

                RessourcesValue value;
                if (row[3] != "" && row[4] != "") {
                    value = new RessourcesValue((Ressources)int.Parse(row[3]), float.Parse(row[4]));
                    building.RessourceCostList.Add(value);
                }


                buildingList[building.ID] = building;


                i = i + 1;
            }

            _miscellaneousBuildings = new List<BuildingMiscellaneousDescription>();


            foreach (BuildingMiscellaneousDescription data in buildingList.Values) {
                _miscellaneousBuildings.Add(data);

            }


            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }


    [ContextMenu("Import Utility Buildings")]
    public void ImportUtilityBuildings() {

        Dictionary<int, BuildingUtilityDescription> buildingList = new Dictionary<int, BuildingUtilityDescription>();

        string path = EditorUtility.OpenFilePanel("select import CSV", "", "csv");

        if (path != "" && path != null) {
            string text = File.ReadAllText(path);

            string[] textArray = Regex.Split(text, "\r\n");

            int length = textArray.Length;

            for (int i = 1; i < length;) {

                string[] row = Regex.Split(textArray[i], ";");


                if (row.Length < 2) {
                    i = i + 1;
                    continue;
                }

                BuildingUtilityDescription building;

                if (buildingList.TryGetValue(int.Parse(row[0]), out building)) {

                }
                else {
                    building = new BuildingUtilityDescription();
                }

                building.ID = int.Parse(row[0]);
                building.Name = row[1];
                if (row[2] != "") {
                    building.Priority = int.Parse(row[2]);
                }

                RessourcesValue value;
                if (row[3] != "" && row[4] != "") {
                    value = new RessourcesValue((Ressources)int.Parse(row[3]), float.Parse(row[4]));
                    building.RessourceCostList.Add(value);
                }


                buildingList[building.ID] = building;


                i = i + 1;
            }

            _utilityBuildings = new List<BuildingUtilityDescription>();


            foreach (BuildingUtilityDescription data in buildingList.Values) {
                _utilityBuildings.Add(data);

            }


            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }


    [ContextMenu("Import Population Buildings")]
    public void ImportPopulationBuildings() {

        Dictionary<int, BuildingPopulationDescription> buildingList = new Dictionary<int, BuildingPopulationDescription>();

        string path = EditorUtility.OpenFilePanel("select import CSV", "", "csv");

        if (path != "" && path != null) {
            string text = File.ReadAllText(path);

            string[] textArray = Regex.Split(text, "\r\n");

            int length = textArray.Length;

            for (int i = 1; i < length;) {

                string[] row = Regex.Split(textArray[i], ";");


                if (row.Length < 2) {
                    i = i + 1;
                    continue;
                }

                BuildingPopulationDescription building;

                if (buildingList.TryGetValue(int.Parse(row[0]), out building)) {

                }
                else {
                    building = new BuildingPopulationDescription();
                }

                building.ID = int.Parse(row[0]);
                building.Name = row[1];
                if (row[2] != "") {
                    building.Priority = int.Parse(row[2]);
                }

                RessourcesValue value;
                if (row[3] != "" && row[4] != "") {
                    value = new RessourcesValue((Ressources)int.Parse(row[3]), float.Parse(row[4]));
                    building.RessourceCostList.Add(value);
                }

                if (row[8] != "" && row[9] != "") {
                    value = new RessourcesValue((Ressources)int.Parse(row[8]), float.Parse(row[9]));
                    building.Production.Add(value);
                }


                if ((row[14] != "" || (row[15] != "" && row[16] != "")) && row[17] != "" && row[18] != "" && row[19] != "" && row[20] != "" && row[21] != "") {

                    NeedsAndWants needsAndWants = new NeedsAndWants();
                    if (row[14] != "") {
                        needsAndWants.BuildingID = int.Parse(row[14]);
                    }
                    else {
                        RessourcesValue needValue = new RessourcesValue((Ressources)int.Parse(row[15]), float.Parse(row[16]));
                        needsAndWants.Value = needValue;
                    }

                    needsAndWants.MaxHappinesIncrease = float.Parse(row[17]);

                    needsAndWants.FulfillIncreaseTickRate = int.Parse(row[18]);
                    needsAndWants.FulfillIncreaseValue = int.Parse(row[19]);

                    needsAndWants.FulfillDecreaseTickRate = int.Parse(row[20]);
                    needsAndWants.FulfillIncreaseValue = int.Parse(row[21]);


                    building.NeedsAndWants.Add(needsAndWants);
                }

                if (row[22] != "" && row[23] != "") {
                    RessourcesValue popValue = new RessourcesValue((Ressources)int.Parse(row[22]));
                    popValue.MaxValue = float.Parse(row[23]);
                    building.Population = popValue;
                }

                if (row[24] != "") {
                    building.PopIncreaseTick = int.Parse(row[24]);
                }
                if (row[25] != "") {
                    building.PopDecreaseTick = int.Parse(row[25]);
                }


                buildingList[building.ID] = building;


                i = i + 1;
            }

            _populationBuildings = new List<BuildingPopulationDescription>();


            foreach (BuildingPopulationDescription data in buildingList.Values) {
                _populationBuildings.Add(data);

            }


            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }


    [ContextMenu("Import Container Buildings")]
    public void ImportContainerBuildings() {

        Dictionary<int, BuildingContainerDescription> buildingList = new Dictionary<int, BuildingContainerDescription>();

        string path = EditorUtility.OpenFilePanel("select import CSV", "", "csv");

        if (path != "" && path != null) {
            string text = File.ReadAllText(path);

            string[] textArray = Regex.Split(text, "\r\n");

            int length = textArray.Length;

            for (int i = 1; i < length;) {

                string[] row = Regex.Split(textArray[i], ";");


                if (row.Length < 2) {
                    i = i + 1;
                    continue;
                }

                BuildingContainerDescription building;

                if (buildingList.TryGetValue(int.Parse(row[0]), out building)) {

                }
                else {
                    building = new BuildingContainerDescription();
                }

                building.ID = int.Parse(row[0]);
                building.Name = row[1];
                if (row[2] != "") {
                    building.Priority = int.Parse(row[2]);
                }

                RessourcesValue value;
                if (row[3] != "" && row[4] != "") {
                    value = new RessourcesValue((Ressources)int.Parse(row[3]), float.Parse(row[4]));
                    building.RessourceCostList.Add(value);
                }


                if (row[12] != "" && row[13] != "") {

                    string[] slotID = Regex.Split(row[12], ",");
                    SlotDefiniton slot = new SlotDefiniton();
                    foreach (string s in slotID) {


                        slot.BuildingIDs.Add(int.Parse(s));



                    }
                    slot.Slots = int.Parse(row[13]);
                    building.BuildingSlots.Add(slot);

                }



                buildingList[building.ID] = building;


                i = i + 1;
            }

            _containerBuildings = new List<BuildingContainerDescription>();


            foreach (BuildingContainerDescription data in buildingList.Values) {
                _containerBuildings.Add(data);

            }


            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }


    [ContextMenu("Import All Buildings")]
    public void ImportAllBuildings() {

        Dictionary<int, BuildingProductionDescription> buildingListProduction = new Dictionary<int, BuildingProductionDescription>();
        Dictionary<int, BuildingMiscellaneousDescription> buildingListMiscellaneous = new Dictionary<int, BuildingMiscellaneousDescription>();
        Dictionary<int, BuildingContainerDescription> buildingListContainer = new Dictionary<int, BuildingContainerDescription>();
        Dictionary<int, BuildingPopulationDescription> buildingListPopulation = new Dictionary<int, BuildingPopulationDescription>();
        Dictionary<int, BuildingUtilityDescription> buildingListUtility = new Dictionary<int, BuildingUtilityDescription>();

        string path = EditorUtility.OpenFilePanel("select import CSV", "", "csv");

        if (path != "" && path != null) {
            string text = File.ReadAllText(path);

            string[] textArray = Regex.Split(text, "\r\n");

            int length = textArray.Length;

            for (int i = 1; i < length;) {

                string[] row = Regex.Split(textArray[i], ";");


                if (row.Length < 2) {
                    i = i + 1;
                    continue;
                }

                if (row[10] != "") {
                    if (BuildingCategories.BuildingProduction == (BuildingCategories)int.Parse(row[10])) {
                        BuildingProductionDescription building;

                        if (buildingListProduction.TryGetValue(int.Parse(row[0]), out building)) {

                        }
                        else {
                            building = new BuildingProductionDescription();
                        }

                        building.ID = int.Parse(row[0]);
                        building.Name = row[1];
                        if (row[2] != "") {
                            building.Priority = int.Parse(row[2]);
                        }

                        RessourcesValue value;
                        if (row[3] != "" && row[4] != "") {
                            value = new RessourcesValue((Ressources)int.Parse(row[3]), float.Parse(row[4]));
                            building.RessourceCostList.Add(value);
                        }

                        if (row[5] != "") {
                            RessourcesProduction production;

                            production = building.ProductionRessources.FirstOrDefault(x => x.ID == row[5]);
                            if (production == null) {
                                production = new RessourcesProduction();
                                building.ProductionRessources.Add(production);
                            }



                            production.ID = row[5];
                            if (row[6] != "" && row[7] != "") {
                                value = new RessourcesValue((Ressources)int.Parse(row[6]), float.Parse(row[7]));
                                production.ConsumptionList.Add(value);
                            }

                            if (row[8] != "" && row[9] != "") {
                                value = new RessourcesValue((Ressources)int.Parse(row[8]), float.Parse(row[9]));
                                production.ProductionList.Add(value);
                            }
                        }
                        buildingListProduction[building.ID] = building;

                    }
                    else if (BuildingCategories.BuildingMiscellaneous == (BuildingCategories)int.Parse(row[10])) {
                        BuildingMiscellaneousDescription building;

                        if (buildingListMiscellaneous.TryGetValue(int.Parse(row[0]), out building)) {

                        }
                        else {
                            building = new BuildingMiscellaneousDescription();
                        }

                        building.ID = int.Parse(row[0]);
                        building.Name = row[1];
                        if (row[2] != "") {
                            building.Priority = int.Parse(row[2]);
                        }

                        RessourcesValue value;
                        if (row[3] != "" && row[4] != "") {
                            value = new RessourcesValue((Ressources)int.Parse(row[3]), float.Parse(row[4]));
                            building.RessourceCostList.Add(value);
                        }


                        buildingListMiscellaneous[building.ID] = building;
                    }
                    else if (BuildingCategories.BuildingContainer == (BuildingCategories)int.Parse(row[10])) {
                        BuildingContainerDescription building;

                        if (buildingListContainer.TryGetValue(int.Parse(row[0]), out building)) {

                        }
                        else {
                            building = new BuildingContainerDescription();
                        }

                        building.ID = int.Parse(row[0]);
                        building.Name = row[1];
                        if (row[2] != "") {
                            building.Priority = int.Parse(row[2]);
                        }

                        RessourcesValue value;
                        if (row[3] != "" && row[4] != "") {
                            value = new RessourcesValue((Ressources)int.Parse(row[3]), float.Parse(row[4]));
                            building.RessourceCostList.Add(value);
                        }


                        if (row[12] != "" && row[13] != "") {

                            string[] slotID = Regex.Split(row[12], ",");
                            SlotDefiniton slot = new SlotDefiniton();
                            foreach (string s in slotID) {


                                slot.BuildingIDs.Add(int.Parse(s));



                            }
                            slot.Slots = int.Parse(row[13]);
                            building.BuildingSlots.Add(slot);

                        }



                        buildingListContainer[building.ID] = building;

                    }
                    else if (BuildingCategories.BuildingPopulation == (BuildingCategories)int.Parse(row[10])) {
                        BuildingPopulationDescription building;

                        if (buildingListPopulation.TryGetValue(int.Parse(row[0]), out building)) {

                        }
                        else {
                            building = new BuildingPopulationDescription();
                        }

                        building.ID = int.Parse(row[0]);
                        building.Name = row[1];
                        if (row[2] != "") {
                            building.Priority = int.Parse(row[2]);
                        }

                        RessourcesValue value;
                        if (row[3] != "" && row[4] != "") {
                            value = new RessourcesValue((Ressources)int.Parse(row[3]), float.Parse(row[4]));
                            building.RessourceCostList.Add(value);
                        }

                        if (row[8] != "" && row[9] != "") {
                            value = new RessourcesValue((Ressources)int.Parse(row[8]), float.Parse(row[9]));
                            building.Production.Add(value);
                        }


                        if ((row[14] != "" || (row[15] != "" && row[16] != "")) && row[17] != "" && row[18] != "" && row[19] != "" && row[20] != "" && row[21] != "") {

                            NeedsAndWants needsAndWants = new NeedsAndWants();
                            if (row[14] != "") {
                                needsAndWants.BuildingID = int.Parse(row[14]);
                            }
                            else {
                                RessourcesValue needValue = new RessourcesValue((Ressources)int.Parse(row[15]), float.Parse(row[16]));
                                needsAndWants.Value = needValue;
                            }

                            needsAndWants.MaxHappinesIncrease = float.Parse(row[17]);

                            needsAndWants.FulfillIncreaseTickRate = int.Parse(row[18]);
                            needsAndWants.FulfillIncreaseValue = int.Parse(row[19]);

                            needsAndWants.FulfillDecreaseTickRate = int.Parse(row[20]);
                            needsAndWants.FulfillIncreaseValue = int.Parse(row[21]);


                            building.NeedsAndWants.Add(needsAndWants);
                        }

                        if (row[22] != "" && row[23] != "") {
                            RessourcesValue popValue = new RessourcesValue((Ressources)int.Parse(row[22]));
                            popValue.MaxValue = float.Parse(row[23]);
                            building.Population = popValue;
                        }

                        if (row[24] != "") {
                            building.PopIncreaseTick = int.Parse(row[24]);
                        }
                        if (row[25] != "") {
                            building.PopDecreaseTick = int.Parse(row[25]);
                        }


                        buildingListPopulation[building.ID] = building;

                    }
                    else if (BuildingCategories.BuildingUtility == (BuildingCategories)int.Parse(row[10])) {
                        BuildingUtilityDescription building;

                        if (buildingListUtility.TryGetValue(int.Parse(row[0]), out building)) {

                        }
                        else {
                            building = new BuildingUtilityDescription();
                        }

                        building.ID = int.Parse(row[0]);
                        building.Name = row[1];
                        if (row[2] != "") {
                            building.Priority = int.Parse(row[2]);
                        }

                        RessourcesValue value;
                        if (row[3] != "" && row[4] != "") {
                            value = new RessourcesValue((Ressources)int.Parse(row[3]), float.Parse(row[4]));
                            building.RessourceCostList.Add(value);
                        }


                        buildingListUtility[building.ID] = building;

                    }
                }
                else {
                    continue;
                }



                i = i + 1;
            }

            _productionBuildings = new List<BuildingProductionDescription>();
            _miscellaneousBuildings = new List<BuildingMiscellaneousDescription>();

            _containerBuildings = new List<BuildingContainerDescription>();

            _populationBuildings = new List<BuildingPopulationDescription>();
            _utilityBuildings = new List<BuildingUtilityDescription>();


            foreach (BuildingProductionDescription data in buildingListProduction.Values) {
                _productionBuildings.Add(data);

            }

            foreach (BuildingMiscellaneousDescription data in buildingListMiscellaneous.Values) {
                _miscellaneousBuildings.Add(data);

            }
            foreach (BuildingContainerDescription data in buildingListContainer.Values) {
                _containerBuildings.Add(data);

            }
            foreach (BuildingPopulationDescription data in buildingListPopulation.Values) {
                _populationBuildings.Add(data);

            }
            foreach (BuildingUtilityDescription data in buildingListUtility.Values) {
                _utilityBuildings.Add(data);

            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }



#endif
}
