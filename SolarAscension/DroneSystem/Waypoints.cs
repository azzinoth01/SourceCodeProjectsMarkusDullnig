using System;
using UnityEngine;

[Serializable]
public class Waypoints : MonoBehaviour {





    [SerializeField] private WaypointInfo _info;



    public WaypointInfo Info {
        get {
            return _info;
        }

        set {
            _info = value;
        }
    }





    private void Update() {


        if (Info.state == WaypointState.blocked) {

            if (Info.PathsOnThisWaypoint.Count != 0) {
                BlockPaths();
                HandelDrones();
            }
        }


        if (Info.DronesInQueue.Count != 0 && Info.CurrentDroneOnWaypoint == null && Info.state != WaypointState.blocked) {
            (Drone, WaypointInfo, WaypointInfo) pair = Info.DronesInQueue.Peek();
            Drone d = pair.Item1;

            WaypointInfo previousWaypoint = pair.Item2;
            WaypointInfo nextWaypoint = pair.Item3;

            if (d == null) {
                Info.DronesInQueue.Dequeue();
                return;
            }

            if (previousWaypoint == null && nextWaypoint != null) {
                if (WaypointInfo.CheckNextWaypointForDrone(d, nextWaypoint) == false) {
                    return;
                }
            }
            else if (previousWaypoint != null) {
                if (previousWaypoint.CurrentDroneOnWaypoint != d || previousWaypoint.WaypointReachedByDrone == false) {
                    return;
                }
            }

            Info.CurrentDroneOnWaypoint = d;
            d._canMoveTowardsNextWaypoint = true;
            d._moveToWaypoint = Info;

            if (d._previousMoveToWaypoint != null) {
                d._previousMoveToWaypoint.CurrentDroneOnWaypoint = null;
                d._previousMoveToWaypoint.WaypointReachedByDrone = false;
            }
            d.stopMovementTime = Info.WaitTime;
            Info.DronesInQueue.Dequeue();





        }
    }

    public void BlockPaths() {
        for (int i = 0; i < Info.PathsOnThisWaypoint.Count;) {

            Info.PathsOnThisWaypoint[i].DestroyPathPair();
        }


    }

    public void HandelDrones() {
        if (Info.CurrentDroneOnWaypoint != null) {
            Destroy(Info.CurrentDroneOnWaypoint.gameObject);
        }

        Info.DronesInQueue.Clear();
    }

}
