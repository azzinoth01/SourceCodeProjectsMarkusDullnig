using SolarAscension;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;





[Serializable]
public class EconemySystemInfo {


    private static EconemySystemInfo _instanz;

    [SerializeField] private float _tickTimeInSeconds;
    [SerializeField] private RessourceDescription _ressourceDescriptionData;
    [SerializeField] private BuildingBalanceDescription _balanceDescriptionData;

    [SerializeField] private List<PlayerBilanzInfo> _playerList;

    private Dictionary<Ressources, RessourceInfo> _ressourceDescription;
    private Dictionary<int, BuildingDescription> _balanceDescription;

    [SerializeField] private bool _isSet;

    private EconomyThread _economyThread;
    private Thread _thread;
    private CancellationTokenSource _cancelToken;

    public static EconemySystemInfo Instanz {
        get {
            if (_instanz == null) {
                _instanz = new EconemySystemInfo();
            }
            return _instanz;
        }

    }


    public float TickTimeInSeconds {
        get {
            return _tickTimeInSeconds;
        }

        set {
            _tickTimeInSeconds = value;
        }
    }

    public List<PlayerBilanzInfo> PlayerList {
        get {
            if (_isSet == false) {
                return null;
            }
            return _playerList;
        }

    }

    public bool IsSet {
        get {
            return _isSet;
        }


    }

    public Dictionary<Ressources, RessourceInfo> RessourceDescription {

        get {
            if (_isSet == false) {
                return null;
            }
            return _ressourceDescription;
        }


    }

    public Dictionary<int, BuildingDescription> BalanceDescription {
        get {
            if (_isSet == false) {
                return null;
            }
            return _balanceDescription;
        }


    }

    public CancellationTokenSource CancelToken {
        get {
            return _cancelToken;
        }


    }

    public EconemySystemInfo() {
        _tickTimeInSeconds = 1;
    }


    public bool SetEconemySystemInfo(float tickTimeInSeconds, RessourceDescription ressourceDescriptionData = null, BuildingBalanceDescription balanceDescriptionData = null, List<PlayerBilanzInfo> playerList = null, bool startThread = true) {

        if (_instanz == null) {
            return false;
        }

        _tickTimeInSeconds = tickTimeInSeconds;
        _ressourceDescriptionData = ressourceDescriptionData;
        _balanceDescriptionData = balanceDescriptionData;
        _playerList = playerList;


        CreateRessourceDictonary();
        CreateBalanceDictonary();
        _isSet = true;

        if (startThread == true) {
            StartEconomyThread();
        }




        return true;
    }





    private void CreateRessourceDictonary() {
        _ressourceDescription = new Dictionary<Ressources, RessourceInfo>();
        foreach (RessourceInfo info in _ressourceDescriptionData.Ressources) {
            _ressourceDescription.Add(info.Ressources, info);

            foreach (PlayerBilanzInfo player in _playerList) {
                RessourcesValue value = new RessourcesValue(info.Ressources, info.Cap);

                player.SetRessourceCap(value);
            }
        }

    }

    public RessourceInfo GetRessourceDescription(Ressources ressources) {
        if (_isSet == false) {
            return null;
        }

        if (_ressourceDescription.TryGetValue(ressources, out RessourceInfo info)) {
            return info;
        }
        else {
            return null;
        }
    }

