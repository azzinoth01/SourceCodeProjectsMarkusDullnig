using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// classe to help gamedesinger make waypoints
/// </summary>
public class Waypoint_Designer : MonoBehaviour
{
    /// <summary>
    /// waypoint list
    /// </summary>
    public List<Vector2> waypoints;
    /// <summary>
    /// speed of object
    /// </summary>
    public float speed;
    /// <summary>
    /// force of object
    /// </summary>
    public float force;
    /// <summary>
    /// restart time of loop
    /// </summary>
    public float restartAfter;
    /// <summary>
    /// loop waypoints
    /// </summary>
    public bool loop;
    /// <summary>
    /// delay between waypoints
    /// </summary>
    public float enemyDelayToNextWaypoint;
    /// <summary>
    /// line render
    /// </summary>
    public LineRenderer line;
    /// <summary>
    /// waypoint prefab
    /// </summary>
    public GameObject waypointPrefab;
    // public GameObject movementCheckPrefab;
    private GameObject waypointPlacer;
    private bool placeWaypoint;
    private int lineIndex;

    /// <summary>
    /// list of waypoint objects
    /// </summary>
    public List<GameObject> waypointList;



    /// <summary>
    /// set vectors in the line Render visualize the connections of the waypoints
    /// </summary>
    void Start() {
        line.positionCount = waypoints.Count + 1;

        lineIndex = 0;
        // waypointList = new List<GameObject>();
        foreach (Vector2 v2 in waypoints) {
            line.SetPosition(lineIndex + 1, v2);
            lineIndex = lineIndex + 1;

            //  GameObject g = Instantiate(waypointPrefab, v2, Quaternion.identity, transform);

            //waypointList.Add(g);
        }

    }

    /// <summary>
    /// places the waypoints on the screen if the left mouse button is pressed
    /// if the right mousbutton was clicked then the seting of waypoints is deactivated
    /// </summary>
    void Update() {
        if (Globals.pause == true) {
            return;
        }
        else {
            if (placeWaypoint == true) {
                waypointPlacer.transform.position = (Vector2)Globals.currentCamera.ScreenToWorldPoint((Vector2)Mouse.current.position.ReadValue());
                if (Mouse.current.leftButton.wasPressedThisFrame) {
                    GameObject g = Instantiate(waypointPlacer, transform);
                    waypoints.Add(g.transform.position);
                    line.positionCount = line.positionCount + 1;
                    line.SetPosition(lineIndex + 1, g.transform.position);
                    lineIndex = lineIndex + 1;

                }
                if (Mouse.current.rightButton.wasPressedThisFrame) {

                    placeWaypoint = false;
                    Destroy(waypointPlacer);
                }
            }
        }

    }


    /// <summary>
    /// activates the seting of waypoints
    /// </summary>
    [ContextMenu("Set Waypoints on Screen")]
    public void paintWayoints() {


        waypointPlacer = Instantiate(waypointPrefab, (Vector3)Mouse.current.position.ReadValue(), Quaternion.identity);
        placeWaypoint = true;

    }
}
