using UnityEngine;



[DefaultExecutionOrder(-1000)]
public class DroneSystem : MonoBehaviour {
    private static DroneSystem _instanz;

    [SerializeField] private DroneSystemInfo _droneSystemInfo;

    public static DroneSystem Instanz {
        get {
            return _instanz;
        }

    }

    private void Awake() {
        if (_instanz == null) {
            _instanz = this;
            DontDestroyOnLoad(gameObject);

        }
        else {
            Destroy(gameObject);
        }




        DroneSystemInfo infoInstanz = DroneSystemInfo.Instanz;

        infoInstanz.SetupDroneSystemInfo(_droneSystemInfo.DronePrefab, _droneSystemInfo.WaypointPrefab, _droneSystemInfo.Basevalue, _droneSystemInfo.DroneSpeed, _droneSystemInfo.Player, _droneSystemInfo.IceMinerDronesPrefab, _droneSystemInfo.IceFieldList, _droneSystemInfo.ScrapFieldList);


        DroneSystemInfo.Instanz.MaxDroneAmountPerModul = _droneSystemInfo.MaxDroneAmountPerModul;


        //infoInstanz.LockedPosition = _droneSystemInfo.LockedPosition;

        _droneSystemInfo = DroneSystemInfo.Instanz;

    }
}
