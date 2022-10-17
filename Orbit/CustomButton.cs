using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// creates custom button shapes
/// </summary>
public class CustomButton : MonoBehaviour
{
    /// <summary>
    /// button image which controls the shape
    /// </summary>
    public Image buttonimage;


    /// <summary>
    /// crates custom button shapes by using the alpha of the image as hit check
    /// </summary>
    void Start() {
        buttonimage.alphaHitTestMinimumThreshold = 0.9f;

    }


}
