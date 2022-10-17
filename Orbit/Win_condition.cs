using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/// <summary>
/// win condition handler class
/// </summary>
public class Win_condition : MonoBehaviour
{
    /// <summary>
    /// enemies to kill
    /// </summary>
    public int enemysToKill;
    /// <summary>
    /// collider to chck portal hit
    /// </summary>
    public BoxCollider2D boxcollider;

    /// <summary>
    /// sprite render of portal
    /// </summary>
    public SpriteRenderer sp;
    /// <summary>
    /// portal light
    /// </summary>
    public Light2D lighting;
    /// <summary>
    /// rotates the portal
    /// </summary>
    public bool rotate;
    /// <summary>
    /// rotation speed of portal
    /// </summary>
    public float rotateSpeed;

    /// <summary>
    /// portal spawn audio
    /// </summary>
    public AudioSource audios;
    /// <summary>
    /// spawn delay
    /// </summary>
    public float spawnDelay;

    private bool alreadyActive;
    private bool alreadyTriggerd;

    /// <summary>
    /// sets the wincondition in the globalen variables
    /// </summary>
    void Start() {
        Globals.currentWinCondition = this;
        alreadyActive = false;
        alreadyTriggerd = false;
    }

    /// <summary>
    /// activates the level end portal
    /// </summary>
    public void activateLevelFinishPortal() {
        alreadyActive = true;
        boxcollider.enabled = true;
        sp.enabled = true;
        lighting.enabled = true;

        if (rotate == true) {
            StartCoroutine(startRotating());
        }

        if (audios != null) {
            audios.Play();
        }

    }

    /// <summary>
    /// delays the spawing of the portal
    /// </summary>
    /// <returns></returns>
    private IEnumerator delayPortalSpawning() {



        yield return new WaitForSeconds(spawnDelay);
        activateLevelFinishPortal();
    }



    /// <summary>
    /// reduces the enemy kill counter by 1
    /// </summary>
    public void enemyKilled() {
        enemysToKill = enemysToKill - 1;

        if (Globals.waveControler != null) {
            Globals.menuHandler.onChangedScore();
        }

        if (enemysToKill <= 0 && Globals.waveControler != null) {
            Globals.waveControler.waveFinished();
        }
        else {
            if (enemysToKill <= 0 && alreadyActive == false) {
                //Globals.menuHandler.levelFinishedUI.SetActive(true);

                //moved to spawn delay
                //activateLevelFinishPortal();

                StartCoroutine(delayPortalSpawning());
            }
        }

    }

    /// <summary>
    /// starts the rotating of the portal
    /// </summary>
    /// <returns></returns>
    private IEnumerator startRotating() {

        while (true) {
            if (Globals.pause == true) {
                yield return null;
            }
            else {
                Vector3 nextAngle = transform.rotation.eulerAngles + new Vector3(0, 0, 30);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(nextAngle), rotateSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }

    /// <summary>
    /// activates the level finish UI
    /// </summary>
    /// <param name="collision"> collision object</param>
    private void OnTriggerEnter2D(Collider2D collision) {

        if (Globals.player == collision.gameObject) {
            if (alreadyTriggerd == false) {
                Globals.menuHandler.Playtime = Time.time - Globals.player.GetComponent<Player>().Timestamp;
                Globals.menuHandler.setLevelFinish();
                alreadyTriggerd = true;
            }

            // Destroy(gameObject);
            // Globals.player.GetComponent<Player>().disableControlls = true;
        }
    }

}
