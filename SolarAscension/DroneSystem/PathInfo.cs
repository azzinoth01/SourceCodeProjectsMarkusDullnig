using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PathInfo {

    [SerializeField] public uint pathID;
    [SerializeField] public List<WaypointInfo> path;
    [NonSerialized] public List<Drone> drones;
    [NonSerialized] public List<Drone> allDronesUsingThisPath;
    [NonSerialized] public DroneModul endModul;
    [NonSerialized] public DroneModul startModul;
    [NonSerialized] public DroneModul originModul;

    private uint _returnPathID;

    public PathInfo(List<WaypointInfo> path) {
        this.path = path;
        drones = new List<Drone>();
        allDronesUsingThisPath = new List<Drone>();
        foreach (WaypointInfo w in path) {
            w.PathsOnThisWaypoint.Add(this);


            DroneSystemInfo.Instanz.Waypointgrid.TryAdd(w.pos, w);
            if (DroneSystemInfo.Instanz.WaypointList.Contains(w) == false) {
                DroneSystemInfo.Instanz.WaypointList.Add(w);
            }

        }

        pathID = DroneSystemInfo.Instanz.PathID;
    }
    public PathInfo() {
        drones = new List<Drone>();
        allDronesUsingThisPath = new List<Drone>();
        pathID = DroneSystemInfo.Instanz.PathID;

    }


    public void DestroyPathPair() {



        PathInfo pathing = originModul.Path;
        PathInfo returnPath = originModul.ReturnPath;

        foreach (WaypointInfo w in pathing.path) {

            w.PathsOnThisWaypoint.Remove(pathing);
            if (w.PathsOnThisWaypoint.Count == 0) {
                w.CurrentValue = DroneSystemInfo.Instanz.Basevalue;
            }
            w.CheckConnections();
        }
        foreach (WaypointInfo w in returnPath.path) {
            w.PathsOnThisWaypoint.Remove(returnPath);
            if (w.PathsOnThisWaypoint.Count == 0) {
                w.CurrentValue = DroneSystemInfo.Instanz.Basevalue;
            }
            //w.currentValue = w.currentValue - (w.baseValue * 100);
            w.CheckConnections();
        }

        pathing.ClearDroneOnPath();
        returnPath.ClearDroneOnPath();

        originModul.Path = null;
        originModul.ReturnPath = null;

        originModul.CalcPath();

    }

    public void DestroyPathWithoutRecalculation() {
        foreach (WaypointInfo w in path) {

            w.PathsOnThisWaypoint.Remove(this);
            if (w.PathsOnThisWaypoint.Count == 0) {
                w.CurrentValue = DroneSystemInfo.Instanz.Basevalue;
            }
            w.CheckConnections();
        }

        ClearDroneOnPath();
    }

    public void ClearDroneOnPath() {

        foreach (Drone d in allDronesUsingThisPath) {
            if (d != null) {
                d.DestroyThisDrone();
            }

        }
        allDronesUsingThisPath.Clear();
        drones.Clear();
    }



}

