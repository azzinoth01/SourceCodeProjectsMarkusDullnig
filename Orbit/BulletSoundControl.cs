using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// controls the audio of the bullets
/// </summary>
public class BulletSoundControl : MonoBehaviour
{
    /// <summary>
    /// audio to be played
    /// </summary>
    public AudioSource audios;


    /// <summary>
    /// plays the audio once enabled
    /// </summary>
    private void OnEnable() {
        audios.Play();
    }

    /// <summary>
    /// checks if the audio is still playing and deactivaes itself once it finished playing
    /// activates the disabled check of parent Skill script if it deactivates
    /// </summary>
    void Update() {
        if (Globals.pause == true) {
            return;
        }
        else {
            if (audios.isPlaying == false) {
                gameObject.SetActive(false);

                try {
                    Skill s = GetComponentInParent<Skill>();
                    s.checkDisabled();
                }
                catch {

                }
            }
        }

    }
}
