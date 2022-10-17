using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class BuildingProduction : Building {


    [SerializeField] private List<RessourcesProduction> _productionRessources;

    public BuildingProduction(BuildingDescription desc) : base(desc) {
    }

    public List<RessourcesProduction> ProductionRessources {
        get {
            return _productionRessources;
        }

        set {
            _productionRessources = value;
        }
    }





    public override bool CompleteBuildingPlacement() {
        foreach (RessourcesProduction pro in _productionRessources) {

            foreach (RessourcesValue ressources in pro.ConsumptionList) {
                //PlayerBilanz.AddTotalConsumption(ressources);
                RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(ressources.Ressources);
                if (info.Type == RessourceTyp.distributionType) {

                    MissingDistributionRessource = true;
                    continue;
                }
                PlayerBilanz.AddConsumption(ressources, Priority);
            }
            foreach (RessourcesValue value in pro.ProductionList) {
                PlayerBilanz.AddTotalProduction(value);
            }
        }

        return true;
    }





    protected override bool SetBuildingID(BuildingDescription desc) {

        base.SetBuildingID(desc);

        BuildingProductionDescription description = (BuildingProductionDescription)desc;

        _productionRessources = description.ProductionRessources.ConvertAll(x => x.Clone());


        return true;
    }


    public override void CheckPriority() {
        if (Priority != CurrentPriority) {
            ChangePriority(CurrentPriority);
        }
    }

    private void ChangePriority(int newPriority) {


        foreach (RessourcesProduction pro in _productionRessources) {
            //PlayerBilanz.Player.RemoveProduction(pro);
            foreach (RessourcesValue ressources in pro.ConsumptionList) {

                PlayerBilanz.RemoveConsumption(ressources, Priority);

            }
            foreach (RessourcesValue value in pro.ConsumptionList) {

                PlayerBilanz.ReturnStoredRessources(value);

            }
            foreach (RessourcesValue value in pro.ProductionList) {
                if (value.Used == true) {
                    PlayerBilanz.ReduceRessourceCap(value);

                }

            }
        }


        foreach (RessourcesProduction pro in _productionRessources) {

            foreach (RessourcesValue ressources in pro.ConsumptionList) {
                //PlayerBilanz.AddTotalConsumption(ressources);
                PlayerBilanz.AddConsumption(ressources, newPriority);
            }
        }

        Priority = newPriority;
        PlayerBilanz.CheckRessourceBilanz();

    }

    public override bool CompleteBuildingRemoval() {
        foreach (RessourcesProduction pro in _productionRessources) {
            //PlayerBilanz.Player.RemoveProduction(pro);
            foreach (RessourcesValue ressources in pro.ConsumptionList) {
                RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(ressources.Ressources);
                if (info.Type == RessourceTyp.distributionType) {

                    MissingDistributionRessource = false;
                    continue;
                }

                PlayerBilanz.RemoveConsumption(ressources, Priority);
                PlayerBilanz.ReturnStoredRessources(ressources);
            }

            foreach (RessourcesValue value in pro.ProductionList) {
                if (value.Used == true) {
                    PlayerBilanz.ReduceRessourceCap(value);

                }
                PlayerBilanz.RemoveTotalProduction(value);
            }

        }

        PlayerBilanz.CheckRessourceBilanz();


        return true;
    }


    public override RessourcesProduction GetProductionInformationPerMinute(bool useEfficiency = false, int index = 0) {
        RessourcesProduction production = new RessourcesProduction();

        RessourcesProduction pro = ProductionRessources[index];

        RessourcesValue addValue;
        production.Efficiency = 1;
        foreach (RessourcesValue value in pro.ConsumptionList) {
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

        foreach (RessourcesValue value in pro.ProductionList) {

            RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(value.Ressources);
            if (info.Type == RessourceTyp.limitType) {
                addValue = new RessourcesValue(value.Ressources, value.Value);
            }
            else if (useEfficiency == false) {
                addValue = new RessourcesValue(value.Ressources, value.Value * (60 / EconemySystemInfo.Instanz.TickTimeInSeconds));
            }
            else {
                addValue = new RessourcesValue(value.Ressources, value.Value * (60 / EconemySystemInfo.Instanz.TickTimeInSeconds) * production.Efficiency);
            }

            production.ProductionList.Add(addValue);
        }

        return production;
    }





    public override void BuildingTick() {



        foreach (RessourcesProduction pro in _productionRessources) {
            bool missingRessources = false;
            pro.Efficiency = 1;
            foreach (RessourcesValue value in pro.ConsumptionList) {
                RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(value.Ressources);
                if (info.Type == RessourceTyp.limitType) {
                    if (value.Stored == 0) {
                        missingRessources = true;
                        pro.Efficiency = 0;
                        break;
                    }
                    pro.Efficiency = pro.Efficiency * (value.Stored / value.Value);
                    if (pro.Efficiency > 1) {
                        pro.Efficiency = 1;
                    }

                }
                else if (info.Type == RessourceTyp.distributionType && InputValue.Value.Ressources == value.Ressources) {
                    if (InputValue.Value.Value != 0) {
                        if (InputValue.ID != PlayerBilanz.CurrentDistributionID) {
                            InputValue.Value.Value = 0;
                            MissingDistributionRessource = true;
                            missingRessources = true;
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

                bool returnRessources = false;

                foreach (RessourcesValue ressources in pro.ProductionList) {

                    if (ressources.Used) {
                        continue;
                    }
                    RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(ressources.Ressources);


                    RessourcesValue have = PlayerBilanz.GetRessourcesValue(ressources.Ressources);
                    RessourcesValue toAdd = PlayerBilanz.GetProductionToAdd(ressources.Ressources);


                    if (info.Type == RessourceTyp.limitType) {
                        toAdd.MaxValue = toAdd.MaxValue + ressources.Value;
                        ressources.Used = true;
                    }
                    else if (info.Type == RessourceTyp.distributionType) {
                        if (OutputValue == null) {
                            OutputValue = new RessourcesValue(ressources.Ressources);
                            PlayerBilanz.DistributionChanged();
                        }
                        if (OutputValue.Value != ressources.Value * pro.Efficiency) {
                            OutputValue.Value = ressources.Value * pro.Efficiency;
                            PlayerBilanz.DistributionChanged();
                        }

                    }
                    else {
                        if (have.MaxValue < (have.Value + toAdd.Value + (ressources.Value * pro.Efficiency))) {
                            returnRessources = true;
                            if (EconemySystem.ShowDebugLogs) {

                            }
                        }
                        else {
                            toAdd.Value = toAdd.Value + (ressources.Value * pro.Efficiency);
                        }

                    }

                }
                if (returnRessources == false) {
                    foreach (RessourcesValue ressources in pro.ConsumptionList) {
                        RessourceInfo info = EconemySystemInfo.Instanz.GetRessourceDescription(ressources.Ressources);
                        if (info.Type == RessourceTyp.limitType || info.Type == RessourceTyp.distributionType) {
                            continue;
                        }
                        ressources.UseStored();

                    }
                }


            }



        }

    }

    public override bool BuildingPlaced(PlayerBilanzInfo playerBilanz, BuildingContainer parent = null, bool buildWithoutRessource = false) {

        if (base.BuildingPlaced(playerBilanz, parent, buildWithoutRessource) == true) {
            playerBilanz.DistributionChanged();
            return true;
        }
        return false;
    }
}
