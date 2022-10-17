
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NeedsAndWants {


    [SerializeField] private int _iD;

    [SerializeField] private int _buildingID;
    [SerializeField] private RessourcesValue _value;
    [SerializeField] private float _fulfillvalue;
    [SerializeField] private float _maxHappinesIncrease;

    [SerializeField] private int _fulfillIncreaseTickRate;
    [SerializeField] private int _fulfillDecreaseTickRate;

    [SerializeField] private int _fulfillIncreaseValue;
    [SerializeField] private int _fulfillDecreaseValue;

    private int _tick;
    private bool _lastState;





    public RessourcesValue Value {
        get {
            return _value;
        }

        set {
            _value = value;
        }
    }

    public float Fulfillvalue {
        get {
            return _fulfillvalue;
        }

        set {
            _fulfillvalue = value;
        }
    }

    public float MaxHappinesIncrease {
        get {
            return _maxHappinesIncrease;
        }

        set {
            _maxHappinesIncrease = value;
        }
    }

    public int BuildingID {
        get {
            return _buildingID;
        }

        set {
            _buildingID = value;
        }
    }

    public int FulfillIncreaseTickRate {
        get {
            return _fulfillIncreaseTickRate;
        }

        set {
            _fulfillIncreaseTickRate = value;
        }
    }

    public int FulfillDecreaseTickRate {
        get {
            return _fulfillDecreaseTickRate;
        }

        set {
            _fulfillDecreaseTickRate = value;
        }
    }

    public int FulfillIncreaseValue {
        get {
            return _fulfillIncreaseValue;
        }

        set {
            _fulfillIncreaseValue = value;
        }
    }

    public int FulfillDecreaseValue {
        get {
            return _fulfillDecreaseValue;
        }

        set {
            _fulfillDecreaseValue = value;
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

    public NeedsAndWants() {
        _tick = 0;
        _lastState = false;
        _fulfillvalue = 0;
        _value = null;
    }


    public void CheckFulfillment(Building population) {
        if (_buildingID == 0) {

            RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(Value.Ressources);
            if (info.Type == RessourceTyp.distributionType && population.InputValue.Value.Ressources == Value.Ressources) {
                if (population.InputValue.Value.Value != 0) {
                    if (population.InputValue.ID != population.PlayerBilanz.CurrentDistributionID) {
                        population.InputValue.Value.Value = 0;
                        population.MissingDistributionRessource = true;
                        Tick(false);

                    }
                    else {
                        Tick(true);
                        population.MissingDistributionRessource = false;
                    }
                }
                else {
                    Tick(false);
                    population.MissingDistributionRessource = true;
                }
            }
            else if (Value.Needed <= 0) {
                Tick(true);


                if (info.Type != RessourceTyp.limitType) {

                    Value.UseStored();
                }
            }
            else {
                if (Value.Stored == 0) {
                    Tick(false);

                }
                else if (((Value.Value / Value.Stored) * 100) >= _fulfillvalue) {
                    Tick(true);
                }
                else {
                    Tick(false);
                }

                if (info.Type != RessourceTyp.limitType) {

                    Value.UseStored();
                }

            }
        }
        else {
            List<Building> buildingList = population.ParentBuilding.GetBuildingListInContainer(_buildingID);
            if (buildingList != null) {
                bool isbreak = false;
                foreach (BuildingUtility utility in buildingList) {
                    if (utility.IsWorking == true) {
                        Tick(true);
                        isbreak = true;
                        break;
                    }

                }
                if (isbreak == false) {
                    Tick(false);
                }
            }
        }
    }
    public void Tick(bool state) {
        if (state != _lastState) {
            _tick = 0;
            _lastState = state;
            return;
        }

        _tick = _tick + 1;
        if (state == true) {


            _tick = _tick % _fulfillIncreaseTickRate;

            if (_tick == 0) {
                if (_fulfillvalue >= 100) {
                    return;
                }
                _fulfillvalue = _fulfillvalue + _fulfillIncreaseValue;

                if (_fulfillvalue >= 100) {
                    _fulfillvalue = 100;
                }
            }
        }
        else {
            _tick = _tick % _fulfillDecreaseTickRate;

            if (_tick == 0) {
                if (_fulfillvalue <= 0) {
                    return;
                }
                _fulfillvalue = _fulfillvalue - _fulfillDecreaseValue;

                if (_fulfillvalue <= 0) {
                    _fulfillvalue = 0;
                }
            }
        }

    }

    public NeedsAndWants Clone() {

        NeedsAndWants clone = new NeedsAndWants();

        clone._buildingID = _buildingID;

        clone._value = _value.Clone();

        clone._iD = _iD;

        clone._maxHappinesIncrease = _maxHappinesIncrease;

        clone._fulfillIncreaseTickRate = _fulfillIncreaseTickRate;
        clone._fulfillDecreaseTickRate = _fulfillDecreaseTickRate;

        clone._fulfillIncreaseValue = _fulfillIncreaseValue;
        clone._fulfillDecreaseValue = _fulfillDecreaseValue;

        return clone;

    }

}
