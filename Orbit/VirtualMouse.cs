using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;


/// <summary>
/// class to createa a virtual mouse
/// </summary>
public class VirtualMouse : MonoBehaviour, Controlls.IVirtualMouseActions
{
    private Mouse virtualMouse;
    private Mouse hardwareMouse;


    private bool hardwareMouseVisible;

    /// <summary>
    /// cursor object
    /// </summary>
    public RectTransform cursor;
    /// <summary>
    /// cursor speed
    /// </summary>
    public float speed;
    private Vector2 direction;

    /// <summary>
    /// Rect transform of the canvas of the cursor
    /// </summary>
    public RectTransform canvasRect;
    /// <summary>
    /// canvas of the cursor
    /// </summary>
    public Canvas canvas;

    private Controlls controls;

    private Vector2 lastMousePos;
    private Vector2 currentMousePos;
    private Vector2 deltaMouse;


    private bool mousePositionUpdated;

    private Vector2 buttonScroll;

    private bool unlockMouse;



    /// <summary>
    /// returns the virtual mouse device object
    /// </summary>
    public Mouse VirtualMouseProperty {
        get {
            return virtualMouse;
        }


    }
    /// <summary>
    /// creates a virtual mouse
    /// </summary>
    private void OnEnable() {
        hardwareMouse = Mouse.current;
        buttonScroll = Vector2.zero;
        unlockMouse = false;



        Cursor.visible = hardwareMouseVisible;
        Cursor.lockState = CursorLockMode.Confined;
        Vector2 start = new Vector2(Screen.width / 2, Screen.height / 2);

        if (controls == null) {
            controls = new Controlls();

            Rebinding_menu rebind = new Rebinding_menu();
            controls = rebind.loadRebinding(controls);

            controls.VirtualMouse.Enable();
            controls.VirtualMouse.SetCallbacks(this);
        }

        if (virtualMouse == null) {
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
            Globals.virtualMouse = this;
        }
        else if (virtualMouse.added == false) {
            InputSystem.AddDevice(virtualMouse);
        }

        if (cursor != null) {

            hardwareMouse.WarpCursorPosition(start);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, start, canvas.worldCamera, out lastMousePos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, start, canvas.worldCamera, out currentMousePos);
            // Debug.Log(lastMousePos);
            InputState.Change(hardwareMouse, start);
            mousePositionUpdated = true;
            InputState.Change(virtualMouse.position, start);


        }

        InputUser.PerformPairingWithDevice(virtualMouse);

