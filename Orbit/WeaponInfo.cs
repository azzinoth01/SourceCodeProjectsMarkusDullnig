using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// class to store the weapon creation info
/// </summary>
[Serializable]
public class WeaponInfo : Inventar_Object
{
    /// <summary>
    /// main weapon
    /// </summary>
    [SerializeField] public bool mainWeapon;
    /// <summary>
    /// weapon skill
    /// </summary>
    [SerializeField] public string skill;
    /// <summary>
    /// weapon reload time
    /// </summary>
    [SerializeField] public float reloadTime;
    /// <summary>
    /// shoots to pre create
    /// </summary>
    [SerializeField] public int shootsToCreate;
    /// <summary>
    /// additional weapon dmg
    /// </summary>
    [SerializeField] public int additionalDmg;
    /// <summary>
    /// weapon dmg multiplier
    /// </summary>
    [SerializeField] public float dmgModifier;
}
