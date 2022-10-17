using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildingProductionDescription : BuildingDescription {

    [SerializeField] private List<RessourcesProduction> _productionRessources;


    public List<RessourcesProduction> ProductionRessources {
        get {
            return _productionRessources;
        }

        set {
            _productionRessources = value;
        }
    }
    public BuildingProductionDescription() {

        RessourceCostList = new List<RessourcesValue>();
        _productionRessources = new List<RessourcesProduction>();
    }

}
