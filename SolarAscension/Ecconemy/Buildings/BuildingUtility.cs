using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildingUtility : Building {



    [SerializeField] private bool _isWorking;

    [SerializeField] private List<RessourcesValue> _consumptionList;

    public BuildingUtility(BuildingDescription desc) : base(desc) {
    }

    public bool IsWorking {
        get {
            return _isWorking;
        }


    }

    public List<RessourcesValue> ConsumptionList {
        get {
            return _consumptionList;
        }

        set {
            _consumptionList = value;
        }
    }

    public override RessourcesProduction GetProductionInformationPerMinute(bool useEfficiency = false, int index = 0) {

        RessourcesProduction production = new RessourcesProduction();

        RessourcesValue addValue;

        foreach (RessourcesValue value in _consumptionList) {
            production.Efficiency = 1;
            RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(value.Ressources);
            if (info.Type == RessourceTyp.limitType) {

                addValue = new RessourcesValue(value.Ressources, value.Value);
                addValue.Stored = value.Stored;
                production.ConsumptionList.Add(addValue);
                if (value.Stored == 0) {
                    production.Efficiency = 0;
                    continue;
                }
                production.Efficiency = production.Efficiency * (value.Stored / value.Value);
                if (production.Efficiency > 1) {
                    production.Efficiency = 1;
                }
                continue;
            }
            else {
                addValue = new RessourcesValue(value.Ressources, value.Value * (60 / EconemySystemInfo.Instanz.TickTimeInSeconds));

                production.ConsumptionList.Add(addValue);

            }


        }


        return production;
    }




    protected override bool SetBuildingID(BuildingDescription desc) {

        base.SetBuildingID(desc);

        BuildingUtilityDescription description = (BuildingUtilityDescription)desc;

        _consumptionList = description.ConsumptionList.ConvertAll(x => x.Clone());



        return true;
    }


    public override void BuildingTick() {
        bool missingRessources = false;

        foreach (RessourcesValue value in _consumptionList) {
            RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(value.Ressources);
            if (info.Type == RessourceTyp.distributionType && InputValue.Value.Ressources == value.Ressources) {
                if (InputValue.Value.Value != 0) {
                    if (InputValue.ID != PlayerBilanz.CurrentDistributionID) {
                        InputValue.Value.Value = 0;
                        missingRessources = true;
                        MissingDistributionRessource = true;
                        break;
                    }
                    MissingDistributionRessource = false;
                }
                else {
                    MissingDistributionRessource = true;
                }
            }
            else if (value.Needed > 0) {
                missingRessources = true;
                break;
            }
        }

        if (missingRessources == false) {
            _isWorking = true;
            foreach (RessourcesValue value in _consumptionList) {
                RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(value.Ressources);
                if (info.Type == RessourceTyp.limitType || info.Type == RessourceTyp.distributionType) {
                    continue;
                }
                value.UseStored();
            }
        }
        else {
            _isWorking = false;
        }
    }

    public override void CheckPriority() {
        if (Priority != CurrentPriority) {
            ChangePriority(CurrentPriority);
        }
    }

    private void ChangePriority(int newPriority) {


        foreach (RessourcesValue value in _consumptionList) {

            PlayerBilanz.RemoveConsumption(value, Priority);
            PlayerBilanz.ReturnStoredRessources(value);

        }


        foreach (RessourcesValue ressources in _consumptionList) {
            //PlayerBilanz.AddTotalConsumption(ressources);
            PlayerBilanz.AddConsumption(ressources, newPriority);
        }

        Priority = newPriority;

        PlayerBilanz.CheckRessourceBilanz();
    }

    public override bool CompleteBuildingPlacement() {


        foreach (RessourcesValue ressources in _consumptionList) {
            //PlayerBilanz.AddTotalConsumption(ressources);
            RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(ressources.Ressources);
            if (info.Type == RessourceTyp.distributionType) {

                MissingDistributionRessource = true;
                continue;
            }
            PlayerBilanz.AddConsumption(ressources, Priority);
        }


        return true;
    }

    public override bool CompleteBuildingRemoval() {

        foreach (RessourcesValue value in _consumptionList) {

            RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(value.Ressources);
            if (info.Type == RessourceTyp.distributionType) {

                MissingDistributionRessource = false;
                continue;
            }

            PlayerBilanz.RemoveConsumption(value, Priority);
            PlayerBilanz.ReturnStoredRessources(value);

        }
        PlayerBilanz.CheckRessourceBilanz();

        return true;
    }

    public override bool BuildingPlaced(PlayerBilanzInfo playerBilanz, BuildingContainer parent = null, bool buildWithoutRessource = false) {

        if (base.BuildingPlaced(playerBilanz, parent, buildWithoutRessource) == true) {
            playerBilanz.DistributionChanged();
            return true;
        }
        return false;
    }
}
