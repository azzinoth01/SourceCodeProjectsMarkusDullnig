using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildingPopulationDescription : BuildingDescription {


    [SerializeField] private List<RessourcesValue> _production;
    [SerializeField] private List<NeedsAndWants> _needsAndWants;
    [SerializeField] private RessourcesValue _population;
    [SerializeField] private int _popIncreaseTick;
    [SerializeField] private int _popDecreaseTick;


    public List<RessourcesValue> Production {
        get {
            return _production;
        }

        set {
            _production = value;
        }
    }

    public List<NeedsAndWants> NeedsAndWants {
        get {
            return _needsAndWants;
        }

        set {
            _needsAndWants = value;
        }
    }

    public RessourcesValue Population {
        get {
            return _population;
        }

        set {
            _population = value;
        }
    }

    public int PopIncreaseTick {
        get {
            return _popIncreaseTick;
        }

        set {
            _popIncreaseTick = value;
        }
    }

    public int PopDecreaseTick {
        get {
            return _popDecreaseTick;
        }

        set {
            _popDecreaseTick = value;
        }
    }

    public BuildingPopulationDescription() {

        RessourceCostList = new List<RessourcesValue>();
        _production = new List<RessourcesValue>();
        _needsAndWants = new List<NeedsAndWants>();

    }

}
