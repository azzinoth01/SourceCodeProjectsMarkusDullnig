using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class EconemySystem : MonoBehaviour {

    private static EconemySystem Instanz;

    [SerializeField] private float _tickTimeInSeconds;
    [SerializeField] private RessourceDescription _ressourceDescriptionData;
    [SerializeField] private BuildingBalanceDescription _balanceDescriptionData;
    [SerializeField] private StartCapital _startCapitalData;


    [SerializeField] private List<PlayerBilanz> _playerList;




    private EconemySystemInfo _econemySystemInfo;

    [Header("Debug Flags")]
    public bool showEconomyLogs;

    public static bool ShowDebugLogs {
        get; private set;
    }


    private void Awake() {
        ShowDebugLogs = showEconomyLogs;
        if (Instanz == null) {
            Instanz = this;
            DontDestroyOnLoad(gameObject);
            //_playerList = new List<PlayerBilanz>();
        }
        else {
            Destroy(gameObject);
        }

        //CreateRessourceDictonary();
        //CreateBalanceDictonary();

        _econemySystemInfo = EconemySystemInfo.Instanz;

        _econemySystemInfo.SetEconemySystemInfo(_tickTimeInSeconds, _ressourceDescriptionData, _balanceDescriptionData, _playerList.Select(x => x.Player).ToList(), false);

        foreach (PlayerBilanz info in _playerList) {
            foreach (RessourcesValue value in _startCapitalData.StartValues) {
                info.Player.AddingRessourceValueLocked(value);
            }
        }
    }

    private void OnDestroy() {


        EconemySystemInfo.Instanz.CancelToken.Cancel();

    }


    public void SetPause(bool pause) {

        if (pause == true) {
            EconemySystemInfo.Instanz.CancelToken.Cancel();
            enabled = false;
        }
        else {
            enabled = true;
        }

    }

    private void Update() {

        if (EconemySystemInfo.Instanz.StartEconomyThread()) {


            enabled = false;
        }
    }


}
