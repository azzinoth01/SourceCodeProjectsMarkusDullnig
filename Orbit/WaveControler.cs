using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// class to control the wave system on the endless level
/// </summary>
public class WaveControler : MonoBehaviour
{
    /// <summary>
    /// min of enemies to spawn
    /// </summary>
    public int minEnemys;
    /// <summary>
    /// max of enemies to spawn
    /// </summary>
    public int maxEnemys;

    private int currentWave;

    private float curentHealthUpgrade;
    private float currentEnemyDmgUpgrade;


    private Player player;

    /// <summary>
    /// returns and sets the current wave
    /// </summary>
    public int CurrentWave {
        get {
            return currentWave;
        }

        set {
            currentWave = value;
        }
    }



    /// <summary>
    /// sets base values for the wavecontroler
    /// </summary>
    void Start() {
        player = Globals.player.GetComponent<Player>();
        Globals.waveControler = this;
        curentHealthUpgrade = 0;
        currentEnemyDmgUpgrade = 0;
        StartCoroutine(delayStart(1f));
    }


    /// <summary>
    /// delays the start of the next wave
    /// </summary>
    /// <param name="delay"> delay in seconds</param>
    /// <returns></returns>
    private IEnumerator delayStart(float delay) {


        yield return new WaitForSeconds(delay);

        startNextWave();
    }

    /// <summary>
    /// starts the next wave
    /// upgrades the enemies and player depending on the wave level
    /// </summary>
    public void startNextWave() {
        currentWave = currentWave + 1;






        if (currentWave % 5 == 0) {
            player.additionalDmg = player.additionalDmg + 0.25f;

        }
        if (currentWave % 10 == 0) {
            player.dmgModifier = player.dmgModifier + 1.1f;

            curentHealthUpgrade = curentHealthUpgrade * 2.8f;
            currentEnemyDmgUpgrade = currentEnemyDmgUpgrade + 1f;
        }

        Globals.currentWinCondition.enemysToKill = 0;


        player.CurrentHealth = player.maxBaseHealth;


        int enemysToSpawn = Random.Range(minEnemys, maxEnemys);

        List<Enemy_Spawner> spawner = new List<Enemy_Spawner>(Globals.infityWaveSpawner);

        while (enemysToSpawn != 0) {

            if (spawner.Count == 0) {
                break;
            }

            int i = Random.Range(0, spawner.Count);

            if (spawner[i].gameObject.activeSelf == true) {
                spawner.RemoveAt(i);
                continue;
            }

            spawner[i].modifyAddHealth = curentHealthUpgrade;
            spawner[i].modifyAddDmg = currentEnemyDmgUpgrade;

            spawner[i].gameObject.SetActive(true);

            spawner.RemoveAt(i);
            enemysToSpawn = enemysToSpawn - 1;

            Globals.currentWinCondition.enemysToKill = Globals.currentWinCondition.enemysToKill + 1;
            Globals.menuHandler.onChangedScore();
        }
    }

    /// <summary>
    /// wave was finished
    /// upgrades wave values
    /// starts timer for next wave
    /// </summary>
    public void waveFinished() {

        curentHealthUpgrade = curentHealthUpgrade + 0.3f;
        minEnemys = minEnemys + 1;
        maxEnemys = maxEnemys + 1;

        StartCoroutine(delayStart(1f));
    }


    /// <summary>
    /// removes the controler from global variables
    /// </summary>
    private void OnDestroy() {
        Globals.waveControler = null;
    }
}
