using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RessourcesProduction {
    [SerializeField] private List<RessourcesValue> _consumptionList;
    [SerializeField] private List<RessourcesValue> _productionList;
    [SerializeField] private int _missingRessources;
    [SerializeField] private string _iD;
    [SerializeField] private float _efficiency;

    public List<RessourcesValue> ConsumptionList {
        get {
            return _consumptionList;
        }

        set {
            _consumptionList = value;
        }

    }

    public List<RessourcesValue> ProductionList {
        get {
            return _productionList;
        }

        set {
            _productionList = value;
        }
    }

    public int MissingRessources {
        get {
            return _missingRessources;
        }

        set {
            _missingRessources = value;
        }
    }

    public string ID {
        get {
            return _iD;
        }

        set {
            _iD = value;
        }
    }

    public float Efficiency {
        get {
            return _efficiency;
        }

        set {
            _efficiency = value;
        }
    }

    public RessourcesProduction() {
        _consumptionList = new List<RessourcesValue>();
        _productionList = new List<RessourcesValue>();
    }


    public RessourcesProduction Clone() {
        RessourcesProduction pro = new RessourcesProduction();
        pro.ID = string.Copy(_iD);
        pro.ConsumptionList = _consumptionList.ConvertAll(x => x.Clone());
        pro.ProductionList = _productionList.ConvertAll(x => x.Clone());

        return pro;
    }
}
