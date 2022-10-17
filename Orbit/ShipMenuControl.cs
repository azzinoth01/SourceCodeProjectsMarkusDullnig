using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// handels the keybind for exit inside the ship menu
/// </summary>
public class ShipMenuControl : MonoBehaviour, Controlls.IBullet_hellActions
{
    private Controlls controll;

    /// <summary>
    /// here not in use
    /// </summary>
    /// <param name="context"></param>
    public void OnDoge(InputAction.CallbackContext context) {
        // throw new System.NotImplementedException();
    }
    /// <summary>
    /// here not in use
    /// </summary>
    /// <param name="context"></param>
    public void OnMove_down(InputAction.CallbackContext context) {
        // throw new System.NotImplementedException();
    }
    /// <summary>
    /// here not in use
    /// </summary>
    /// <param name="context"></param>
    public void OnMove_left(InputAction.CallbackContext context) {
        // throw new System.NotImplementedException();
    }
    /// <summary>
    /// here not in use
    /// </summary>
    /// <param name="context"></param>
    public void OnMove_rigth(InputAction.CallbackContext context) {
        //throw new System.NotImplementedException();
    }
    /// <summary>
    /// here not in use
    /// </summary>
    /// <param name="context"></param>
    public void OnMove_up(InputAction.CallbackContext context) {
        //  throw new System.NotImplementedException();
    }
    /// <summary>
    /// exits to the main menu
    /// </summary>
    /// <param name="context"></param>
    public void OnPause_menu(InputAction.CallbackContext context) {

        if (context.started) {
            Globals.menuHandler.onClickMainMenu(0);
        }

    }
    /// <summary>
    /// here not in use
    /// </summary>
    /// <param name="context"></param>
    public void OnShoot(InputAction.CallbackContext context) {
        // throw new System.NotImplementedException();
    }

    /// <summary>
    /// initialises the controls
    /// </summary>
    void Start() {
        if (controll == null) {
            controll = new Controlls();

            Rebinding_menu rebind = new Rebinding_menu();
            controll = rebind.loadRebinding(controll);

            controll.bullet_hell.Enable();
            controll.bullet_hell.SetCallbacks(this);



        }
    }

    /// <summary>
    /// disposes the controls
    /// </summary>
    private void OnDestroy() {
        controll.Dispose();
        controll = null;
    }
}
