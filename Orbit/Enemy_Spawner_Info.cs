using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// container classe for spawner variablen
/// </summary>
[Serializable]
public class Enemy_Spawner_Info
{
    /// <summary>
    /// trigger area to activate the spawn
    /// </summary>
    public GameObject triggerArea;
    /// <summary>
    /// uses trigger area to activate the spawn
    /// </summary>
    public bool useTriggerArea;
    /// <summary>
    /// the delay between spawn
    /// </summary>
    public float delay;
    /// <summary>
    /// how many enemies to spawn 
    /// </summary>
    public int enemysToSpawn;
    /// <summary>
    /// enemy to spawn prefab
    /// </summary>
    public GameObject enemyPrefab;
    private bool spawnStartet;
    private int currentEnemysSpawned;
    private bool spawnConditonFulfilled;


    /// <summary>
    /// base constructor sets standard values
    /// </summary>
    public Enemy_Spawner_Info() {
        spawnStartet = false;
        currentEnemysSpawned = 0;
        spawnConditonFulfilled = false;
    }

    /// <summary>
    /// returns if the spawner is startet and sets the spawncondition to if the spawner is started
    /// </summary>
    public bool SpawnStartet {
        get {
            return spawnStartet;
        }

        set {
            if (value == true) {
                spawnConditonFulfilled = true;
            }
            spawnStartet = value;
        }
    }

    /// <summary>
    /// returns and sets current enemies spawned
    /// </summary>
    public int CurrentEnemysSpawned {
        get {
            return currentEnemysSpawned;
        }

        set {
            currentEnemysSpawned = value;
        }
    }

    /// <summary>
    /// retruns and sets the spawncondition
    /// </summary>
    public bool SpawnConditonFulfilled {
        get {
            return spawnConditonFulfilled;
        }
        set {
            spawnConditonFulfilled = value;
        }
    }
}
