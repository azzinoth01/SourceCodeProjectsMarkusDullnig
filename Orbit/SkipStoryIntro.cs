using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// class to skip the story intro
/// </summary>
public class SkipStoryIntro : MonoBehaviour, Controlls.IBullet_hellActions
{
    /// <summary>
    /// story intro object
    /// </summary>
    public StoryIntro intro;
    private Controlls controll;

    /// <summary>
    /// not in use here
    /// </summary>
    /// <param name="context"></param>
    public void OnDoge(InputAction.CallbackContext context) {
        // throw new System.NotImplementedException();
    }
    /// <summary>
    /// not in use here
    /// </summary>
    /// <param name="context"></param>
    public void OnMove_down(InputAction.CallbackContext context) {
        //throw new System.NotImplementedException();
    }
    /// <summary>
    /// not in use here
    /// </summary>
    /// <param name="context"></param>
    public void OnMove_left(InputAction.CallbackContext context) {
        // throw new System.NotImplementedException();
    }
    /// <summary>
    /// not in use here
    /// </summary>
    /// <param name="context"></param>
    public void OnMove_rigth(InputAction.CallbackContext context) {
        //throw new System.NotImplementedException();
    }
    /// <summary>
    /// not in use here
    /// </summary>
    /// <param name="context"></param>
    public void OnMove_up(InputAction.CallbackContext context) {
        // throw new System.NotImplementedException();
    }
    /// <summary>
    /// skips the story intro
    /// </summary>
    /// <param name="context"></param>
    public void OnPause_menu(InputAction.CallbackContext context) {
        if (context.started) {
            intro.Skip = true;
        }
    }
    /// <summary>
    /// not in use here
    /// </summary>
    /// <param name="context"></param>
    public void OnShoot(InputAction.CallbackContext context) {
        //  throw new System.NotImplementedException();
    }

    /// <summary>
    /// sets the controler 
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
    /// disposes the controler
    /// </summary>
    private void OnDestroy() {
        controll.Dispose();
        controll = null;
    }


}
