using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// did not work properly and not in use anymore
/// </summary>
public class GameCursor : MonoBehaviour
{

    public Texture2D cursor;
    public CursorMode mode;
    public Vector2 hotSpot;
    // Start is called before the first frame update
    void Start() {
        Cursor.SetCursor(cursor, hotSpot, mode);
    }


}
