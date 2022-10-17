using UnityEngine;

public class DroneTarget : MonoBehaviour {
    public DroneModul modul;



    private void Start() {
        if (modul.MoveInPoint != null) {
            GridCoordinate cord = modul.MoveInPoint.transform.position.ToGridCoordinate();
            modul.MoveInPoint.Info.pos = new Vector3Int(cord.x, cord.y, cord.z);

            DroneSystemInfo.Instanz.Waypointgrid.Add(modul.MoveInPoint.Info.pos, modul.MoveInPoint.Info);
        }
        if (modul.MoveOutPoint != null) {
            GridCoordinate cord = modul.MoveOutPoint.transform.position.ToGridCoordinate();
            modul.MoveOutPoint.Info.pos = new Vector3Int(cord.x, cord.y, cord.z);

            DroneSystemInfo.Instanz.Waypointgrid.Add(modul.MoveOutPoint.Info.pos, modul.MoveOutPoint.Info);


        }

        if (modul.InsidePath != null) {

            foreach (WaypointInfo info in modul.InsidePath.path) {

                Waypoints point = info.GameObject.GetComponent<Waypoints>();
                point.Info = info;

            }
        }
    }
}
