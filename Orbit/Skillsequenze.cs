using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// container classe to describe skills
/// </summary>
[Serializable]
public class Skillsequenze
{
    [SerializeField] private float delay;
    [SerializeField] private GameObject skill;
    [SerializeField] private bool shootInRotatedDirection;

    /// <summary>
    /// construktor classe
    /// </summary>
    /// <param name="delay"> delay between skills</param>
    /// <param name="skill"> the skill to be used</param>
    /// <param name="shootInRotatedDirection"> if the skill is to be shoot in the rotated direction</param>
    public Skillsequenze(float delay, GameObject skill, bool shootInRotatedDirection) {
        this.delay = delay;
        this.skill = skill;
        this.shootInRotatedDirection = shootInRotatedDirection;
    }

    /// <summary>
    /// retruns skill gameobject
    /// </summary>
    public GameObject Skill {
        get {
            return skill;
        }


    }
    /// <summary>
    /// returns skill delay
    /// </summary>
    public float Delay {
        get {
            return delay;
        }


    }
    /// <summary>
    /// returns and sets if the skill shoots in player direction
    /// </summary>
    public bool ShootInRotatedDirection {
        get {
            return shootInRotatedDirection;
        }

        set {
            shootInRotatedDirection = value;
        }
    }
}
