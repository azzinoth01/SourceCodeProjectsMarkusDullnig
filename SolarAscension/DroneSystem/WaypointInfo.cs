using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class WaypointInfo {


    [SerializeField] private GameObject _gameObject;
    [SerializeField] public Vector3Int pos;
    [SerializeField] public WaypointState state;
    [SerializeField] private bool _waypointCreated;
    [SerializeField] public int baseValue;
    [SerializeField] private int _currentValue;
    [NonSerialized] private List<WaypointConnection> _outGoingConnections;
    [NonSerialized] private List<WaypointConnection> _inGoingConnections;

    private Queue<(Drone, WaypointInfo, WaypointInfo)> _dronesInQueue;
    private bool _waypointReachedByDrone;
    [SerializeField] private float _waitTime;

    [SerializeField] private Drone _currentDroneOnWaypoint;
    [NonSerialized] private List<PathInfo> _pathsOnThisWaypoint;


    public List<WaypointConnection> OutGoingConnections {
        get {
            return _outGoingConnections;
        }

        set {
            _outGoingConnections = value;
        }
    }

    public List<WaypointConnection> InGoingConnections {
        get {
            return _inGoingConnections;
        }

        set {
            _inGoingConnections = value;
        }
    }

    public Drone CurrentDroneOnWaypoint {
        get {
            return _currentDroneOnWaypoint;
        }

        set {
            _currentDroneOnWaypoint = value;
        }
    }

    public GameObject GameObject {
        get {
            return _gameObject;
        }

        set {
            _gameObject = value;
        }
    }

    public Queue<(Drone, WaypointInfo, WaypointInfo)> DronesInQueue {
        get {
            return _dronesInQueue;
        }


    }

    public bool WaypointReachedByDrone {
        get {
            return _waypointReachedByDrone;
        }

        set {
            _waypointReachedByDrone = value;
        }
    }

    public float WaitTime {
        get {
            return _waitTime;
        }

        set {
            _waitTime = value;
        }
    }

    public List<PathInfo> PathsOnThisWaypoint {
        get {
            return _pathsOnThisWaypoint;
        }

        set {
            _pathsOnThisWaypoint = value;
        }
    }

    public int CurrentValue {
        get {
            return _currentValue;
        }

        set {
            _currentValue = value;
        }
    }

    public bool WaypointCreated {
        get {
            return _waypointCreated;
        }

        set {
            _waypointCreated = value;
        }
    }

    public WaypointInfo(Vector3Int pos, int baseValue) {
        this.pos = pos;
        state = WaypointState.open;
        this.baseValue = baseValue;
        CurrentValue = baseValue;

        _outGoingConnections = new List<WaypointConnection>();
        _inGoingConnections = new List<WaypointConnection>();
        _currentDroneOnWaypoint = null;
        _dronesInQueue = new Queue<(Drone, WaypointInfo, WaypointInfo)>();
        WaitTime = 0;
        PathsOnThisWaypoint = new List<PathInfo>();
        WaypointReachedByDrone = false;

    }
    public WaypointInfo() {

        state = WaypointState.open;

        CurrentValue = baseValue;

        _outGoingConnections = new List<WaypointConnection>();
        _inGoingConnections = new List<WaypointConnection>();
        _currentDroneOnWaypoint = null;
        _dronesInQueue = new Queue<(Drone, WaypointInfo, WaypointInfo)>();
        WaitTime = 0;
        PathsOnThisWaypoint = new List<PathInfo>();
        WaypointReachedByDrone = false;

    }


    public static bool CheckNextWaypointForDrone(Drone drone, WaypointInfo waypoint) {


        (Drone, WaypointInfo, WaypointInfo) pair = waypoint.DronesInQueue.Peek();

        Drone d = pair.Item1;
        //WaypointInfo previousWaypoint = pair.Item2;
        WaypointInfo nextWaypoint = pair.Item3;


        if (d != drone) {
            return false;
        }

        if (nextWaypoint == null) {
            return true;
        }
        else {
            return CheckNextWaypointForDrone(drone, nextWaypoint);
        }



    }

    public void CheckConnections() {

        InGoingConnections = new List<WaypointConnection>();
        OutGoingConnections = new List<WaypointConnection>();

        foreach (PathInfo paths in PathsOnThisWaypoint) {
            for (int index = 0; index < paths.path.Count;) {


                if (paths.path[index] == this) {
                    if (index + 1 < paths.path.Count) {
                        bool breaking = false;
                        foreach (WaypointConnection con in OutGoingConnections) {
                            if (con.connectTo == paths.path[index + 1]) {
                                breaking = true;
                                break;
                            }
                        }
                        if (breaking == false) {
                            WaypointConnection con = new WaypointConnection();
                            con.connectTo = paths.path[index + 1];
                            con.state = PathState.oneWay;
                            con.value = baseValue;
                            OutGoingConnections.Add(con);
                        }
                    }
                    if (index - 1 > -1) {
                        bool breaking = false;
                        foreach (WaypointConnection con in InGoingConnections) {
                            if (con.connectTo == paths.path[index - 1]) {
                                breaking = true;
                                break;
                            }
                        }
                        if (breaking == false) {
                            WaypointConnection con = new WaypointConnection();
                            con.connectTo = paths.path[index - 1];
                            con.state = PathState.oneWay;
                            con.value = baseValue;
                            InGoingConnections.Add(con);
                        }
                    }
                    break;
                }

                index = index + 1;
            }
        }

        foreach (WaypointConnection outCon in OutGoingConnections) {
            foreach (WaypointConnection inCon in InGoingConnections) {
                if (outCon.connectTo == inCon.connectTo) {

                    inCon.state = PathState.bothWays;
                    inCon.value = baseValue * 300;

                    outCon.state = PathState.bothWays;
                    outCon.value = baseValue * 300;
                }
            }
        }
    }
}
