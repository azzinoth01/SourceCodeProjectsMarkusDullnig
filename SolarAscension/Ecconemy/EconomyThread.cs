using SolarAscension;
using System.Collections.Generic;
using System.IO;
using System.Threading;

public class EconomyThread {
    private int _ticktime;
    private List<Building> _buildingList;



    public static Queue<(Building, bool)> AddBuildingQueue;


    private int _tick;
    private List<Ressources> _ressourceList;
    private List<BuildingDescription> _buildingTyps;

    private string _logSavePath;

    private CancellationToken _token;


    public EconomyThread(CancellationToken token, float ticktime, string logSavePath = "") {

        _token = token;

        _ticktime = (int)(1000 * ticktime);

        _buildingList = new List<Building>();
        AddBuildingQueue = new Queue<(Building, bool)>();

        _tick = 0;
        _logSavePath = logSavePath;
        CreateLogHeader();
        LogBilanz();
    }


    public void SetNewToken(CancellationToken token) {
        _token = token;
    }




    public void ThreadLoop() {


        while (_token.IsCancellationRequested == false) {
            Thread.Sleep(_ticktime);

            if (_token.IsCancellationRequested) {
                break;
            }




            List<Building> removelist = new List<Building>();
            List<Building> addList = new List<Building>();



            while (AddBuildingQueue.Count != 0) {
                (Building, bool) pair = AddBuildingQueue.Dequeue();
                if (pair.Item2 == true) {
                    addList.Add(pair.Item1);
                }
                else {
                    removelist.Add(pair.Item1);
                }


            }

            foreach (Building building in addList) {
                if (building.CompleteBuildingPlacement()) {

                    _buildingList.Add(building);

                }
            }


            foreach (Building building in removelist) {
                if (building.CompleteBuildingRemoval()) {
                    _buildingList.Remove(building);

                }

            }





            EconemySystemInfo.Instanz.Distribution();

            foreach (Building building in _buildingList) {

                building.BuildingTick();
                building.CheckPriority();
            }

            foreach (PlayerBilanzInfo info in EconemySystemInfo.Instanz.PlayerList) {
                info.AddProducedValues();
            }

            QuestSystem.updateResourceGoals = true;

            //EconemySystemInfo.Instanz.CheckRessourceQuestGoals();

            _tick = _tick + 1;

            LogBilanz();

            if (_token.IsCancellationRequested) {
                break;
            }

        }


    }

    public void LogBilanz() {
        string log = "\r\n";

        log = log + _tick.ToString();

        foreach (Ressources r in _ressourceList) {
            log = log + ";" + EconemySystemInfo.Instanz.PlayerList[0].GetRessourcesValue(r).Value;
        }
        foreach (BuildingDescription desc in _buildingTyps) {
            int count = _buildingList.FindAll(x => x.ID == desc.ID).Count;
            log = log + ";" + count.ToString();
        }

        if (File.Exists(_logSavePath + "/Log.csv")) {
            File.AppendAllText(_logSavePath + "/Log.csv", log);
        }
    }


    public void CreateLogHeader() {
        _ressourceList = new List<Ressources>();

        foreach (Ressources ressources in EconemySystemInfo.Instanz.RessourceDescription.Keys) {
            _ressourceList.Add(ressources);
        }

        string header = "Tick";
        foreach (Ressources r in _ressourceList) {
            header = header + ";" + r.ToString();
        }

        _buildingTyps = new List<BuildingDescription>();
        foreach (BuildingDescription desc in EconemySystemInfo.Instanz.BalanceDescription.Values) {
            _buildingTyps.Add(desc);

        }

        foreach (BuildingDescription desc in _buildingTyps) {
            header = header + ";" + desc.Name;
        }

        if (File.Exists(_logSavePath + "/Log.csv")) {
            File.Delete(_logSavePath + "/Log.csv");
            FileStream fileStream = File.Create(_logSavePath + "/Log.csv");
            fileStream.Close();
            File.WriteAllText(_logSavePath + "/Log.csv", header);
        }
        else {
            FileStream fileStream = File.Create(_logSavePath + "/Log.csv");
            fileStream.Close();
            File.WriteAllText(_logSavePath + "/Log.csv", header);
        }


    }
}
