using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.Controls;


/// <summary>
/// class to control the story intro
/// </summary>
public class StoryIntro : MonoBehaviour
{

    private bool skip;
    /// <summary>
    /// main menu panel
    /// </summary>
    public GameObject mainMenu;
    /// <summary>
    /// story slide list
    /// </summary>
    public List<GameObject> slides;

    private List<GameObject> privateSlides;

    //private IDisposable buttonEvent;

    private List<ButtonControl> pressedButtons;

    /// <summary>
    /// returns and sets the skip value
    /// </summary>
    public bool Skip {
        get {
            return skip;
        }

        set {
            skip = value;
        }
    }



    /// <summary>
    /// checks if the intro was already displayed this session
    /// and skips it if it was already displayed
    /// activates the anybutton input check for moving between intro slides
    /// </summary>
    private void OnEnable() {
        skip = false;
        InputSystem.onEvent += anyButtonWasPressed;
        InputSystem.onEvent += anyButtonWasReleased;
        privateSlides = new List<GameObject>(slides);

        if (Globals.skipStartCutscene == true) {
            skip = true;
            if (skip == true) {
                foreach (GameObject g in slides) {
                    g.SetActive(false);

                }
                mainMenu.SetActive(true);
                return;
            }
        }
        pressedButtons = new List<ButtonControl>();

        // buttonEvent = InputSystem.onAnyButtonPress.Call(nextSlide);


        //InputSystem.onEvent += (eventPtr, device) => {
        //    if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>()) {
        //        return;
        //    }
        //    ReadOnlyArray<InputControl> controls = device.allControls;
        //    float presspoint = InputSystem.settings.defaultButtonPressPoint;

        //    //Debug.Log("test1");

        //    foreach (InputControl inp in controls) {
        //        ButtonControl but = inp as ButtonControl;

        //        if (but == null || but.synthetic || but.noisy) {
        //            continue;
        //        }
        //        but.ReadValueFromEvent(eventPtr, out float value);

        //        //Debug.Log(pressedButtons.Contains(but));

        //        if (value >= presspoint && pressedButtons.Contains(but) == false) {
        //            Debug.Log("button pressed");
        //            pressedButtons.Add(but);
        //            break;
        //        }


        //    }

        //    return;
        //};



        //InputSystem.onEvent += (eventPtr, device) => {
        //    if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>()) {
        //        return;
        //    }
        //    ReadOnlyArray<InputControl> controls = device.allControls;
        //    float presspoint = InputSystem.settings.defaultButtonPressPoint;

        //    // Debug.Log("test2");
        //    foreach (InputControl inp in controls) {
        //        ButtonControl but = inp as ButtonControl;

        //        if (but == null || but.synthetic || but.noisy) {
        //            continue;
        //        }
        //        but.ReadValueFromEvent(eventPtr, out float value);
        //        //  Debug.Log(value);



        //        if (pressedButtons.Contains(but) == true && value <= presspoint) {
        //            pressedButtons.Remove(but);
        //            Debug.Log("button released");
        //            break;
        //        }


        //    }

        //    return;
        //};


        //    InputSystem.onEvent += (eventPtr, device) => {
        //    if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
        //        return;
        //    var controls = device.allControls;
        //    var buttonPressPoint = InputSytem.settings.defaultButtonPressPoint;
        //    for (var i = 0; i < controls.Count; ++i) {
        //        var control = controls[i] as ButtonControl;
        //        if (control == null || control.synthetic || control.noisy)
        //            continue;
        //        if (control.ReadValueFromEvent(eventPtr, out var value) && value >= buttonPressPoint) {
        //            m_ButtonPressed = true;
        //            break;
        //        }
        //    }
        //};


    }

    /// <summary>
    /// if skip is activated the intro is skiped
    /// </summary>
    void Update() {



        if (skip == true) {
            foreach (GameObject g in slides) {
                g.SetActive(false);

            }
            Globals.skipStartCutscene = true;
            mainMenu.SetActive(true);
            enabled = false;
        }


    }

    /// <summary>
    /// checks if any button was pressed
    /// </summary>
    /// <param name="eventPtr"> input event</param>
    /// <param name="device"> input device</param>
    private void anyButtonWasPressed(InputEventPtr eventPtr, InputDevice device) {
        if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>()) {
            return;
        }
        ReadOnlyArray<InputControl> controls = device.allControls;
        float presspoint = InputSystem.settings.defaultButtonPressPoint;

        //Debug.Log("test1");

        foreach (InputControl inp in controls) {
            ButtonControl but = inp as ButtonControl;

            if (but == null || but.synthetic || but.noisy) {
                continue;
            }
            but.ReadValueFromEvent(eventPtr, out float value);

            //Debug.Log(pressedButtons.Contains(but));

            if (value >= presspoint && pressedButtons.Contains(but) == false) {
                //  Debug.Log("button pressed");
                nextSlide();
                pressedButtons.Add(but);
                break;
            }


        }

        return;
    }

    /// <summary>
    /// checks if any button was released
    /// </summary>
    /// <param name="eventPtr"> input event</param>
    /// <param name="device"> input device</param>
    private void anyButtonWasReleased(InputEventPtr eventPtr, InputDevice device) {
        if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>()) {
            return;
        }
        ReadOnlyArray<InputControl> controls = device.allControls;
        float presspoint = InputSystem.settings.defaultButtonPressPoint;

        // Debug.Log("test2");
        foreach (InputControl inp in controls) {
            ButtonControl but = inp as ButtonControl;

            if (but == null || but.synthetic || but.noisy) {
                continue;
            }
            but.ReadValueFromEvent(eventPtr, out float value);
            //  Debug.Log(value);



            if (pressedButtons.Contains(but) == true && value <= presspoint) {
                pressedButtons.Remove(but);
                //  Debug.Log("button released");
                break;
            }


        }

        return;
    }


    /// <summary>
    /// moves the story to the next slide
    /// if there are no more slides the the story intro is deactivated and the main menu is activated
    /// </summary>
    private void nextSlide() {




        if (privateSlides.Count != 0) {
            privateSlides[0].SetActive(false);
            privateSlides.RemoveAt(0);
            if (privateSlides.Count != 0) {
                privateSlides[0].SetActive(true);

            }
            else {
                mainMenu.SetActive(true);
                enabled = false;
            }
        }

    }

    /// <summary>
    /// saves the intro skip
    /// deactivates the any button input listener
    /// </summary>
    private void OnDisable() {
        Globals.skipStartCutscene = true;

        //if (buttonEvent != null) {
        //    buttonEvent.Dispose();
        //    buttonEvent = null;
        //}


        InputSystem.onEvent -= anyButtonWasPressed;
        InputSystem.onEvent -= anyButtonWasReleased;
    }

}
