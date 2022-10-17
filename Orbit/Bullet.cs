using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// class that describes how bullets fly
/// </summary>
public class Bullet : MonoBehaviour
{
    /// <summary>
    /// waypoint list which the bullet follows
    /// </summary>
    public List<Vector2> waypoints;
    /// <summary>
    /// bullet speed
    /// </summary>
    public float speed;
    private Waypoint_Designer designer;

    /// <summary>
    /// physic object ob bullet
    /// </summary>
    public Rigidbody2D body;

    /// <summary>
    /// waypoint prefab
    /// </summary>
    public GameObject waypointPrefab;

    private List<GameObject> waypointObject;
    private int waypointIndex;
    private bool waypointDirectionSet;
    private bool loop;
    private float restartAfter;
    private float restartTime;



    private float bulletDmg;


    /// <summary>
    /// sets the bullet dmg
    /// </summary>
    public float BulletDmg {

        set {
            bulletDmg = value;
        }
    }



    /// <summary>
    /// sets basevalues and creates the waypoints to which the bullets fly
    /// </summary>
    void Start() {
        //Debug.Log("start");
        //restartTime = 0;
        //time = 0;
        //waypointIndex = 0;
        waypointObject = new List<GameObject>();
        try {
            designer = GetComponentInParent<Waypoint_Designer>();

            waypoints = new List<Vector2>(designer.waypoints);
            speed = designer.speed;
            loop = designer.loop;
            restartAfter = designer.restartAfter;
            waypointPrefab = designer.waypointPrefab;
        }
        catch {
            //       Debug.Log("no designer mode");
        }
        if (waypoints[0] != Vector2.zero) {
            waypoints.Insert(0, Vector2.zero);
        }



        for (int i = 0; i < waypoints.Count;) {
            createNextWaypoint(waypoints[i]);
            i = i + 1;
        }

    }

    /// <summary>
    /// controls the movement of the bullets
    /// </summary>
    void Update() {
        if (Globals.pause == true) {
            return;
        }
        else {
            movement();
        }
    }

    /// <summary>
    /// describes how the bullets move with the waypoints
    /// </summary>
    private void movement() {
        if (waypoints.Count > waypointIndex && waypointDirectionSet == false) {
            //createNextWaypoint(waypoints[waypointIndex]);
            activeNextWaypoint();
            waypointDirectionSet = true;
            Vector2 direction = waypointObject[waypointIndex].transform.position - transform.position;
            body.velocity = direction.normalized * speed;
        }
        else if (designer != null && waypoints.Count == waypointIndex) {

            if (loop == true) {
                //createNextWaypoint(new Vector2(0, 0));
                waypointIndex = 0;
                activeNextWaypoint();
                waypointDirectionSet = true;
                Vector2 direction = waypointObject[waypointIndex].transform.position - transform.position;
                body.velocity = direction.normalized * speed;

                return;
            }
            restartTime = restartTime + Time.deltaTime;
            if (restartAfter <= restartTime) {
                restartTime = 0;
                transform.position = waypointObject[0].transform.position;
                waypointIndex = 0;
                waypointDirectionSet = false;

            }
        }




    }

    /// <summary>
    /// activates the next waypoint the bullet has to fly to
    /// </summary>
    private void activeNextWaypoint() {
        waypointObject[waypointIndex].SetActive(true);
    }


    /// <summary>
    /// creates the waypoints using the given vector
    /// </summary>
    /// <param name="v2"> position of the waypoint</param>
    private void createNextWaypoint(Vector2 v2) {
        GameObject g = Instantiate(waypointPrefab, transform.parent);
        g.transform.localPosition = v2;
        g.SetActive(false);
        // g.layer = gameObject.layer;
        waypointObject.Add(g);

    }

    /// <summary>
    /// bullet has hit the bullet border and will be deactivated
    /// </summary>
    /// <param name="collision"> collision object</param>
    private void OnTriggerExit2D(Collider2D collision) {

        if (collision.tag == Tag_enum.bullet_border.ToString()) {
            setInactive();
        }

    }

    /// <summary>
    /// checks if the bullet has hit a waypint
    /// checks if a player of enemy was hit
    /// if player of enemy was hit health will be reduced based on bullet dmg
    /// </summary>
    /// <param name="collision"> collision object</param>
    private void OnTriggerEnter2D(Collider2D collision) {


        try {

            if (collision.gameObject == waypointObject[waypointIndex]) {
                waypointIndex = waypointIndex + 1;
                waypointDirectionSet = false;
                collision.gameObject.SetActive(false);
                return;
                //Destroy(collision.gameObject);
                //waypointObject.RemoveAt(0);
            }
        }
        catch {
            //Debug.Log("no Waypoint collision");
            //Debug.Log(collision);
        }

        try {
            // Debug.Log("bullet hit enemy");
            //Debug.Log(collision);
            Enemy g = collision.GetComponent<Enemy>();
            if (g != null) {
                //  Debug.Log("bullet hit enemy2");
                //Debug.Log(collision);
                g.takeDmg(bulletDmg);
                //Destroy(gameObject.transform.parent.gameObject);
                setInactive();
                return;
            }


        }
        catch {
            //Debug.Log("no enemy hit");
            //Debug.Log(collision);
        }

        try {

            //Debug.Log(collision);
            Player g = collision.GetComponent<Player>();
            if (g != null) {
                g.takeDmg(bulletDmg);
                //Destroy(gameObject.transform.parent.gameObject);
                setInactive();
                return;
            }


        }
        catch {
            //Debug.Log("no player hit");
            //Debug.Log(collision);
        }


    }

    //private void OnDisable() {
    //    //Debug.Log("bullet got disabled");

    //    if (gameObject.layer == 8) { // layer 8 enemy_bullets

    //        Globals.bulletPool.Add(gameObject.transform.parent.gameObject);

    //    }
    //    else {
    //        Globals.bulletPool.Add(gameObject.transform.parent.gameObject);
    //    }

    //}


    /// <summary>
    /// resets the basic values
    /// </summary>
    private void OnEnable() {
        //Debug.Log("bullet got enabled");
        restartTime = 0;

        waypointIndex = 0;
        if (waypointObject != null) {
            transform.position = waypointObject[0].transform.position;
        }

        waypointDirectionSet = false;

    }

    /// <summary>
    /// sets the bullet inactive and activates the skill inactive check
    /// </summary>
    private void setInactive() {

        gameObject.transform.parent.gameObject.SetActive(false);

        //Debug.Log("set inactive");
        gameObject.transform.parent.transform.parent.GetComponent<Skill>().checkDisabled();
    }




}
