using System;
using UnityEngine;

[Serializable]
public struct PriorityStruct {
    [SerializeField] private int _priorityFrom;
    [SerializeField] private int _priorityTo;
    [SerializeField] private float _percentage;

    public int PriorityFrom {
        get {
            return _priorityFrom;
        }

        set {
            _priorityFrom = value;
        }
    }

    public float Percentage {
        get {
            return (_percentage / 100);
        }

        set {
            _percentage = value;
        }
    }

    public int PriorityTo {
        get {
            return _priorityTo;
        }

        set {
            _priorityTo = value;
        }
    }
}
