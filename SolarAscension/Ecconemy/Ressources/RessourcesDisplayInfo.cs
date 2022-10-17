using System;
using UnityEngine;

[Serializable]
public class RessourcesDisplayInfo {
    [SerializeField] private Ressources _ressources;
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField] private RessourceTyp _typ;


    public Ressources Ressources {
        get {
            return _ressources;
        }

        set {
            _ressources = value;
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

    public RessourceTyp Typ {
        get {
            return _typ;
        }

        set {
            _typ = value;
        }
    }



    public RessourcesDisplayInfo(Ressources ressources, RessourceTyp typ, string name = "", Sprite icon = null) {
        _ressources = ressources;
        _typ = typ;
        _name = name;
        _icon = icon;

    }
}
