using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// script to make an object follow the cursor
/// </summary>
public class FollowMouse : MonoBehaviour
{

    private bool mouseIsPressed;

    /// <summary>
    /// equipment slots
    /// </summary>
    public List<RemoveSlotItem> slotMouseoverCheck;
    /// <summary>
    /// inventory list
    /// </summary>
    public Inventory_fill inv;

    public bool MouseIsPressed {
        get {
            return mouseIsPressed;
        }

        set {
            mouseIsPressed = value;
        }
    }

    /// <summary>
    /// sets basevalues
    /// </summary>
    void Start() {
        mouseIsPressed = false;
    }

    /// <summary>
    /// sets the transform of the object to cursor position
    /// checks if the left mouse button was released and was over a ship slot
    /// </summary>
    void Update() {

        Vector3 pos = Globals.virtualMouse.canvas.worldCamera.ScreenToWorldPoint(Globals.virtualMouse.VirtualMouseProperty.position.ReadValue());

        //Debug.Log(pos);
        transform.position = pos;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);


        if (Globals.virtualMouse.VirtualMouseProperty.leftButton.wasReleasedThisFrame && mouseIsPressed == true) {
            mouseIsPressed = false;
            foreach (RemoveSlotItem slot in slotMouseoverCheck) {
                if (slot.IsMouseOver == true) {
                    if (slot.isMainWeapon == true) {
                        inv.mainWeaponSlotClicked(slot.Image);
                    }
                    if (slot.isSecondaryWeapon == true) {
                        inv.secondaryWeaponSlotClicked(slot.Image);
                    }
                    if (slot.isSecondaryWeapon1 == true) {
                        inv.secondaryWeaponSlotTwoClicked(slot.Image);
                    }
                    if (slot.isShipPart == true) {
                        inv.shieldSlotClicked(slot.Image);
                    }

                }
            }
        }
    }
}
