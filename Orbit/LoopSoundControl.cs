using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// controls audiosources which are split into and intro, transition and a loop
/// </summary>
public class LoopSoundControl : MonoBehaviour
{
    /// <summary>
    /// the start audio
    /// </summary>
    public AudioSource startAudio;
    /// <summary>
    /// the transition audio
    /// </summary>
    public AudioSource transitionAudio;
    /// <summary>
    /// the loop audio
    /// </summary>
    public AudioSource loopAudio;

    private bool transitionPlayed;
    private bool isPlaying;


    /// <summary>
    /// returns the playing state
    /// </summary>
    public bool IsPlaying {
        get {
            return isPlaying;
        }


    }


    /// <summary>
    /// starts the audio
    /// </summary>
    void Start() {

        startPlaying();



    }

    /// <summary>
    /// handels the transition from start to transition
    /// and transition to loop
    /// </summary>
    void Update() {

        if (transitionAudio != null && IsPlaying == true) {
            if (transitionPlayed == false && startAudio.isPlaying == false && transitionAudio.isPlaying == false && loopAudio.isPlaying == false) {
                transitionAudio.Play();
                transitionPlayed = true;
                Debug.Log("transition started");
            }
            else if (loopAudio != null && IsPlaying == true) {
                if (transitionPlayed == true && startAudio.isPlaying == false && transitionAudio.isPlaying == false && loopAudio.isPlaying == false) {
                    loopAudio.Play();
                    Debug.Log("loop started");
                }
            }
        }





    }

    /// <summary>
    /// starts the audio and sets base values
    /// </summary>
    public void startPlaying() {
        startAudio.Play();
        transitionPlayed = false;
        isPlaying = true;
    }

    /// <summary>
    /// stops the audio 
    /// </summary>
    public void stopPlaying() {
        isPlaying = false;
        startAudio.Stop();
        transitionAudio.Stop();
        loopAudio.Stop();
    }
}
