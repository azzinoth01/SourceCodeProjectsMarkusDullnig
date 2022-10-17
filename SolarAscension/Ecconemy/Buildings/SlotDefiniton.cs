using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SlotDefiniton {
    [SerializeField] private uint _slotID;
    [SerializeField] private List<int> _buildingIDs;
    [SerializeField] private int _slots;
    [HideInInspector][SerializeField] private int _used;
    private List<AttachmentBehaviour> _attachmentBehaviours;


    public int Slots {
        get {
            return _slots;
        }

        set {
            _slots = value;
        }
    }

    public int Used {
        get {
            return _used;
        }

        set {
            _used = value;
        }
    }

    public List<int> BuildingIDs {
        get {
            return _buildingIDs;
        }

        set {
            _buildingIDs = value;
        }
    }

    public uint SlotID {
        get {
            return _slotID;
        }

        set {
            _slotID = value;
        }
    }

    public List<AttachmentBehaviour> AttachmentBehaviours {
        get {
            return _attachmentBehaviours;
        }
        set {
            _attachmentBehaviours = value;
        }
    }

    public bool CheckSlot(int check) {

        if (_buildingIDs.Contains(check)) {
            return true;
        }

        return false;
    }

    public SlotDefiniton() {
        _buildingIDs = new List<int>();
        _attachmentBehaviours = new List<AttachmentBehaviour>();
        _slots = 0;
        _used = 0;
    }


    public SlotDefiniton Clone() {

        SlotDefiniton clone = new SlotDefiniton();
        clone._slots = _slots;
        foreach (int id in _buildingIDs) {
            clone._buildingIDs.Add(id);
        }


        return clone;
    }
}
