using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// class to trigger tooltips
/// </summary>
public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    /// <summary>
    /// the text for the content text field
    /// </summary>
    public string content;
    /// <summary>
    /// the text for the header text field
    /// </summary>
    public string header;


    /// <summary>
    /// shows the tooltips if the cursor is over a button
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData) {
        TooltipSystem.Show(content, header);
    }

    /// <summary>
    /// hides the tooltip if the cursor leaves the button
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData) {
        TooltipSystem.Hide();

    }
}
