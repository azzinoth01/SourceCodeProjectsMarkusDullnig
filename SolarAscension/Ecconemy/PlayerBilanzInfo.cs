
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerBilanzInfo {
    [SerializeField] private string _playerID;
    [SerializeField] private Dictionary<Ressources, RessourcesValue> _bilanz;


    private Dictionary<Ressources, RessourcesValue> _productionToAdd;
    private Dictionary<Ressources, RessourcesValue> _totalConsumption;
    private Dictionary<Ressources, RessourcesValue> _totalProduction;
    private Dictionary<Ressources, RessourcesValue> _totalProductionLastMinute;
    private Dictionary<Ressources, RessourcesValue[]> _productionLastMinute;
    private int _lastMinuteIndex;
    private int _maxIndex;


    private Dictionary<(int, Ressources), List<RessourcesValue>> _priorityConsumRessourceList;
    private List<RessourcesValue> _allPriorityConsumRessourceList;
    private List<int> _priorityKeys;

    private bool _redistribute;

    private uint _currentDistributionID;
    private bool _distributionChanged;


    public event Action<uint> startRedistributionEvent;


    public string PlayerID {
        get {
            return _playerID;
        }

        set {
            _playerID = value;
        }
    }

    public uint CurrentDistributionID {
        get {
            return _currentDistributionID;
        }


    }



    public PlayerBilanzInfo() {

        _redistribute = false;

        _totalConsumption = new Dictionary<Ressources, RessourcesValue>();
        _bilanz = new Dictionary<Ressources, RessourcesValue>();
        _priorityConsumRessourceList = new Dictionary<(int, Ressources), List<RessourcesValue>>();
        _priorityKeys = new List<int>();
        _allPriorityConsumRessourceList = new List<RessourcesValue>();
        _productionToAdd = new Dictionary<Ressources, RessourcesValue>();
        _totalProduction = new Dictionary<Ressources, RessourcesValue>();
        _totalProductionLastMinute = new Dictionary<Ressources, RessourcesValue>();
        _productionLastMinute = new Dictionary<Ressources, RessourcesValue[]>();
        _lastMinuteIndex = 0;
        _maxIndex = (int)(60 / EconemySystemInfo.Instanz.TickTimeInSeconds);

        if (_maxIndex <= 0) {
            _maxIndex = 1;
        }
        _currentDistributionID = 0;
        _distributionChanged = false;

        // foreach (Ressources ressources in EconemySystemInfo.Instanz.RessourceDescription.Keys) {
        //     RessourcesValue[] newList = new RessourcesValue[_maxIndex];
        //     int counter = 0;
        //     while (counter < newList.Length) {
        //         newList[counter] = new RessourcesValue(ressources);
        //         counter = counter + 1;
        //     }
        //     _productionLastMinute[ressources] = newList;
        // }

    }


    public void DistributionChanged() {

        _distributionChanged = true;
    }

    public void AddTotalProduction(RessourcesValue value) {

        if (_totalProduction.TryGetValue(value.Ressources, out RessourcesValue current)) {
            current.Value = current.Value + value.Value;


        }
        else {
            RessourcesValue newValue = new RessourcesValue(value.Ressources, value.Value);
            _totalProduction.Add(newValue.Ressources, newValue);

        }
    }

    public void RemoveTotalProduction(RessourcesValue value) {
        if (_totalProduction.TryGetValue(value.Ressources, out RessourcesValue current)) {
            current.Value = current.Value - value.Value;


        }
    }

    public RessourcesValue GetTotalProduction(Ressources ressources) {

        if (_totalProduction.TryGetValue(ressources, out RessourcesValue current)) {
            return current;


        }
        else {
            RessourcesValue value = new RessourcesValue(ressources);
            _totalProduction.Add(ressources, value);


            return value;
        }
    }

    public RessourcesValue GetProductionToAdd(Ressources ressources) {

        if (_productionToAdd.TryGetValue(ressources, out RessourcesValue value)) {
            return value;
        }
        else {
            RessourcesValue ressourcesValue = new(ressources);
            AddProductionToAdd(ressourcesValue);
            return ressourcesValue;
        }

    }

    private void AddProductionToAdd(RessourcesValue value) {
        _productionToAdd.Add(value.Ressources, value);
    }

    public RessourcesValue GetRessourcesValue(Ressources ressources) {

        if (_bilanz.TryGetValue(ressources, out RessourcesValue value)) {
            return value;
        }
        else {
            RessourcesValue ressourcesValue = new(ressources);
            AddRessourcesValue(ressourcesValue);
            return ressourcesValue;
        }
    }

    private void AddRessourcesValue(RessourcesValue value) {
        lock (_bilanz) {
            _bilanz.Add(value.Ressources, value);
        }

    }

    private void AddingRessourceValue(RessourcesValue value) {
        GetRessourcesValue(value.Ressources).Value = GetRessourcesValue(value.Ressources).Value + value.Value;
    }
    public void AddingRessourceValueLocked(RessourcesValue value) {



        lock (_bilanz) {
            RessourcesValue AddLockedvalue = GetRessourcesValue(value.Ressources);

            if (AddLockedvalue.MaxValue < AddLockedvalue.Value + value.Value) {
                AddLockedvalue.Value = AddLockedvalue.MaxValue;
            }
            else {
                AddLockedvalue.Value = AddLockedvalue.Value + value.Value;
            }



        }
    }


    public void SetRessourceCap(RessourcesValue value) {
        lock (_bilanz) {
            RessourcesValue cap = GetRessourcesValue(value.Ressources);
            cap.MaxValue = value.Value;

        }

    }

    private void AddRessourceCap(RessourcesValue value) {
        RessourcesValue cap = GetRessourcesValue(value.Ressources);
        cap.MaxValue = cap.MaxValue + value.MaxValue;
        cap.Value = cap.Value + value.MaxValue;
        CheckRessourceBilanz();
    }
    public void AddRessourceCapLocked(RessourcesValue value) {
        lock (_bilanz) {
            RessourcesValue cap = GetRessourcesValue(value.Ressources);
            cap.MaxValue = cap.MaxValue + value.Value;
            cap.Value = cap.Value + value.Value;
        }

    }

    public void ReduceRessourceCap(RessourcesValue value) {
        lock (_bilanz) {
            RessourcesValue cap = GetRessourcesValue(value.Ressources);
            cap.MaxValue = cap.MaxValue - value.Value;
            cap.Value = cap.Value - value.Value;
        }
    }

    public void AddTotalConsumption(RessourcesValue value) {

        if (_totalConsumption.TryGetValue(value.Ressources, out RessourcesValue current)) {
            current.Value = current.Value + value.Value;


        }
        else {
            RessourcesValue newValue = new RessourcesValue(value.Ressources, value.Value);
            _totalConsumption.Add(newValue.Ressources, newValue);

        }
    }
    public void RemoveTotalConsumption(RessourcesValue value) {

        if (_totalConsumption.TryGetValue(value.Ressources, out RessourcesValue current)) {
            current.Value = current.Value - value.Value;


        }
    }

    public RessourcesValue GetTotalConsumption(Ressources ressources) {

        if (_totalConsumption.TryGetValue(ressources, out RessourcesValue current)) {
            return current;
        }
        else {
            RessourcesValue value = new RessourcesValue(ressources);
            _totalConsumption.Add(ressources, value);


            return value;
        }

    }



    public void AddConsumption(RessourcesValue value, int priority = 1) {

        AddPriorityConsumption(value, priority);
        AddTotalConsumption(value);
    }



    public void RemoveConsumption(RessourcesValue value, int priority = 1) {
        if (_priorityConsumRessourceList.TryGetValue((priority, value.Ressources), out List<RessourcesValue> list)) {
            if (list.Contains(value)) {
                list.Remove(value);
                _allPriorityConsumRessourceList.Remove(value);
                RemoveTotalConsumption(value);
                _redistribute = true;
            }

        }

    }


    private void AddPriorityConsumption(RessourcesValue value, int priority) {
        List<RessourcesValue> list;
        if (_priorityConsumRessourceList.TryGetValue((priority, value.Ressources), out list)) {
            list.Add(value);
            _allPriorityConsumRessourceList.Add(value);
            _redistribute = true;
        }
        else {
            list = new List<RessourcesValue>();
            list.Add(value);
            _priorityConsumRessourceList[(priority, value.Ressources)] = list;
            if (_priorityKeys.Contains(priority) == false) {
                _priorityKeys.Add(priority);
            }

            _priorityKeys.Sort();
            _allPriorityConsumRessourceList.Add(value);
            _redistribute = true;
        }

    }



    public void ReturnStoredRessources(RessourcesValue value) {

        lock (_bilanz) {
            GetRessourcesValue(value.Ressources).Value = GetRessourcesValue(value.Ressources).Value + value.Stored;
            value.Stored = 0;
        }
    }
    public void ReturnRessources(RessourcesValue value) {

        ReturnRessources(value.Ressources, value.Value);
    }

    public void ReturnRessources(Ressources resources, float value) {
        lock (_bilanz) {
            GetRessourcesValue(resources).Value = GetRessourcesValue(resources).Value + value;

        }
    }



    public void CheckRessourceBilanz() {
        foreach (RessourcesValue value in _bilanz.Values) {
            RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(value.Ressources);
            if (value.Value < 0 && info.CanGoNegativ == false) {
                _redistribute = true;

                //RedestributePriority(0, value, true);
            }
        }

    }
    private void Redestribute(Ressources ressources) {
        RessourcesValue returnBackValue = new RessourcesValue(ressources);
        foreach (int prioritKey in _priorityKeys) {


            if (_priorityConsumRessourceList.TryGetValue((prioritKey, ressources), out List<RessourcesValue> list)) {
                foreach (RessourcesValue getstored in list) {


                    returnBackValue.Value = returnBackValue.Value + getstored.Stored;
                    getstored.Stored = 0;

                }
            }

        }


        lock (_bilanz) {
            GetRessourcesValue(ressources).Value = GetRessourcesValue(ressources).Value + returnBackValue.Value;

        }


    }



    public void RessourceValuesChanged() {
        _redistribute = true;
    }



    public void PriorityConsumption() {

        if (_redistribute == true) {
            foreach (Ressources key in _bilanz.Keys) {
                Redestribute(key);
            }
            _redistribute = false;
        }



        Dictionary<Ressources, RessourcesValue> distribution = new Dictionary<Ressources, RessourcesValue>();
        Dictionary<Ressources, RessourcesValue> currentConsumption = SumGroupedRessourceValues(_allPriorityConsumRessourceList);

        CalcDistribution(distribution, currentConsumption);






        foreach (RessourcesValue value in distribution.Values) {

            if (value.Value == currentConsumption[value.Ressources].Value) {
                foreach (int priority in _priorityKeys) {
                    if (_priorityConsumRessourceList.ContainsKey((priority, value.Ressources))) {
                        foreach (RessourcesValue consum in _priorityConsumRessourceList[(priority, value.Ressources)]) {

                            consum.Stored = consum.Stored + consum.Needed;
                        }
                    }

                }
                continue;
            }
            RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(value.Ressources);
            foreach (int priority in _priorityKeys) {
                PriorityStruct priorityValue = new PriorityStruct();
                priorityValue.PriorityFrom = priority;
                priorityValue.Percentage = 100;
                if (info.Priorities.Count != 0) {
                    foreach (PriorityStruct priorityStruct in info.Priorities) {
                        if (priorityStruct.PriorityFrom <= priority && priorityStruct.PriorityTo >= priority) {
                            priorityValue = priorityStruct;
                            break;
                        }
                    }
                }



                if (_priorityConsumRessourceList.TryGetValue((priority, value.Ressources), out List<RessourcesValue> list)) {
                    RessourcesValue maxNeeded = SumRessourceValues(list);
                    float currentConsumValue = 0;
                    if (maxNeeded.Value == 0) {
                        continue;
                    }
                    RessourcesValue calcValue = new RessourcesValue(value.Ressources);
                    if (value.Value > maxNeeded.Value) {
                        calcValue.Value = maxNeeded.Value;
                    }
                    else {
                        calcValue.Value = value.Value;
                    }

                    foreach (RessourcesValue consum in list) {

                        if (info.IsInteger) {
                            currentConsumValue = currentConsumValue + Mathf.FloorToInt(consum.Needed / maxNeeded.Value * priorityValue.Percentage * calcValue.Value);
                            consum.Stored = consum.Stored + Mathf.FloorToInt(consum.Needed / maxNeeded.Value * priorityValue.Percentage * calcValue.Value);

                        }
                        else {
                            currentConsumValue = currentConsumValue + (consum.Needed / maxNeeded.Value * priorityValue.Percentage * calcValue.Value);
                            consum.Stored = consum.Stored + (consum.Needed / maxNeeded.Value * priorityValue.Percentage * calcValue.Value);
                        }


                    }
                    value.Value = value.Value - currentConsumValue;
                    if (value.Value <= 0) {
                        break;
                    }
                }
                else {
                    continue;
                }
            }
            if (value.Value > 0) {
                foreach (int priority in _priorityKeys) {

                    if (_priorityConsumRessourceList.TryGetValue((priority, value.Ressources), out List<RessourcesValue> list)) {
                        RessourcesValue maxNeeded = SumRessourceValues(list);
                        float currentConsumValue = 0;
                        if (maxNeeded.Value == 0) {
                            continue;
                        }
                        RessourcesValue calcValue = new RessourcesValue(value.Ressources);
                        if (value.Value > maxNeeded.Value) {
                            calcValue.Value = maxNeeded.Value;
                        }
                        else {
                            calcValue.Value = value.Value;
                        }
                        foreach (RessourcesValue consum in list) {

                            if (info.IsInteger) {
                                currentConsumValue = currentConsumValue + Mathf.FloorToInt((consum.Needed / maxNeeded.Value * calcValue.Value));
                                consum.Stored = consum.Stored + Mathf.FloorToInt((consum.Needed / maxNeeded.Value * calcValue.Value));
                            }
                            else {
                                currentConsumValue = currentConsumValue + (consum.Needed / maxNeeded.Value * calcValue.Value);
                                consum.Stored = consum.Stored + (consum.Needed / maxNeeded.Value * calcValue.Value);
                            }

                        }
                        value.Value = value.Value - currentConsumValue;
                        if (value.Value <= 0) {
                            break;
                        }
                    }
                    else {
                        continue;
                    }
                }

                while (info.IsInteger == true && value.Value > 0) {
                    foreach (int priority in _priorityKeys) {

                        if (_priorityConsumRessourceList.TryGetValue((priority, value.Ressources), out List<RessourcesValue> list)) {
                            foreach (RessourcesValue consum in list) {

                                if (consum.Needed > 0) {
                                    value.Value = value.Value - 1;
                                    consum.Stored = consum.Stored + 1;
                                }

                                if (value.Value <= 0) {
                                    break;
                                }

                            }

                            if (value.Value <= 0) {
                                break;
                            }
                        }
                        else {
                            continue;
                        }
                        if (value.Value <= 0) {
                            break;
                        }
                    }
                }
            }
        }
    }


    private RessourcesValue SumRessourceValues(List<RessourcesValue> toSumList) {
        RessourcesValue value = new RessourcesValue(toSumList[0].Ressources);

        foreach (RessourcesValue sumValue in toSumList) {
            value.Value = value.Value + sumValue.Needed;
        }
        return value;
    }


    private Dictionary<Ressources, RessourcesValue> SumGroupedRessourceValues(List<RessourcesValue> toSumList) {
        Dictionary<Ressources, RessourcesValue> sum = new Dictionary<Ressources, RessourcesValue>();


        RessourcesValue summing;

        foreach (RessourcesValue value in toSumList) {
            if (sum.TryGetValue(value.Ressources, out summing) == false) {
                summing = new RessourcesValue(value.Ressources, value.Needed);
                sum[value.Ressources] = summing;
            }
            else {
                summing.Value = summing.Value + value.Needed;
            }
        }

        return sum;
    }



    private void CalcDistribution(Dictionary<Ressources, RessourcesValue> distribution, Dictionary<Ressources, RessourcesValue> currentConsumption, Dictionary<Ressources, RessourcesValue> storedRessources = null) {




        // take away total consumtion from bilanz
        foreach (RessourcesValue value in currentConsumption.Values) {
            RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(value.Ressources);
            RessourcesValue distributionValue = new RessourcesValue(value.Ressources);

            if (storedRessources != null) {
                RessourcesValue stored;

                if (storedRessources.TryGetValue(value.Ressources, out stored) == false) {
                    stored = new RessourcesValue(value.Ressources);
                }

                if (value.Value <= stored.Value) {
                    distributionValue.Value = value.Value;
                    stored.Value = stored.Value - value.Value;
                }
                else {
                    distributionValue.Value = stored.Value;
                    stored.Value = 0;
                }
            }
            else {
                lock (_bilanz) {
                    if (value.Value <= GetRessourcesValue(value.Ressources).Value || (info.CanGoNegativ == true && info.Type != RessourceTyp.limitType)) {
                        distributionValue.Value = value.Value;
                        GetRessourcesValue(value.Ressources).Value = GetRessourcesValue(value.Ressources).Value - value.Value;
                    }
                    else {
                        distributionValue.Value = GetRessourcesValue(value.Ressources).Value;
                        GetRessourcesValue(value.Ressources).Value = 0;
                    }
                }
            }



            distribution[value.Ressources] = distributionValue;

        }


    }

    public bool ConsumeBuildRessources(int buildingID, List<RessourcesValue> costs) {

        lock (_bilanz) {
            if (BuildingHelper.CheckHasBuildRessources(buildingID, this)) {
                foreach (RessourcesValue value in costs) {
                    RessourcesValue have = GetRessourcesValue(value.Ressources);
                    have.Value = have.Value - value.Value;
                }
                return true;
            }
            else {
                return false;
            }

        }
    }

    public bool ConsumeBuildRessources(int[] buildingID) {

        lock (_bilanz) {
            if (BuildingHelper.CheckHasBuildRessources(this, buildingID)) {
                Dictionary<Ressources, RessourcesValue> buildCostsList = BuildingHelper.GetBuildCostList(buildingID);
                foreach (RessourcesValue value in buildCostsList.Values) {
                    RessourcesValue have = GetRessourcesValue(value.Ressources);
                    have.Value = have.Value - value.Value;
                }
                return true;
            }
            else {
                return false;
            }

        }
    }



    public void ReturnConsumeBuildRessources(List<RessourcesValue> costs) {

        lock (_bilanz) {

            foreach (RessourcesValue value in costs) {
                RessourcesValue have = GetRessourcesValue(value.Ressources);
                have.Value = have.Value + value.Value;
            }


        }
    }


    public void AddProducedValues() {
        // LogLastProduced();


        lock (_bilanz) {

            foreach (RessourcesValue value in _productionToAdd.Values) {

                AddingRessourceValue(value);
                AddRessourceCap(value);

                value.Value = 0;
                value.MaxValue = 0;
            }

        }
        if (_distributionChanged) {
            _currentDistributionID = CurrentDistributionID + 1;
            startRedistributionEvent?.Invoke(_currentDistributionID);
            _distributionChanged = false;
        }


    }

    private void LogLastProduced() {

        _maxIndex = (int)(60 / EconemySystemInfo.Instanz.TickTimeInSeconds);

        if (_maxIndex <= 0) {
            _maxIndex = 1;
        }



        if (_lastMinuteIndex >= _maxIndex) {
            _lastMinuteIndex = 0;
        }
        bool resetIndex = false;
        foreach (Ressources ressources in EconemySystemInfo.Instanz.RessourceDescription.Keys) {

            if (_productionLastMinute.TryGetValue(ressources, out RessourcesValue[] list)) {
                if (list.Length != _maxIndex) {
                    resetIndex = true;
                    RessourcesValue[] newList = new RessourcesValue[_maxIndex];
                    int counter = 0;

                    int i = _lastMinuteIndex;
                    while (counter < newList.Length) {


                        newList[counter] = list[_lastMinuteIndex];


                        i = i + 1;
                        counter = counter + 1;
                    }

                    list = newList;
                }

                RessourcesValue newValue = new RessourcesValue(ressources, _productionToAdd[ressources].Value);
                if (_productionToAdd[ressources].MaxValue != 0) {
                    newValue.Value = _productionToAdd[ressources].MaxValue;
                }
                if (resetIndex == true) {
                    list[0] = newValue;
                }
                else {
                    list[_lastMinuteIndex] = newValue;
                }

            }
            else {

                RessourcesValue[] newList = new RessourcesValue[_maxIndex];

                _productionLastMinute[ressources] = newList;




                RessourcesValue newValue = new RessourcesValue(ressources, _productionToAdd[ressources].Value);
                if (_productionToAdd[ressources].MaxValue != 0) {
                    newValue.Value = _productionToAdd[ressources].MaxValue;
                }

                newList[_lastMinuteIndex] = newValue;

            }

        }

        if (resetIndex == true) {
            _lastMinuteIndex = 0;
        }

        _lastMinuteIndex = _lastMinuteIndex + 1;
    }
}
