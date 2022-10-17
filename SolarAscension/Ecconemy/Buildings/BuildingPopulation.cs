using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class BuildingPopulation : Building {

    [SerializeField] private List<RessourcesValue> _production;
    [SerializeField] private RessourcesValue _population;
    [SerializeField] private float _maxHappines;
    [SerializeField] private float _currentHappines;
    [SerializeField] private int _popIncreaseTick;
    [SerializeField] private int _popDecreaseTick;
    [SerializeField] private List<NeedsAndWants> _needsAndWants;

    private bool _lastState;
    private int _tick;

    public BuildingPopulation(BuildingDescription desc) : base(desc) {
    }

    public List<RessourcesValue> Production {
        get {
            return _production;
        }

        set {
            _production = value;
        }
    }

    public List<NeedsAndWants> NeedsAndWants {
        get {
            return _needsAndWants;
        }

    }

    public float MaxHappines {
        get {
            return _maxHappines;
        }

    }

    public float CurrentHappines {
        get {
            return _currentHappines;
        }

        set {
            _currentHappines = value;
        }
    }

    public RessourcesValue Population {
        get {
            return _population;
        }

    }

    public override RessourcesProduction GetProductionInformationPerMinute(bool useEfficiency = false, int index = 0) {
        RessourcesProduction production = new RessourcesProduction();


        RessourcesValue addValue;

        foreach (NeedsAndWants needs in _needsAndWants) {
            if (needs.BuildingID == 0) {
                RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(needs.Value.Ressources);
                if (info.Type == RessourceTyp.limitType) {

                    addValue = new RessourcesValue(needs.Value.Ressources, needs.Value.Value);
                    addValue.Stored = needs.Value.Stored;
                    production.ConsumptionList.Add(addValue);


                }
                else {
                    addValue = new RessourcesValue(needs.Value.Ressources, needs.Value.Value * (60 / EconemySystemInfo.Instanz.TickTimeInSeconds));

                    production.ConsumptionList.Add(addValue);

                }
            }
        }

        foreach (RessourcesValue value in _production) {
            RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(value.Ressources);
            if (info.Type == RessourceTyp.limitType) {

                addValue = new RessourcesValue(value.Ressources, value.Value);
                addValue.Stored = value.Stored;
                production.ProductionList.Add(addValue);


            }
            else {
                addValue = new RessourcesValue(value.Ressources, value.Value * (60 / EconemySystemInfo.Instanz.TickTimeInSeconds));

                production.ProductionList.Add(addValue);

            }
        }

        return production;
    }




    protected override bool SetBuildingID(BuildingDescription desc) {

        base.SetBuildingID(desc);

        BuildingPopulationDescription description = (BuildingPopulationDescription)desc;

        _production = description.Production.ConvertAll(x => x.Clone());
        _population = description.Population.Clone();
        _needsAndWants = description.NeedsAndWants.ConvertAll(x => x.Clone());
        _popIncreaseTick = description.PopIncreaseTick;
        _popDecreaseTick = description.PopDecreaseTick;

        return true;
    }


    public override void BuildingTick() {



        CheckingNeedsAndWants();

        CalcHappines();
        CheckPopulation();



        foreach (RessourcesValue value in _production) {
            RessourcesValue toAdd = PlayerBilanz.GetProductionToAdd(value.Ressources);
            RessourcesValue have = PlayerBilanz.GetRessourcesValue(value.Ressources);
            if (have.MaxValue < (have.Value + toAdd.Value + (value.Value * _population.Value))) {
                if (EconemySystem.ShowDebugLogs) {

                }
            }
            else {
                toAdd.Value = toAdd.Value + (value.Value * _population.Value);
            }
        }
    }

    private void CalcHappines() {
        _currentHappines = 0;
        foreach (NeedsAndWants need in _needsAndWants) {

            _currentHappines = _currentHappines + (need.Fulfillvalue * (need.MaxHappinesIncrease / _maxHappines));


        }
    }

    private void CheckingNeedsAndWants() {


        foreach (NeedsAndWants need in _needsAndWants) {

            need.CheckFulfillment(this);
        }
    }

    private void CheckPopulation() {



        bool state;

        if (_currentHappines == _maxHappines) {
            state = true;
        }
        else {


            int currentMaxPop = (int)((_currentHappines / _maxHappines) * _population.MaxValue);

            if (currentMaxPop == _population.Value) {
                // no population change
                return;
            }
            else if (currentMaxPop > _population.Value) {
                state = true;
            }
            else {
                state = false;
            }


        }




        if (state != _lastState) {
            _lastState = state;
            _tick = 0;
            return;
        }

        if (state == true) {
            _tick = _tick % _popIncreaseTick;

            if (_tick == 0) {
                if (_population.Value == _population.MaxValue) {
                    return;
                }
                else {
                    _population.Value = _population.Value + 1;
                    RessourcesValue toAdd = PlayerBilanz.GetProductionToAdd(_population.Ressources);
                    toAdd.MaxValue = toAdd.MaxValue + 1;

                }

            }
        }
        else {
            _tick = _tick % _popDecreaseTick;

            if (_tick == 0) {
                if (_population.Value == 0) {
                    return;
                }
                else {
                    RessourcesValue toAdd = PlayerBilanz.GetProductionToAdd(_population.Ressources);
                    _population.Value = _population.Value - 1;
                    toAdd.MaxValue = toAdd.MaxValue - 1;
                }

            }
        }

    }


    public override bool BuildingPlaced(string playerID, BuildingContainer parent = null, bool buildWithoutRessource = false) {
        return base.BuildingPlaced(playerID, parent, buildWithoutRessource);
    }

    public override bool BuildingPlaced(PlayerBilanzInfo playerBilanz, BuildingContainer parent = null, bool buildWithoutRessource = false) {
        if (base.BuildingPlaced(playerBilanz, parent)) {
            _maxHappines = 0;

            foreach (NeedsAndWants needs in _needsAndWants) {
                _maxHappines = _maxHappines + needs.MaxHappinesIncrease;
            }

            _currentHappines = 0;
            _tick = 0;
            playerBilanz.DistributionChanged();
            return true;
        }
        else {
            return false;
        }
    }




    public override bool CompleteBuildingPlacement() {

        foreach (NeedsAndWants need in _needsAndWants) {

            if (need.Value != null) {
                PlayerBilanz.AddConsumption(need.Value, Priority);
            }
        }

        foreach (RessourcesValue value in _production) {
            PlayerBilanz.AddTotalProduction(value);
        }


        return true;
    }

    public override bool CompleteBuildingRemoval() {

        foreach (NeedsAndWants need in _needsAndWants) {

            if (need.Value != null) {
                PlayerBilanz.RemoveConsumption(need.Value, Priority);
                PlayerBilanz.ReturnStoredRessources(need.Value);
            }
        }
        foreach (RessourcesValue value in _production) {
            PlayerBilanz.RemoveTotalProduction(value);
        }

        PlayerBilanz.ReduceRessourceCap(_population);

        PlayerBilanz.CheckRessourceBilanz();
        return true;
    }

    public override void CheckPriority() {
        if (Priority != CurrentPriority) {
            ChangePriority(CurrentPriority);
        }
    }

    private void ChangePriority(int newPriority) {


        foreach (NeedsAndWants need in _needsAndWants) {

            if (need.Value != null) {
                PlayerBilanz.RemoveConsumption(need.Value, Priority);
                PlayerBilanz.ReturnStoredRessources(need.Value);
            }
        }


        foreach (NeedsAndWants need in _needsAndWants) {

            if (need.Value != null) {
                PlayerBilanz.AddConsumption(need.Value, newPriority);
            }
        }

        Priority = newPriority;
        PlayerBilanz.CheckRessourceBilanz();

    }
}
