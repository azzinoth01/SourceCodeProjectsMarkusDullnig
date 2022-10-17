using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DroneModul {

    [SerializeField] public TargetType target;
    private DroneModul _targetModul;

    [SerializeField] private Waypoints _moveOutPoint;

    [SerializeField] private Waypoints _moveInPoint;



    [SerializeField] private PathInfo _path;
    [SerializeField] private PathInfo _returnPath;
    [SerializeField] private PathInfo _insidePath;

    private int _maxDroneCreateAmount;
    private int _currentMaxDroneReduceAmount;
    [NonSerialized] private List<Drone> _dronesCreated;

    private uint _returnPathID;
    private uint _pathID;
    private bool _stopCreatingDrones;


    private ModuleData _moduleData;
    private Transform _parent;
    private GridObject _origin;
    private Building _building;


    private Coroutine _droneCreationCoroutine;
    private bool _isActive;

    public Waypoints MoveOutPoint {
        get {
            return _moveOutPoint;
        }

        set {
            _moveOutPoint = value;
        }
    }

    public Waypoints MoveInPoint {
        get {
            return _moveInPoint;
        }

        set {
            _moveInPoint = value;
        }
    }

    public PathInfo Path {
        get {
            return _path;
        }

        set {
            _path = value;
        }
    }

    public PathInfo ReturnPath {
        get {
            return _returnPath;
        }

        set {
            _returnPath = value;
        }
    }

    public PathInfo InsidePath {
        get {
            return _insidePath;
        }

        set {
            _insidePath = value;
        }
    }

    public List<Drone> DronesCreated {
        get {
            return _dronesCreated;
        }

        set {
            _dronesCreated = value;
        }
    }

    public int CurrentMaxDroneReduceAmount {
        get {
            return _currentMaxDroneReduceAmount;
        }

        set {
            _currentMaxDroneReduceAmount = value;
        }
    }

    public void SetupModul(ModuleData moduleData, Transform parent, GridObject origin, TargetType target) {
        _isActive = true;
        _stopCreatingDrones = false;

        _moduleData = moduleData;
        _origin = origin;
        _parent = parent;


        origin.economyBuilding.activeChanged += ModulSleepModeEvent;

        this.target = target;

        _dronesCreated = new List<Drone>();
        _maxDroneCreateAmount = DroneSystemInfo.Instanz.MaxDroneAmountPerModul;


        CreateMoveInAndOutPoints();


        CreateInsidePath(moduleData, parent);


    }

    public void RelocateModul(GridObject origin) {

        RemoveModul();
        _origin = origin;
        _dronesCreated = new List<Drone>();

        CreateMoveInAndOutPoints();

        DroneSystem.Instanz.StartCoroutine(DelayCalcPath(1));

    }


    private void CreateMoveInAndOutPoints() {
        GridCoordinate moveInPos = new GridCoordinate(_moduleData.droneMoveInPosition.x, _moduleData.droneMoveInPosition.y, _moduleData.droneMoveInPosition.z);


        moveInPos = moveInPos.Rotate(_origin.orientation) + _origin.origin;

        Vector3Int pos = new Vector3Int(moveInPos.x, moveInPos.y, moveInPos.z);
        WaypointInfo info = null;


        if (DroneSystemInfo.Instanz.Waypointgrid.TryGetValue(pos, out info) == false) {
            MoveInPoint = GameObject.Instantiate(DroneSystemInfo.Instanz.WaypointPrefab, _parent, false);
            MoveInPoint.transform.localPosition = MoveInPoint.transform.localPosition + (_moduleData.droneMoveInPosition * 5);
            MoveInPoint.transform.SetParent(null);
            MoveInPoint.Info = new WaypointInfo(pos, DroneSystemInfo.Instanz.Basevalue);
            MoveInPoint.Info.WaypointCreated = true;
            MoveInPoint.Info.GameObject = MoveInPoint.gameObject;
            DroneSystemInfo.Instanz.Waypointgrid.Add(MoveInPoint.Info.pos, MoveInPoint.Info);
            DroneSystemInfo.Instanz.WaypointList.Add(MoveInPoint.Info);
        }
        else {
            MoveInPoint = info.GameObject.GetComponent<Waypoints>();
            MoveInPoint.Info = info;


        }

        MoveInPoint.Info.state = WaypointState.onlyMoveIn;


        info = null;

        GridCoordinate moveOutPos = new GridCoordinate(_moduleData.droneMoveOutPosition.x, _moduleData.droneMoveOutPosition.y, _moduleData.droneMoveOutPosition.z);


        moveOutPos = moveOutPos.Rotate(_origin.orientation) + _origin.origin;

        pos = new Vector3Int(moveOutPos.x, moveOutPos.y, moveOutPos.z);
        if (DroneSystemInfo.Instanz.Waypointgrid.TryGetValue(pos, out info) == false) {
            MoveOutPoint = GameObject.Instantiate(DroneSystemInfo.Instanz.WaypointPrefab, _parent, false);
            MoveOutPoint.transform.localPosition = MoveOutPoint.transform.localPosition + (_moduleData.droneMoveOutPosition * 5);
            MoveOutPoint.transform.SetParent(null);
            MoveOutPoint.Info = new WaypointInfo(pos, DroneSystemInfo.Instanz.Basevalue);
            MoveOutPoint.Info.WaypointCreated = true;
            MoveOutPoint.Info.GameObject = MoveOutPoint.gameObject;
            DroneSystemInfo.Instanz.Waypointgrid.Add(MoveOutPoint.Info.pos, MoveOutPoint.Info);
            DroneSystemInfo.Instanz.WaypointList.Add(MoveOutPoint.Info);
        }
        else {
            MoveOutPoint = info.GameObject.GetComponent<Waypoints>();
            MoveOutPoint.Info = info;


        }

        MoveOutPoint.Info.state = WaypointState.moveOut;

    }


    private void CreateInsidePath(ModuleData moduleData, Transform parent) {
        List<WaypointInfo> waypointList = new List<WaypointInfo>();
        for (int i = 0; i < moduleData.droneStayPoints.Count; i++) {
            Waypoints w = GameObject.Instantiate(DroneSystemInfo.Instanz.WaypointPrefab, parent, false);
            w.transform.localPosition = moduleData.droneStayPoints[i];
            w.Info = new WaypointInfo();
            w.Info.GameObject = w.gameObject;
            w.Info.pos = new Vector3Int(i, 0, 0);
            w.Info.WaypointCreated = true;
            waypointList.Add(w.Info);
        }


        if (waypointList.Count != 0) {
            InsidePath = new PathInfo(waypointList);
        }
    }


    public void RemoveModul(bool freeConnectionPoints = true) {

        _stopCreatingDrones = true;



        Path.DestroyPathWithoutRecalculation();
        Path = null;
        ReturnPath.DestroyPathWithoutRecalculation();
        ReturnPath = null;

        if (freeConnectionPoints == true) {
            if (MoveInPoint != null) {
                MoveInPoint.Info.state = WaypointState.open;
            }
            if (MoveOutPoint != null) {
                MoveOutPoint.Info.state = WaypointState.open;
            }
        }


    }




    public bool CreateDrones() {


        if (Path != null && ReturnPath != null && InsidePath != null && _stopCreatingDrones == false) {


            int currentMaxDroneCreateAmount = Path.path.Count / 2;
            if (currentMaxDroneCreateAmount > _maxDroneCreateAmount) {
                currentMaxDroneCreateAmount = _maxDroneCreateAmount;
            }
            //currentMaxDroneCreateAmount = 5;

            currentMaxDroneCreateAmount = currentMaxDroneCreateAmount - _currentMaxDroneReduceAmount;

            for (int i = InsidePath.path.Count - 2; i >= InsidePath.path.Count - 2; i--) {
                if (DronesCreated.Count >= currentMaxDroneCreateAmount) {
                    return false;
                }
                if (InsidePath.path[i].CurrentDroneOnWaypoint == null) {

                    Drone d;

                    if (target == TargetType.scrapField) {
                        d = GameObject.Instantiate(DroneSystemInfo.Instanz.DronePrefab);
                    }
                    else {
                        d = GameObject.Instantiate(DroneSystemInfo.Instanz.IceMinerDronesPrefab);
                    }

                    DronesCreated.Add(d);
                    d.transform.position = InsidePath.path[i].GameObject.transform.position;
                    d.path = Path;
                    d.returnPath = ReturnPath;
                    d.origin = this;
                    d.startModul = this;
                    d.endModul = Path.endModul;
                    d.PathIndex = i;
                    d.transform.SetParent(DroneSystemInfo.Instanz.DroneContainer.transform);

                    InsidePath.path[i].DronesInQueue.Enqueue((d, null, null));
                    d.enabled = true;
                }
                else {
                    return true;
                }
            }

            return true;
        }
        return false;

    }






    [ContextMenu("Calc Path")]
    public void CalcPath() {

        if (_isActive == false) {
            return;
        }

        _currentMaxDroneReduceAmount = 0;





        _targetModul = null;
        int currentDistance = 0;

        List<DroneModul> targetList;

        if (target == TargetType.scrapField) {
            targetList = DroneSystemInfo.Instanz.ScrapField;
        }
        else if (target == TargetType.iceField) {
            targetList = DroneSystemInfo.Instanz.IceField;
        }
        else {
            return;
        }


        foreach (DroneModul closest in targetList) {

            int xDif = (int)MathF.Abs(MoveOutPoint.Info.pos.x - closest.MoveInPoint.Info.pos.x);
            int yDif = (int)MathF.Abs(MoveOutPoint.Info.pos.y - closest.MoveInPoint.Info.pos.y);
            int zDif = (int)MathF.Abs(MoveOutPoint.Info.pos.z - closest.MoveInPoint.Info.pos.z);

            int distance = (xDif + yDif + zDif) * DroneSystemInfo.Instanz.Basevalue;
            if (_targetModul == null) {
                _targetModul = closest;
                currentDistance = distance;
            }
            else if (currentDistance > distance) {
                _targetModul = closest;
                currentDistance = distance;
            }



        }

        DroneSystemInfo.Instanz.PathGenerator.calcFinished += PathGenerator_calcFinished;
        _pathID = DroneSystemInfo.Instanz.PathGenerator.AddNewPathToQueue(MoveOutPoint.Info, _targetModul.MoveInPoint.Info);
        _returnPathID = DroneSystemInfo.Instanz.PathGenerator.AddNewPathToQueue(_targetModul.MoveOutPoint.Info, MoveInPoint.Info);



    }

    private IEnumerator StartCreatingDrone() {


        while (CreateDrones() == true) {
            yield return new WaitForSeconds(5f);
        }
        _droneCreationCoroutine = null;
    }


    private IEnumerator DelayCalcPath(float delay) {

        yield return new WaitForSeconds(delay);
        _stopCreatingDrones = false;
        CalcPath();

    }

    private void ModulSleepModeEvent(bool state) {
        _isActive = state;
        if (_isActive == true) {
            _stopCreatingDrones = false;
            foreach (Drone d in _dronesCreated) {
                d.DroneFlagedForSleep = false;
            }
            if (_path == null || _returnPath == null) {
                CalcPath();
            }
            else if (_droneCreationCoroutine == null) {
                _droneCreationCoroutine = DroneSystem.Instanz.StartCoroutine(StartCreatingDrone());
            }
        }
        else {
            _stopCreatingDrones = true;
            foreach (Drone d in _dronesCreated) {
                d.DroneFlagedForSleep = true;
            }
        }


    }


    private void PathGenerator_calcFinished((uint, PathInfo) obj) {

        if (obj.Item1 == _pathID) {
            _path = obj.Item2;
            _pathID = 0;
            SetPathInformations(_path, this, this, _targetModul);
        }
        else if (obj.Item1 == _returnPathID) {
            _returnPath = obj.Item2;
            _returnPathID = 0;
            SetPathInformations(_returnPath, this, _targetModul, this);
        }

        if (_pathID == 0 && _returnPathID == 0) {
            DroneSystemInfo.Instanz.PathGenerator.calcFinished -= PathGenerator_calcFinished;
            _droneCreationCoroutine = DroneSystem.Instanz.StartCoroutine(StartCreatingDrone());
        }
    }

    private void SetPathInformations(PathInfo pathing, DroneModul origin, DroneModul start, DroneModul goal) {

        if (pathing == null) {
            return;
        }
        pathing.endModul = goal;
        pathing.startModul = start;

        pathing.originModul = origin;


        foreach (WaypointInfo way in pathing.path) {


            if (way.WaypointCreated == false) {
                Waypoints w = GameObject.Instantiate(DroneSystemInfo.Instanz.WaypointPrefab);

                w.Info = way;
                way.WaypointCreated = true;
                w.transform.position = (way.pos * DroneSystemInfo.Instanz.Basevalue);
                w.Info.GameObject = w.gameObject;
                w.enabled = true;


            }



            way.CheckConnections();


        }

    }


}
