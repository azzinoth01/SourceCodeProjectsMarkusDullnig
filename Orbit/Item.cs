using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// baseclass for all items
/// </summary>
[Serializable]
public class Item
{
    [SerializeField] private string iD;
    [SerializeField] private string ItemName;
    [SerializeField] private int value;
    [SerializeField] private string icon;
    [SerializeField] private string patternIcon;
    [SerializeField] private string sprite;


    /// <summary>
    /// unique ID
    /// </summary>
    public string ID {
        get {
            return iD;
        }

        set {
            iD = value;
        }
    }
    /// <summary>
    /// name
    /// </summary>
    public string Name {
        get {
            return ItemName;
        }

        set {
            ItemName = value;
        }
    }
    /// <summary>
    /// buy price
    /// </summary>
    public int Value {
        get {
            return value;
        }

        set {
            this.value = value;
        }
    }
    /// <summary>
    /// icon for inventory
    /// </summary>
    public string Icon {
        get {
            return icon;
        }

        set {
            icon = value;
        }
    }

    /// <summary>
    /// game sprice for the item
    /// </summary>
    public string Sprite {
        get {
            return sprite;
        }

        set {
            sprite = value;
        }
    }
    /// <summary>
    /// pattern icon for the item
    /// </summary>
    public string PatternIcon {
        get {
            return patternIcon;
        }

        set {
            patternIcon = value;
        }
    }
}
