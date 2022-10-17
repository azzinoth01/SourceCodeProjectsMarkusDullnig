using System;
using UnityEngine;

[Serializable]
public struct RessourceDistributionStruct {

    [SerializeField] uint _iD;
    [SerializeField] RessourcesValue _value;

    public uint ID {
        get {
            return _iD;
        }

        set {
            _iD = value;
        }
    }

    public RessourcesValue Value {
        get {
            return _value;
        }

        set {
            _value = value;
        }
    }
}
