using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public abstract class BuildingDescription {
    [SerializeField] private string _name;
    [SerializeField] private int _iD;
    [SerializeField] private int _priority;
    [SerializeField] private int _level;
    [SerializeField] private List<RessourcesValue> _ressourceCostList;
    [SerializeField, HideInInspector] private bool _requiresDistributionRessource;


    public List<RessourcesValue> RessourceCostList {
        get {
            return _ressourceCostList;
        }

        set {
            _ressourceCostList = value;
        }
    }

    public int Priority {
        get {
            return _priority;
        }

        set {
            _priority = value;
        }
    }

    public int ID {
        get {
            return _iD;
        }

        set {
            _iD = value;
        }
    }

    public string Name {
        get {
            return _name;
        }

        set {
            _name = value;
        }
    }

    public int Level {
        get {
            return _level;
        }

        set {
            _level = value;
        }
    }

    public bool RequiresDistributionRessource {
        get {
            return _requiresDistributionRessource;
        }

        set {
            _requiresDistributionRessource = value;
        }
    }
}
