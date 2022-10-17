using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// checks if certain levels are already unlocked
/// </summary>
public class CheckLevelUnlock : MonoBehaviour
{

    /// <summary>
    /// checks if the survival level is already unlocked
    /// </summary>
    private void Awake() {
        PlayerSave s = PlayerSave.loadSettings();

        if (s.Level1Played == true) {
            gameObject.SetActive(true);
        }
        else {
            gameObject.SetActive(false);
        }
    }


}
