using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// class to control tooltips
/// </summary>
public class TooltipSystem : MonoBehaviour
{
    /// <summary>
    /// tooltip object to be used
    /// </summary>
    public ToolTip tooltip;


    /// <summary>
    /// loads the tooltip settings
    /// </summary>
    public void Start() {
        Globals.tooltip = tooltip;
        SaveSettings s = SaveSettings.loadSettings();

        tooltip.tooltipToogled = s.IsToogleOn;

        Debug.Log("tooltip is " + tooltip.tooltipToogled.ToString());
    }

    /// <summary>
    /// shows the tooltip if it is enabled
    /// </summary>
    /// <param name="content"> text to display in the content text field</param>
    /// <param name="header"> text to display in the header text field</param>
    public static void Show(string content, string header = "") {
        if (Globals.tooltip != null) {
            if (Globals.tooltip.tooltipToogled == true) {
                Globals.tooltip.SetText(content, header);
                Globals.tooltip.gameObject.SetActive(true);
            }
            else {
                Hide();
            }
        }


    }

    /// <summary>
    /// hides the tooltip
    /// </summary>
    public static void Hide() {
        if (Globals.tooltip != null) {
            Globals.tooltip.gameObject.SetActive(false);
        }
    }

}
