using UnityEngine;


public class WaypointConnection {

    public WaypointInfo connectTo;
    [SerializeField] public int value;
    [SerializeField] public PathState state;

    public WaypointConnection() {
        state = PathState.none;
        value = 1;
    }

}
