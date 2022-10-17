using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildingContainerDescription : BuildingDescription {


    [SerializeField] private List<SlotDefiniton> _buildingSlots;

    public List<SlotDefiniton> BuildingSlots {
        get {
            return _buildingSlots;
        }

        set {
            _buildingSlots = value;
        }
    }

    public BuildingContainerDescription() {
        RessourceCostList = new List<RessourcesValue>();
        _buildingSlots = new List<SlotDefiniton>();
    }
}
