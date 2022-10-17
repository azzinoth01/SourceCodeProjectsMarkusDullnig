using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// sub class of item to manage iventory objects
/// </summary>
[Serializable]
public class Inventar_Object : Item
{

    [SerializeField] private int amount;

    /// <summary>
    /// returns and sets the amount
    /// </summary>
    public int Amount {
        get {
            return amount;
        }

        set {
            amount = value;
        }
    }
}
