using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// limits framerate to 60 fps
/// </summary>
public class LimitFrameRate : MonoBehaviour
{
    /// <summary>
    /// limits framerate to 60 fps
    /// </summary>
    private void Awake() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

}
