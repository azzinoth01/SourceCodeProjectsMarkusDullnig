using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// class to control the speech bubbles
/// </summary>
public class SpeechBubbles : MonoBehaviour
{
    /// <summary>
    /// speech bubble object
    /// </summary>
    public GameObject UiObject;
    /// <summary>
    /// trigger object
    /// </summary>
    public GameObject Trigger;
    /// <summary>
    /// audio source
    /// </summary>
    public AudioSource audios;
    private bool isEnterd;

    /// <summary>
    /// deactivates every speech bubble so they start in an inactive state
    /// </summary>
    void Start() {
        UiObject.SetActive(false);
        isEnterd = false;
    }

    /// <summary>
    /// checks if the player moved onto the trigger area
    /// and activates the speech bubble
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            UiObject.SetActive(true);
            audios.Play();
            isEnterd = true;
        }
    }

    /// <summary>
    /// destroys the trigger area and deactivates the speech bubble
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit2D(Collider2D other) {
        if (isEnterd == true) {
            if (other.tag == "Player") {
                UiObject.SetActive(false);
                Destroy(Trigger);
            }
        }

    }
}
