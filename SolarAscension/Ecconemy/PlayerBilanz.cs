
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class PlayerBilanz : MonoBehaviour, SolarAscension.Input.ICheatActionsActions {

    [SerializeField] private PlayerBilanzInfo _player;

    private SolarAscension.Input controller;

    public PlayerBilanzInfo Player {
        get {
            if (_player == null) {
                _player = new PlayerBilanzInfo();
            }
            return _player;
        }

        set {
            _player = value;
        }
    }


    private void Start() {
        if (controller == null) {
            controller = new SolarAscension.Input();
            controller.CheatActions.Enable();
            controller.CheatActions.SetCallbacks(this);
        }
    }

    public void OnAddAluminium(InputAction.CallbackContext context) {
        if (context.started) {
            RessourcesValue value = new RessourcesValue(Ressources.Aluminium, 5000);
            _player.AddingRessourceValueLocked(value);
        }
    }

    public void OnAddEnergy(InputAction.CallbackContext context) {
        if (context.started) {
            RessourcesValue value = new RessourcesValue(Ressources.Energy, 9999);
            _player.AddRessourceCapLocked(value);

        }
    }

    public void OnAddMoney(InputAction.CallbackContext context) {
        if (context.started) {
            RessourcesValue value = new RessourcesValue(Ressources.Money, 999999);
            _player.AddingRessourceValueLocked(value);
        }
    }

    public void OnAddOxygen(InputAction.CallbackContext context) {
        //if (context.started) {
        //    RessourcesValue value = new RessourcesValue(Ressources.Oxygen, 9999);
        //    _player.AddingRessourceValueLocked(value);
        //}
    }

    public void OnAddWater(InputAction.CallbackContext context) {
        if (context.started) {
            RessourcesValue value = new RessourcesValue(Ressources.Water, 5000);
            _player.AddingRessourceValueLocked(value);
        }
    }

    public void OnAddPolymer(InputAction.CallbackContext context) {
        if (context.started) {
            RessourcesValue value = new RessourcesValue(Ressources.BioPolymer, 5000);
            _player.AddingRessourceValueLocked(value);
        }
    }

    public void OnAddOil(InputAction.CallbackContext context) {
        if (context.started) {
            RessourcesValue value = new RessourcesValue(Ressources.Oil, 5000);
            _player.AddingRessourceValueLocked(value);
        }
    }

    public void OnAddVegtables(InputAction.CallbackContext context) {
        if (context.started) {
            RessourcesValue value = new RessourcesValue(Ressources.Vegtables, 5000);
            _player.AddingRessourceValueLocked(value);
        }
    }

    public void OnAddWorkforce(InputAction.CallbackContext context) {
        //if (context.started) {
        //    RessourcesValue value = new RessourcesValue(Ressources.WorkforceEcology, 9999);
        //    _player.AddRessourceCapLocked(value);

        //}
    }

    private void OnDestroy() {
        controller.Dispose();
    }

}
