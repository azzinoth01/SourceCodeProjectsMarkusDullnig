using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// class to describe ship parts sub class of inventory objects
/// </summary>
[Serializable]
public class Parts : Inventar_Object
{
    [SerializeField] private int healthBoost;
    [SerializeField] private float shieldRefreshValueBoost;

    /// <summary>
    /// boosts the health of the ship
    /// </summary>
    public int HealthBoost {
        get {
            return healthBoost;
        }

        set {
            healthBoost = value;
        }
    }
    /// <summary>
    /// boosts the shield refresh value
    /// </summary>
    public float ShieldRefreshValueBoost {
        get {
            return shieldRefreshValueBoost;
        }

        set {
            shieldRefreshValueBoost = value;
        }
    }
}
