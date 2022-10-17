using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildingUtilityDescription : BuildingDescription {

    [SerializeField] private List<RessourcesValue> _consumptionList;





    public List<RessourcesValue> ConsumptionList {
        get {
            return _consumptionList;
        }

        set {
            _consumptionList = value;
        }
    }


    public BuildingUtilityDescription() {

        RessourceCostList = new List<RessourcesValue>();
        _consumptionList = new List<RessourcesValue>();

    }
}
