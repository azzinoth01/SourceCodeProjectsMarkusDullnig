using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// class to load the money icon from the global variables
/// </summary>
public class LoadMoneyIcon : MonoBehaviour
{
    private Image icon;
    private SpriteRenderer render;

    /// <summary>
    /// loads the money icon and sets it to the image or the sprite of the object
    /// </summary>
    void Start() {
        try {
            icon = gameObject.GetComponent<Image>();
        }
        catch {

        }
        try {
            render = gameObject.GetComponent<SpriteRenderer>();
        }
        catch {

        }
        if (icon != null) {
            icon.sprite = Globals.moneyIcon;
        }
        if (render != null) {
            render.sprite = Globals.moneyIcon;
        }
    }

}