        InputSystem.onAfterUpdate += updateMotion;

    }


    /// <summary>
    /// removes the virtual mouse from the input system
    /// </summary>
    private void OnDisable() {
        // Debug.Log("disabled");
        controls.Dispose();
        controls = null;
        InputSystem.onAfterUpdate -= updateMotion;


        InputSystem.RemoveDevice(virtualMouse);
        //virtualMouse = null;

        Globals.virtualMouse = null;

        //   Debug.Log("disabled fertig");

    }


    /// <summary>
    /// load reabindings of the virtuals mouse
    /// </summary>
    public void loadNewRebinds() {

        controls.VirtualMouse.Disable();
        Rebinding_menu rebind = new Rebinding_menu();
        controls = rebind.loadRebinding(controls);
        controls.VirtualMouse.Enable();
    }


    /// <summary>
    /// updates the cursor position of the virtual mouse
    /// </summary>
    private void updateMotion() {





        if (lastMousePos != currentMousePos) {
            deltaMouse = currentMousePos - lastMousePos;

            lastMousePos = currentMousePos;
            deltaMouse = deltaMouse * canvas.scaleFactor;
        }
        else {
            deltaMouse = Vector2.zero;

        }


        //Debug.Log(scaleFactor);



        Vector2 delta = (direction * speed * Time.unscaledDeltaTime) * canvas.scaleFactor;
        Vector2 newPos = virtualMouse.position.ReadValue() + delta + deltaMouse;

        newPos.x = Mathf.Clamp(newPos.x, 0, Screen.width);
        newPos.y = Mathf.Clamp(newPos.y, 0, Screen.height);

        InputState.Change(virtualMouse.position, newPos);
        InputState.Change(virtualMouse.delta, (delta + deltaMouse));

        moveCursor(newPos);

        if ((delta + deltaMouse) == Vector2.zero && mousePositionUpdated == false && unlockMouse == false) {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, newPos, canvas.worldCamera, out lastMousePos);






            hardwareMouse.WarpCursorPosition(newPos);
            InputState.Change(hardwareMouse, newPos);



            mousePositionUpdated = true;
        }
        else if ((delta + deltaMouse) != Vector2.zero) {
            mousePositionUpdated = false;
        }

        if (buttonScroll != Vector2.zero) {
            InputState.Change(virtualMouse.scroll, buttonScroll);
        }


    }


    /// <summary>
    /// moves the virtual mouse gameobject
    /// </summary>
    /// <param name="position"> position to move the gameobject to</param>
    private void moveCursor(Vector2 position) {

        Vector2 newPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, position, canvas.worldCamera, out newPos);

        cursor.anchoredPosition = newPos;
    }

    /// <summary>
    /// input action left mouse button for the virtual mouse
    /// </summary>
    /// <param name="context"></param>
    public void OnLeftMouseButton(InputAction.CallbackContext context) {
        if (context.control.device != virtualMouse) {

            virtualMouse.CopyState<MouseState>(out MouseState mouseState);
            mouseState.WithButton(MouseButton.Left, context.ReadValueAsButton());
            InputState.Change(virtualMouse, mouseState);

        }
    }
    /// <summary>
    /// input action middle mouse button for the virtual mouse
    /// </summary>
    /// <param name="context"></param>
    public void OnMiddleMouseButton(InputAction.CallbackContext context) {
        if (context.control.device != virtualMouse) {

            virtualMouse.CopyState<MouseState>(out MouseState mouseState);
            mouseState.WithButton(MouseButton.Middle, context.ReadValueAsButton());
            InputState.Change(virtualMouse, mouseState);

        }
    }
    /// <summary>
    /// input action move cursor for the virtual mouse
    /// </summary>
    /// <param name="context"></param>
    public void OnMoveCursor(InputAction.CallbackContext context) {

        direction = context.ReadValue<Vector2>();
    }
    /// <summary>
    /// input action of the system mouse for the virtual mouse
    /// </summary>
    /// <param name="context"></param>
    public void OnPoint(InputAction.CallbackContext context) {
        if (controls == null) {
            return;
        }
        if (context.control.device != virtualMouse) {
            Vector2 pos = context.ReadValue<Vector2>();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, pos, canvas.worldCamera, out currentMousePos);


        }
    }
    /// <summary>
    /// input action right mouse button for the virtual mouse
    /// </summary>
    /// <param name="context"></param>
    public void OnRightMouseButton(InputAction.CallbackContext context) {
        if (context.control.device != virtualMouse) {

            virtualMouse.CopyState<MouseState>(out MouseState mouseState);
            mouseState.WithButton(MouseButton.Right, context.ReadValueAsButton());
            InputState.Change(virtualMouse, mouseState);

        }
    }
    /// <summary>
    /// input action scroll for the virtual mouse
    /// </summary>
    /// <param name="context"></param>
    public void OnScroll(InputAction.CallbackContext context) {
        if (context.control.device != virtualMouse) {
            if (context.control.device == hardwareMouse) {
                InputState.Change(virtualMouse.scroll, context.ReadValue<Vector2>());
            }
            else {
                buttonScroll = context.ReadValue<Vector2>() * 120;

            }
        }
    }

    /// <summary>
    /// input action to toogle the visibility of the system mouse
    /// </summary>
    /// <param name="context"></param>
    public void OnToogleMouseVisibility(InputAction.CallbackContext context) {
        if (context.started) {
            if (unlockMouse == true) {
                unlockMouse = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else {
                unlockMouse = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
