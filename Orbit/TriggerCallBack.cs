using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// classe to create trigger areas
/// </summary>
public class TriggerCallBack : MonoBehaviour
{
    /// <summary>
    /// makes the trigger a spawn trigger
    /// </summary>
    public bool spawnTrigger;
    /// <summary>
    /// makes the trigger a spawner activation trigger
    /// </summary>
    public bool spawnerActivationTrigger;
    /// <summary>
    /// makes the trigger a money drop
    /// </summary>
    public bool moneyDrop;

    private bool canDestory;
    private int moneyValue;


    /// <summary>
    /// sets the money drop value
    /// </summary>
    public int MoneyValue {
        set {
            moneyValue = value;
        }
    }

    /// <summary>
    /// check triggers against player collision only 1 trigger option can be active per trigger gameobject
    /// </summary>
    /// <param name="collision">collision object</param>
    private void OnTriggerEnter2D(Collider2D collision) {
        if (Globals.player == collision.gameObject) {
            canDestory = true;
            if (spawnTrigger == true) {


                foreach (Enemy_Spawner e in Globals.spawnerListe) {

                    if (e.checkSpawnTrigger(gameObject) == false) {
                        canDestory = false;
                    }
                }
                if (canDestory == true) {
                    Destroy(gameObject);
                }

            }
            if (spawnerActivationTrigger == true) {
                foreach (Enemy_Spawner e in Globals.spawnerListe) {

                    if (e.checkSpawnerActivationTrigger(gameObject) == false) {
                        canDestory = false;
                    }

                    if (canDestory == true) {
                        Destroy(gameObject);
                    }
                }
            }
            if (moneyDrop == true) {
                Globals.money = Globals.money + moneyValue;
                //Debug.Log("Current Money: " + Globals.money.ToString());
                Destroy(gameObject);
            }

        }
    }
}
