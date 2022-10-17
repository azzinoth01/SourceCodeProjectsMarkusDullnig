using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class RessourceInfo {
    [SerializeField] private Ressources _ressources;
    [SerializeField] private string _name;
    [SerializeField] private RessourceTyp _type;
    [SerializeField] private bool _canGoNegativ;
    [SerializeField] private int _level;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _cap;
    [SerializeField] private bool _isInteger;
    [SerializeField] private List<PriorityStruct> _priorities;

    public Ressources Ressources {
        get {
            return _ressources;
        }

        set {
            _ressources = value;
        }
    }

    public RessourceTyp Type {
        get {
            return _type;
        }

        set {
            _type = value;
        }
    }

    public bool CanGoNegativ {
        get {
            return _canGoNegativ;
        }

        set {
            _canGoNegativ = value;
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

    public string Name {
        get {
            return _name;
        }

        set {
            _name = value;
        }
    }

    public Sprite Icon {
        get {
            return _icon;
        }

        set {
            _icon = value;
        }
    }

    public List<PriorityStruct> Priorities {
        get {
            return _priorities;
        }

        set {
            _priorities = value;
        }
    }

    public int Cap {
        get {
            return _cap;
        }

        set {
            _cap = value;
        }
    }

    public bool IsInteger {
        get {
            return _isInteger;
        }

        set {
            _isInteger = value;
        }
    }

    public RessourceInfo(Ressources ressources, string name, RessourceTyp type = RessourceTyp.valueType, bool canGoNegativ = false, int level = 0, Sprite icon = null, bool isInteger = false) {
        _ressources = ressources;
        _name = name;
        _type = type;
        _canGoNegativ = canGoNegativ;
        _level = level;
        _icon = icon;
        _isInteger = isInteger;
        _priorities = new List<PriorityStruct>();
    }

}
