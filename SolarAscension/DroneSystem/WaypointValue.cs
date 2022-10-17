using System;
using UnityEngine;

[Serializable]
public class WaypointValue : IHeapItem<WaypointValue>, IEquatable<WaypointValue> {

    [SerializeField] public WaypointValue parent;
    [SerializeField] public WaypointInfo node;
    [SerializeField] public int movedOnPathsCount;
    [SerializeField] public int fValue;
    [SerializeField] private int gValue;
    [SerializeField] private int hValue;
    private int _heapIndex;

    public int GValue {
        get {
            return gValue;
        }

        set {
            gValue = value;
            fValue = gValue + hValue;
        }
    }

    public int HValue {
        get {
            return hValue;
        }

        set {
            hValue = value;
            fValue = gValue + hValue;
        }
    }

    public int HeapIndex {
        get {
            return _heapIndex;
        }

        set {
            _heapIndex = value;
        }
    }

    public WaypointValue(int gValue, int hValue) {

        fValue = gValue + hValue;
        this.gValue = gValue;
        this.hValue = hValue;
        movedOnPathsCount = 0;

    }

    public int CompareTo(WaypointValue other) {
        int compare = fValue.CompareTo(other.fValue);
        if (compare == 0) {
            compare = hValue.CompareTo(other.hValue);

        }

        return compare * -1;
    }

    public bool Equals(WaypointValue other) {
        if (ReferenceEquals(null, other)) {
            return false;
        }
        if (ReferenceEquals(this, other)) {
            return true;
        }

        return node.pos.Equals(other.node.pos);
    }

    public override int GetHashCode() {
        return node.pos.GetHashCode();

    }
}