    private void CreateBalanceDictonary() {
        _balanceDescription = new Dictionary<int, BuildingDescription>();
        foreach (BuildingProductionDescription pro in _balanceDescriptionData.ProductionBuildings) {
            pro.RequiresDistributionRessource = false;
            foreach (RessourcesProduction production in pro.ProductionRessources) {
                bool breaking = false;
                foreach (RessourcesValue value in production.ConsumptionList) {
                    RessourceInfo info = _ressourceDescription[value.Ressources];
                    if (info.Type == RessourceTyp.distributionType) {
                        pro.RequiresDistributionRessource = true;
                        breaking = true;
                        break;
                    }
                }
                if (breaking == true) {
                    break;
                }
            }
            _balanceDescription.Add(pro.ID, pro);
        }
        foreach (BuildingMiscellaneousDescription misc in _balanceDescriptionData.MiscellaneousBuildings) {
            _balanceDescription.Add(misc.ID, misc);
        }
        foreach (BuildingContainerDescription cont in _balanceDescriptionData.ContainerBuildings) {
            _balanceDescription.Add(cont.ID, cont);
        }
        foreach (BuildingPopulationDescription pop in _balanceDescriptionData.PopulationBuildings) {

            pop.RequiresDistributionRessource = false;
            foreach (NeedsAndWants need in pop.NeedsAndWants) {
                if (need.BuildingID == 0) {
                    RessourceInfo info = _ressourceDescription[need.Value.Ressources];
                    if (info.Type == RessourceTyp.distributionType) {
                        pop.RequiresDistributionRessource = true;
                        break;
                    }
                }

            }


            _balanceDescription.Add(pop.ID, pop);
        }
        foreach (BuildingUtilityDescription utility in _balanceDescriptionData.UtilityBuildings) {
            utility.RequiresDistributionRessource = false;

            foreach (RessourcesValue value in utility.ConsumptionList) {
                RessourceInfo info = _ressourceDescription[value.Ressources];
                if (info.Type == RessourceTyp.distributionType) {
                    utility.RequiresDistributionRessource = true;
                    break;
                }

            }
            _balanceDescription.Add(utility.ID, utility);
        }
    }


    public BuildingDescription GetBuildingProductionDescription(int ID) {
        if (_isSet == false) {
            return null;
        }

        if (_balanceDescription.TryGetValue(ID, out BuildingDescription value)) {
            return value;
        }
        else {
            return null;
        }

    }

    private void RefreshLimitValues() {
        if (_isSet == false) {
            return;
        }

        foreach (RessourceInfo info in _ressourceDescription.Values) {

            if (info.Type != RessourceTyp.limitType) {
                continue;
            }
            foreach (PlayerBilanzInfo bilanz in _playerList) {
                RessourcesValue value = bilanz.GetRessourcesValue(info.Ressources);
                RessourcesValue total = bilanz.GetTotalConsumption(info.Ressources);
                if (value != null && total != null) {
                    if (value.Value < (value.MaxValue - total.Value) && value.Value > 0) {
                        value.Value = value.MaxValue - total.Value;
                    }

                }
            }
        }
    }

    public bool StartEconomyThread() {


        if (_economyThread == null) {


            if (_cancelToken == null) {
                _cancelToken = new CancellationTokenSource();
            }
            _economyThread = new EconomyThread(_cancelToken.Token, EconemySystemInfo.Instanz.TickTimeInSeconds, Application.persistentDataPath);



        }

        if (_thread == null || _thread.IsAlive == false) {

            if (_cancelToken != null) {
                _cancelToken.Dispose();
            }

            _cancelToken = new CancellationTokenSource();
            _economyThread.SetNewToken(_cancelToken.Token);

            _thread = new Thread(_economyThread.ThreadLoop);

            _thread.Start();


            return true;
        }

        return false;
    }



    public void Distribution() {
        if (_isSet == false) {
            return;
        }

        RefreshLimitValues();
        foreach (PlayerBilanzInfo bilanz in _playerList) {

            bilanz.PriorityConsumption();
        }


    }


    public void CheckRessourceQuestGoals() {
        foreach (PlayerBilanzInfo bilanz in _playerList) {

            foreach (RessourceInfo ressourceinfo in _ressourceDescriptionData.Ressources) {
                if (ressourceinfo.Type == RessourceTyp.limitType) {
                    QuestSystem.Instance.InvokeGoalUpdate(QuestSystem.RessourcesToQuestGoal(ressourceinfo.Ressources), 0, (int)bilanz.GetRessourcesValue(ressourceinfo.Ressources).MaxValue);

                }
                else {
                    QuestSystem.Instance.InvokeGoalUpdate(QuestSystem.RessourcesToQuestGoal(ressourceinfo.Ressources), 0, (int)bilanz.GetRessourcesValue(ressourceinfo.Ressources).Value);

                }


            }
        }
    }

}
