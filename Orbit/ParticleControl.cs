using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// controliert die Particle system effecte und Particle Sounds
/// </summary>
public class ParticleControl : MonoBehaviour
{
    /// <summary>
    /// the used particle system
    /// </summary>
    public ParticleSystem particle;
    /// <summary>
    /// destorys after playing
    /// </summary>
    public bool destroyAfterPlay;

    /// <summary>
    /// particle audio
    /// </summary>
    public List<AudioSource> particleAudio;
    /// <summary>
    /// animation
    /// </summary>
    public Animator anim;

    /// <summary>
    /// starts the particle system 
    /// starts the audio
    /// </summary>
    private void OnEnable() {
        if (particle != null) {
            particle.Play();
        }

        if (particleAudio != null) {

            foreach (AudioSource audio in particleAudio) {
                audio.Play();
            }

        }
        if (anim != null) {
            anim.enabled = true;
        }


        //Debug.Log("start playing");
    }




    /// <summary>
    /// deactivates or destorys the gameobject after playing
    /// </summary>
    private void Update() {
        if (Globals.pause == true) {
            return;
        }
        else {
            if (isPlayingCheck() == false) {
                if (destroyAfterPlay == true) {
                    //Debug.Log("test");
                    Destroy(gameObject);
                }
                else {
                    //Debug.Log("test");
                    gameObject.SetActive(false);
                }

            }

        }

    }

    /// <summary>
    /// checks if audio or particle system is still playing
    /// </summary>
    /// <returns> returns true if it is still playing</returns>
    private bool isPlayingCheck() {

        bool check = true;
        if (particleAudio != null) {
            foreach (AudioSource audio in particleAudio) {
                if (audio.isPlaying == false) {
                    check = false;
                }
                else {
                    return true;
                }
            }

        }
        if (particle != null) {
            if (particle.isPlaying == false) {
                check = false;
            }
            else {
                return true;
            }
        }
        if (anim != null) {
            if (anim.enabled == false) {
                check = false;
            }
            else {
                return true;
            }
        }


        return check;
    }
}
