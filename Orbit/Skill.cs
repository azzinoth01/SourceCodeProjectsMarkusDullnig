using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class that controls the bullets
/// </summary>
public class Skill : MonoBehaviour
{

    /// <summary>
    /// list of bullet infos
    /// </summary>
    public List<BulletInfo> bulletInfoList;
    /// <summary>
    /// max duration of skill
    /// </summary>
    public int maxDuration;
    private float time;
    private bool isRunning;
    private Coroutine timer;

    private float timestamp;


    /// <summary>
    /// timestamp of skill
    /// </summary>
    public float Timestamp {
        get {
            return timestamp;
        }

        set {
            timestamp = value;
        }
    }

    /// <summary>
    /// creates bulletobjects out of the bulletInfoList
    /// </summary>
    private void Awake() {
        int i = 0;

        foreach (BulletInfo b in bulletInfoList) {
            GameObject g = new GameObject("bulletInfo" + i.ToString());

            g.transform.SetParent(transform, false);
            g.transform.localEulerAngles = new Vector3(0, 0, b.StartRotation);

            GameObject bullet = Instantiate(b.Bullet, g.transform);
            if (b.StartEffect != null) {
                GameObject effect = Instantiate(b.StartEffect, g.transform);


                b.InstantStartEffect = effect;
            }
            if (b.SfxSound != null) {
                GameObject sound = Instantiate(b.SfxSound, g.transform.parent.transform);
            }


            b.BulletScript = bullet.GetComponent<Bullet>();
            i = i + 1;


        }
    }

    /// <summary>
    /// starts the max duration timer
    /// </summary>
    private void Update() {

        if (Globals.pause == true) {
            return;
        }
        else {
            if (isRunning == false) {
                isRunning = true;
                timer = StartCoroutine(startDurationTimer(maxDuration));
            }
        }

    }

    /// <summary>
    /// max duration timer
    /// </summary>
    /// <param name="wait"> duration time in seconds</param>
    /// <returns></returns>
    private IEnumerator startDurationTimer(float wait) {
        yield return new WaitForSeconds(wait);
        isRunning = false;
        gameObject.SetActive(false);

    }

    /// <summary>
    /// changes the layer of the bullets if the layer of the skill changes
    /// </summary>
    public void layerChange() {
        foreach (BulletInfo b in bulletInfoList) {
            b.setLayer(gameObject.layer);
        }
    }


    /// <summary>
    /// puts the skill back into the bulletpool
    /// resets all modifier on the bullet
    /// stops the duration timer
    /// sets a timestamp for the bulletpool cleaner
    /// </summary>
    private void OnDisable() {

        Globals.bulletPool.Add(this);
        foreach (BulletInfo b in bulletInfoList) {
            b.resetModifiers();
        }
        if (timer != null) {
            StopCoroutine(timer);
        }
        timestamp = Time.time;
    }

    /// <summary>
    /// remove itself from bulletpool if it is destroyed
    /// </summary>
    private void OnDestroy() {
        Globals.bulletPool.Remove(this);
    }


    /// <summary>
    /// set all child objecte active
    /// set the layer new
    /// </summary>
    private void OnEnable() {
        foreach (Transform t in transform) {
            t.gameObject.SetActive(true);
        }
        layerChange();
        time = 0;
        isRunning = false;
        effectEnable();
    }

    /// <summary>
    /// enables the particle effect object
    /// </summary>
    private void effectEnable() {
        foreach (BulletInfo b in bulletInfoList) {
            b.enableEffects();
        }
    }


    /// <summary>
    /// set modifier on the bullet
    /// </summary>
    /// <param name="additionalDmg"> increases the dmg directly on the bullet by this value</param>
    /// <param name="dmgModifier"> after adding the additional dmg to the bullet dmg the resulting value is multiplied by this value</param>
    public void setDmgModifiers(float additionalDmg, float dmgModifier) {
        foreach (BulletInfo b in bulletInfoList) {
            b.AddBaseDmg = additionalDmg;
            b.DmgModifier = dmgModifier;
        }
    }


    /// <summary>
    /// checks if all child objects are inactive, so the skill can be deactivated
    /// is called if a bullet is deactivated
    /// </summary>
    public void checkDisabled() {
        int i = transform.childCount;
        int counter = 0;

        foreach (Transform t in transform) {
            if (t.gameObject.activeSelf == false) {
                counter = counter + 1;
            }
        }

        if (counter == i) {
            //gameObject.name = "test deactivation";
            gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// sets the sfx sound on the bullet
    /// </summary>
    /// <param name="sound"> the sfx prefab that is to be played</param>
    public void setSfxSoundOnBullets(GameObject sound) {

        foreach (BulletInfo b in bulletInfoList) {
            b.SfxSound = sound;
        }

    }

}
