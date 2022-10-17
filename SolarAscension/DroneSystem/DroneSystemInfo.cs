using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DroneSystemInfo {
    private static DroneSystemInfo _instanz;

    private uint _pathID;
    private Dictionary<Vector3Int, WaypointInfo> _waypointgrid;
    private List<WaypointInfo> _waypointList;


    [SerializeField] private Waypoints _waypointPrefab;
    [SerializeField] private Drone _dronePrefab;
    [SerializeField] private Drone _iceMinerDronesPrefab;

    [SerializeField] private int _basevalue;
    [SerializeField] private int _droneSpeed;

    private List<DroneModul> _moduls;

    [SerializeField] private List<DroneTarget> _scrapFieldList;
    [SerializeField] private List<DroneTarget> _iceFieldList;

    private List<DroneModul> _scrapField;
    private List<DroneModul> _iceField;

    [SerializeField] private PathGenerator _pathGenerator;


    [SerializeField] private Player _player;

    //[SerializeField] public GameObject LockedPosition;

    [SerializeField] private GameObject _droneContainer;

    [SerializeField] private int _maxDroneAmountPerModul;

    [SerializeField] private float _droneSpeedRadius;


    public static DroneSystemInfo Instanz {
        get {
            if (_instanz == null) {
                _instanz = new DroneSystemInfo();
            }
            return _instanz;
        }
    }

    public Dictionary<Vector3Int, WaypointInfo> Waypointgrid {
        get {
            return _waypointgrid;
        }


    }


    public Waypoints WaypointPrefab {
        get {
            return _waypointPrefab;
        }


    }

    public int Basevalue {
        get {
            return _basevalue;
        }


    }

    public int DroneSpeed {
        get {
            return _droneSpeed;
        }


    }

    public Drone DronePrefab {
        get {
            return _dronePrefab;
        }


    }



    public uint PathID {
        get {
            _pathID = _pathID + 1;
            return _pathID;
        }


    }



    public List<DroneModul> Moduls {
        get {
            return _moduls;
        }

    }

    public List<DroneModul> ScrapField {
        get {
            return _scrapField;
        }


    }



    public PathGenerator PathGenerator {
        get {
            return _pathGenerator;
        }

    }


    public Player Player {
        get {
            return _player;
        }


    }

    public List<WaypointInfo> WaypointList {
        get {
            return _waypointList;
        }


    }

    public Drone IceMinerDronesPrefab {
        get {
            return _iceMinerDronesPrefab;
        }


    }

    public GameObject DroneContainer {
        get {
            return _droneContainer;
        }


    }

    public List<DroneModul> IceField {
        get {
            return _iceField;
        }


    }

    public int MaxDroneAmountPerModul {
        get {
            return _maxDroneAmountPerModul;
        }

        set {
            _maxDroneAmountPerModul = value;
        }
    }

    public List<DroneTarget> ScrapFieldList {
        get {
            return _scrapFieldList;
        }


    }

    public List<DroneTarget> IceFieldList {
        get {
            return _iceFieldList;
        }


    }

    public float DroneSpeedRadius {
        get {
            return _droneSpeedRadius;
        }

        set {
            _droneSpeedRadius = value;
        }
    }



    public void SetupDroneSystemInfo(Drone dronePrefab, Waypoints waypointsPrefab, int basevalue, int speed, Player player, Drone iceMinerDronePrefab, List<DroneTarget> iceFieldTarget, List<DroneTarget> scrapFieldTarget) {

        if (_instanz == null) {
            _instanz = Instanz;
        }
        _pathID = 0;

        _droneSpeedRadius = 0;

        _waypointgrid = new Dictionary<Vector3Int, WaypointInfo>();
        _waypointList = new List<WaypointInfo>();


        _scrapField = new List<DroneModul>();
        _iceField = new List<DroneModul>();

        _dronePrefab = dronePrefab;
        _waypointPrefab = waypointsPrefab;
        _basevalue = basevalue;
        _droneSpeed = speed;
        _moduls = new List<DroneModul>();
        _pathGenerator = new PathGenerator();
        _player = player;

        _iceMinerDronesPrefab = iceMinerDronePrefab;

        _droneContainer = new GameObject();
        _droneContainer.name = "Drone Container";
        GameObject.DontDestroyOnLoad(_droneContainer);
        _droneContainer.transform.position = Vector3.zero;

        foreach (DroneTarget target in iceFieldTarget) {
            _iceField.Add(target.modul);
        }

        foreach (DroneTarget target in scrapFieldTarget) {
            _scrapField.Add(target.modul);
        }

        _scrapFieldList = scrapFieldTarget;
        _iceFieldList = iceFieldTarget;

        DroneSystem.Instanz.StartCoroutine(CheckLockedWaypoints());
    }


    public IEnumerator CheckLockedWaypoints() {
        yield return null;
        int checksPerFrame = 100;

        for (int i = 0; i < WaypointList.Count; i++) {

            if (_player.BuildingGrid.LockGrid.IsLocked(new GridCoordinate(WaypointList[i].pos.x, WaypointList[i].pos.y, WaypointList[i].pos.z)) == true) {
                WaypointList[i].state = WaypointState.blocked;


            }
            else if (WaypointList[i].state == WaypointState.blocked) {
                WaypointList[i].state = WaypointState.open;
            }
            checksPerFrame = checksPerFrame - 1;

            if (checksPerFrame < 0) {
                checksPerFrame = 100;
                yield return null;
            }
        }



        DroneSystem.Instanz.StartCoroutine(CheckLockedWaypoints());
    }
}
