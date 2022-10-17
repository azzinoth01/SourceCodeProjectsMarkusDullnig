using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// class to create spawner out of a referenz spawner
/// </summary>
public class SpawnerCreation : MonoBehaviour
{
    /// <summary>
    /// referenz spawner object
    /// </summary>
    public GameObject refSpawner;

    /// <summary>
    /// move in and move out offset
    /// </summary>
    public Vector2 moveInOutOffset;
    /// <summary>
    /// use waypoint X offset
    /// </summary>
    public bool useWaypointOffsetX;
    /// <summary>
    /// use waypoint Y offset
    /// </summary>
    public bool useWaypointOffsetY;
    /// <summary>
    /// flip waypoint offset
    /// </summary>
    public bool flipOffset;


    /// <summary>
    /// creates a spawner as a child object of this gameobject and modifies the waypoints of the referenz spawner
    /// </summary>
    void Start() {

        GameObject g = Instantiate(refSpawner, transform);

        Enemy_Spawner spawner = g.GetComponent<Enemy_Spawner>();

        Globals.infityWaveSpawner.Add(g.GetComponent<Enemy_Spawner>());

        Vector2 moveOffset = Vector2.zero;

        if (useWaypointOffsetX == true) {
            moveOffset = new Vector2(transform.position.x - refSpawner.transform.position.x, 0);

        }
        if (useWaypointOffsetY == true) {
            moveOffset = new Vector2(moveOffset.x, transform.position.y - refSpawner.transform.position.y);

        }

        if (flipOffset == true) {
            moveOffset = moveOffset * -1;
        }

        int index = 0;

        for (; index < spawner.modifyWaypoints.Count;) {

            spawner.modifyWaypoints[index] = spawner.modifyWaypoints[index] - moveOffset;

            index = index + 1;
        }

        index = 0;
        for (; index < spawner.modifyMoveIn.Count;) {

            spawner.modifyMoveIn[index] = spawner.modifyMoveIn[index] - moveInOutOffset;

            index = index + 1;
        }
        index = 0;
        for (; index < spawner.modifyMoveOut.Count;) {

            spawner.modifyMoveOut[index] = spawner.modifyMoveOut[index] + moveInOutOffset;

            index = index + 1;
        }

        //  g.SetActive(true);

    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
