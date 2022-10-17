using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// controls the enemy rotation
/// </summary>
public class Enemy_rotation : MonoBehaviour
{
    /// <summary>
    /// if true rotates the enemy towards the player
    /// </summary>
    public bool rotateTowardsPlayer;
    /// <summary>
    /// the rotation speed
    /// </summary>
    public float rotateSpeed;


    /// <summary>
    /// activates the rotation after hiting the first waypoint
    /// </summary>
    public bool activatedAfterMoveIn;

    private Enemy enemy;

    /// <summary>
    /// starts the rotating coroutine
    /// </summary>
    void Start() {
        enemy = gameObject.GetComponent<Enemy>();
        StartCoroutine(rotating());
    }


    /// <summary>
    /// rotates the enemy every frame to face player
    /// </summary>
    /// <returns></returns>
    private IEnumerator rotating() {


        while (true) {


            if (Globals.player != null && Globals.pause == false && rotateTowardsPlayer == true && (activatedAfterMoveIn == false || (activatedAfterMoveIn == true && enemy.enabled == true))) {


                Vector3 pos = Globals.player.transform.position;


                pos.z = 0;
                Vector2 dir = pos - transform.position;
                float angle = Vector2.SignedAngle(Vector2.right, dir);

                angle = angle + 90;

                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle), rotateSpeed * Time.deltaTime);


            }


            yield return null;
        }

    }
}
