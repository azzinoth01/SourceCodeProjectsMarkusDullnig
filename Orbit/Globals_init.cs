using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// class to initialise the globale variables
/// </summary>
public class Globals_init : MonoBehaviour
{
    /// <summary>
    /// money icon
    /// </summary>
    public Sprite moneyIcon;
    /// <summary>
    /// money drop prefab
    /// </summary>
    public GameObject moneyDropPrefrab;
    /// <summary>
    /// enemy hit sound
    /// </summary>
    public AudioSource tempEnemyHit;

    /// <summary>
    /// initialise the globale variables with standard values
    /// </summary>
    private void Awake() {
        Globals.pause = false;

        if (Globals.bulletPool == null) {
            Globals.bulletPool = new List<Skill>();
        }
        if (Globals.spawnerListe == null) {
            Globals.spawnerListe = new List<Enemy_Spawner>();
        }


        Globals.moneyIcon = moneyIcon;
        Globals.moneyDrop = moneyDropPrefrab;

        PlayerSave save = PlayerSave.loadSettings();
        if (save == null) {
            save = new PlayerSave();
        }
        Globals.money = save.Money;

        Globals.catalog = ItemCatalog.loadSettings();

        //  Debug.LogError("settings loaded");

        Globals.tempEnemyHit = tempEnemyHit;

        if (Globals.dontDestoryOnLoadObjectID == null) {
            Globals.dontDestoryOnLoadObjectID = new List<string>();
        }

        if (Globals.infityWaveSpawner == null) {
            Globals.infityWaveSpawner = new List<Enemy_Spawner>();
        }

    }

}
