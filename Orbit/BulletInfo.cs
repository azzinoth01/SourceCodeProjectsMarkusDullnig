using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// container class to describe the bullets
/// </summary>
[Serializable]
public class BulletInfo
{
    [SerializeField] private float startRotation;
    [SerializeField] private float bulletBaseDmg;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject startEffect;
    [SerializeField] private GameObject sfxSound;

    private GameObject instantStartEffect;

    private float addBaseDmg;
    private float dmgModifier;

    private Bullet bulletScript;


    /// <summary>
    /// standardconstuctor sets the basevalues
    /// </summary>
    public BulletInfo() {
        //  Debug.Log("construktor called1");
        //bulletScript = bullet.GetComponent<Bullet>();
        addBaseDmg = 0;
        dmgModifier = 1;


    }
    /// <summary>
    /// constructor to set values
    /// </summary>
    /// <param name="startRotation"> describes the flying direction of the baseobject</param>
    /// <param name="bulletBaseDmg"> describes the base dmg of the bullet</param>
    /// <param name="bullet"> bullet prefab</param>
    /// <param name="startEffect"> particel system effect prefab</param>
    /// <param name="sfxSound"> sound effect prefab</param>
    public BulletInfo(float startRotation, float bulletBaseDmg, GameObject bullet, GameObject startEffect, GameObject sfxSound) {
        Debug.Log("construktor called");
        this.startRotation = startRotation;
        this.bulletBaseDmg = bulletBaseDmg;
        this.bullet = bullet;
        this.startEffect = startEffect;
        this.sfxSound = sfxSound;
        //bulletScript = bullet.GetComponent<Bullet>();
        addBaseDmg = 0;
        dmgModifier = 1;

    }

    /// <summary>
    /// start rotation
    /// </summary>
    public float StartRotation {
        get {
            return startRotation;
        }

        set {
            startRotation = value;
        }
    }

    /// <summary>
    /// resets the dmg modifiers
    /// </summary>
    public void resetModifiers() {
        addBaseDmg = 0;
        dmgModifier = 1;
        setBulletDmg();
    }


    /// <summary>
    /// bullet prefab
    /// </summary>
    public GameObject Bullet {
        get {
            return bullet;
        }

        set {
            bullet = value;
        }
    }

    /// <summary>
    /// set bullet script and calc bullet dmg
    /// </summary>
    public Bullet BulletScript {

        set {
            bulletScript = value;
            setBulletDmg();

        }
    }

    /// <summary>
    /// set base dmg and calc bullet dmg
    /// </summary>
    public float AddBaseDmg {


        set {
            addBaseDmg = value;
            setBulletDmg();
        }
    }

    /// <summary>
    /// set dmg modifier and calc bullet dmg
    /// </summary>
    public float DmgModifier {


        set {
            dmgModifier = value;
            setBulletDmg();
        }
    }
    /// <summary>
    /// set and returns the sart effect of the bullet
    /// </summary>
    public GameObject StartEffect {
        get {
            return startEffect;
        }

        set {
            startEffect = value;
        }
    }

    /// <summary>
    /// sets and returns the instanz of the start effect
    /// </summary>
    public GameObject InstantStartEffect {
        get {
            return instantStartEffect;
        }

        set {
            instantStartEffect = value;
        }
    }

    /// <summary>
    /// sets and returns the sfx Sound effect
    /// </summary>
    public GameObject SfxSound {
        get {
            return sfxSound;
        }

        set {
            sfxSound = value;
        }
    }

    /// <summary>
    /// returns the bullet base dmg
    /// </summary>
    public float BulletBaseDmg {
        get {
            return bulletBaseDmg;
        }


    }

    /// <summary>
    /// sets the bullet dmg on the bullet
    /// </summary>
    public void setBulletDmg() {
        if (bulletScript != null) {
            bulletScript.BulletDmg = (bulletBaseDmg + addBaseDmg) * dmgModifier;
        }

    }

    /// <summary>
    /// sets den layer der Bullet
    /// sets the layer of the bullet (Layer_enum.player_bullets or Layer_enum.enemy_bullets)
    /// </summary>
    /// <param name="layer"> layer in integer (use Layer_enum)</param>
    public void setLayer(int layer) {
        bulletScript.gameObject.layer = layer;


    }


    /// <summary>
    /// enables the bullet particle effect
    /// </summary>
    public void enableEffects() {
        if (instantStartEffect != null) {
            instantStartEffect.SetActive(true);
        }

    }

    //private void OnEnable() {
    //    bullet.GetComponent<Bullet>().BulletDmg = bulletBaseDmg; // + modifier 
    //}
}
