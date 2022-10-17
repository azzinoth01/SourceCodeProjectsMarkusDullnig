using System;
using UnityEngine;


[Serializable]
public class RessourcesValue : IComparable<RessourcesValue> {
    [SerializeField] private Ressources _ressources;
    [SerializeField] private float _value;
    [SerializeField] private float _maxValue;
    [SerializeField] private bool _used;
    [SerializeField] private float _stored;
    [SerializeField] private float _needed;

    public Ressources Ressources {
        get {
            return _ressources;
        }

        set {
            _ressources = value;
        }
    }

    public float Value {
        get {
            return _value;
        }

        set {
            _value = value;
            _needed = _value - _stored;
        }
    }

    public float MaxValue {
        get {
            return _maxValue;
        }

        set {
            _maxValue = value;
        }
    }

    public bool Used {
        get {
            return _used;
        }

        set {
            _used = value;
        }
    }

    public float Stored {
        get {
            return _stored;
        }

        set {
            _stored = value;
            _needed = _value - _stored;
        }
    }

    public float Needed {
        get {
            return _needed;
        }
    }

    public void UseStored() {

        Stored = 0;

    }



    public RessourcesValue(Ressources ressources, float value = 0) {
        _ressources = ressources;
        _value = value;
        _stored = 0;

        _needed = _value - _stored;
        _maxValue = 0;
    }

    public RessourcesValue Clone() {
        RessourcesValue clone = new RessourcesValue(_ressources, _value);
        clone.MaxValue = _maxValue;
        return clone;
    }

    public int CompareTo(RessourcesValue other) {
        return _needed.CompareTo(other._needed);
    }
}
