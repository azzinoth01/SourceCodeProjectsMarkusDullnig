using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Building {
    [SerializeField] private BuildingContainer _parentBuilding;
    [SerializeField] private bool _isActive;
    [SerializeField] private string _name;
    [SerializeField] private int _iD;
    [SerializeField] private int _priority;

    [SerializeField] private RessourcesValue _outputValue;
    [SerializeField] private RessourceDistributionStruct _inputValue;

    [SerializeField] private PlayerBilanzInfo _playerBilanz;
    [SerializeField] private List<RessourcesValue> _ressourceCostList;
    private bool _missingDistributionRessource;
    private bool _requiresDistributionRessource;

    private int _currentPriority;


    public event Action<bool> activeChanged;

    public string Name {
        get {
            return _name;
        }

        set {
            _name = value;
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

    public int Priority {
        get {
            return _priority;
        }
        protected set {
            _priority = value;
        }
    }

    public PlayerBilanzInfo PlayerBilanz {
        get {
            return _playerBilanz;
        }

        set {
            _playerBilanz = value;
        }
    }

    public List<RessourcesValue> RessourceCostList {
        get {
            return _ressourceCostList;
        }

        set {
            _ressourceCostList = value;
        }
    }

    public BuildingContainer ParentBuilding {
        get {
            return _parentBuilding;
        }

        set {
            _parentBuilding = value;
        }
    }

    public virtual bool IsActive {
        get {
            return _isActive;
        }

        set {
            EconomyThread.AddBuildingQueue.Enqueue((this, value));
            _isActive = value;
            activeChanged?.Invoke(_isActive);
        }
    }



    public int CurrentPriority {
        get {
            return _currentPriority;
        }

        set {
            _currentPriority = value;
        }
    }

    public RessourcesValue OutputValue {
        get {
            return _outputValue;
        }

        set {
            _outputValue = value;
        }
    }

    public virtual RessourceDistributionStruct InputValue {
        get {
            return _inputValue;
        }

        set {
            _inputValue = value;
        }
    }

    public virtual bool MissingDistributionRessource {
        get {
            return _missingDistributionRessource;
        }

        set {
            _missingDistributionRessource = value;

        }
    }

    public virtual bool RequiresDistributionRessource {
        get {
            if (IsActive == false) {
                return false;
            }

            return _requiresDistributionRessource;


        }

        protected set {
            _requiresDistributionRessource = value;
        }
    }

    public virtual bool BuildingPlaced(string playerID, BuildingContainer parent = null, bool buildWithoutRessource = false) {
        _playerBilanz = EconemySystemInfo.Instanz.PlayerList.FirstOrDefault(x => x.PlayerID == playerID);
        if (_playerBilanz == null) {
            return false;
        }
        return BuildingPlaced(_playerBilanz, parent, buildWithoutRessource);
    }
    public virtual bool BuildingPlaced(PlayerBilanzInfo playerBilanz, BuildingContainer parent = null, bool buildWithoutRessource = false) {
        if (EconemySystemInfo.Instanz.PlayerList.Contains(playerBilanz)) {
            _playerBilanz = playerBilanz;

            if (buildWithoutRessource == false) {
                if (ConsumeBuildRessources() == false) {

                    return false;
                }
            }

            if (parent != null) {
                if (parent.AddBuildingToBuildingSlot(this) == false) {
                    if (buildWithoutRessource == false) {
                        ReturnConsumedBuildRessources();
                    }

                    return false;
                }
            }

            _playerBilanz = playerBilanz;
            _parentBuilding = parent;
            _isActive = true;

            EconomyThread.AddBuildingQueue.Enqueue((this, true));


            //PlayerBilanz.InvokeRedistributionEvent();
            return true;
        }
        else {
            return false;
        }

    }




    private bool ConsumeBuildRessources() {

        return _playerBilanz.ConsumeBuildRessources(_iD, _ressourceCostList);


    }
    private void ReturnConsumedBuildRessources() {
        _playerBilanz.ReturnConsumeBuildRessources(_ressourceCostList);
    }

    public virtual bool BuildingRemoved() {

        foreach (RessourcesValue value in _ressourceCostList) {

            _playerBilanz.ReturnRessources(value.Ressources, value.Value * 0.6f);

        }



        if (_parentBuilding != null) {

            _parentBuilding.RemoveBuildingFromBuilidngSlot(this);
        }

        if (_isActive == true) {
            EconomyThread.AddBuildingQueue.Enqueue((this, false));
        }

        _isActive = false;

        //PlayerBilanz.InvokeRedistributionEvent();

        return true;
    }

    public abstract bool CompleteBuildingRemoval();

    public abstract bool CompleteBuildingPlacement();

    public abstract void BuildingTick();

    public abstract void CheckPriority();

    public abstract RessourcesProduction GetProductionInformationPerMinute(bool useEfficiency = false, int index = 0);


    public Building(BuildingDescription desc) {
        SetBuildingID(desc);
    }

    protected virtual bool SetBuildingID(BuildingDescription desc) {
        _iD = desc.ID;
        _name = desc.Name;
        _priority = desc.Priority;
        _currentPriority = desc.Priority;
        _ressourceCostList = desc.RessourceCostList;
        _parentBuilding = null;

        _outputValue = new RessourcesValue(Ressources.Oxygen, 0);


        _inputValue = new RessourceDistributionStruct();

        _inputValue.Value = new RessourcesValue(Ressources.Oxygen, 0);
        _inputValue.ID = 0;

        _requiresDistributionRessource = desc.RequiresDistributionRessource;
        _missingDistributionRessource = false;
        return true;
    }
}
