using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// class handels the moving in to the playing field and out the playing field for enemies 
/// </summary>
public class Move_in_out_Scene : MonoBehaviour
{

    /// <summary>
    /// move in waypoint list
    /// </summary>
    public List<Vector2> moveInWaypoints;
    /// <summary>
    /// move out waypoint list
    /// </summary>
    public List<Vector2> moveOutWaypoints;

    /// <summary>
    /// waypoint prefab
    /// </summary>
    public GameObject waypointPrefab;



    /// <summary>
    /// the used force to move the enemy
    /// </summary>
    public float force;
    /// <summary>
    /// the maximum speed the enemy can have
    /// </summary>
    public float maxSpeed;
    /// <summary>
    /// physic object of enemy
    /// </summary>
    public Rigidbody2D body;
    private Rigidbody2D childBody;


    private List<GameObject> waypointInObjects;
    private List<GameObject> waypointOutObjects;
    private int waypointIndex;
    private bool moveIn;


    /// <summary>
    /// creates the waypoints to move enemies in and out of the scene
    /// </summary>
    void Start() {
        waypointInObjects = new List<GameObject>();
        waypointOutObjects = new List<GameObject>();

        for (int i = 0; i < moveInWaypoints.Count;) {
            waypointInObjects.Add(createNextWaypoint(moveInWaypoints[i]));
            i = i + 1;
        }

        for (int i = 0; i < moveOutWaypoints.Count;) {
            waypointOutObjects.Add(createNextWaypoint(moveOutWaypoints[i]));
            i = i + 1;
        }

        moveIn = true;
        waypointIndex = 0;

        childBody = GetComponentInChildren<Enemy>().body;

    }

    /// <summary>
    /// executes the in and out movement
    /// </summary>
    void Update() {

        if (Globals.pause == true) {
            return;
        }
        else {

            move();
        }
    }


    /// <summary>
    /// describes the move in and moveout of enemies dependet on the waypoints
    /// starts the destroying of enemies, who hit the last moveout point
    /// </summary>
    private void move() {

        if (moveIn == true) {
            if (waypointInObjects.Count > waypointIndex) {
                Vector2 direction = waypointInObjects[waypointIndex].transform.position - transform.position;
                body.AddForce(direction.normalized * force * Time.deltaTime, ForceMode2D.Impulse);

                Vector2 normalizedSpeed = body.velocity.normalized * maxSpeed;
                normalizedSpeed.x = Mathf.Abs(normalizedSpeed.x);
                normalizedSpeed.y = Mathf.Abs(normalizedSpeed.y);

                body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -normalizedSpeed.x, normalizedSpeed.x), Mathf.Clamp(body.velocity.y, -normalizedSpeed.y, normalizedSpeed.y));
                childBody.velocity = body.velocity;
                waypointInObjects[waypointIndex].SetActive(true);
            }
            else {
                Enemy e = GetComponentInChildren<Enemy>();
                e.enabled = true;

                e.body.velocity = body.velocity;
                body.bodyType = RigidbodyType2D.Static;
                enabled = false;
                return;
            }
        }
        else {
            if (waypointOutObjects.Count > waypointIndex) {
                Vector2 direction = waypointOutObjects[waypointIndex].transform.position - transform.position;
                body.AddForce(direction.normalized * force * Time.deltaTime, ForceMode2D.Impulse);

                Vector2 normalizedSpeed = body.velocity.normalized * maxSpeed;
                normalizedSpeed.x = Mathf.Abs(normalizedSpeed.x);
                normalizedSpeed.y = Mathf.Abs(normalizedSpeed.y);

                body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -normalizedSpeed.x, normalizedSpeed.x), Mathf.Clamp(body.velocity.y, -normalizedSpeed.y, normalizedSpeed.y));
                childBody.velocity = body.velocity;
                waypointOutObjects[waypointIndex].SetActive(true);
            }
            else {
                Destroy(gameObject);
                //StartCoroutine(startDestroy());
                //enabled = false;
            }
        }
    }

    /// <summary>
    /// starts the move out
    /// </summary>
    public void startMoveOut() {
        //Debug.Log("startet Move out");
        waypointIndex = 0;
        moveIn = false;
        enabled = true;


    }
    ///// <summary>
    ///// corutine um die enemys zu zerstören
    ///// destory in corutine um rechenleistung beim destroyen zu sparen
    ///// </summary>
    ///// <returns></returns>
    //private IEnumerator startDestroy() {

    //    foreach (GameObject g in waypointInObjects) {
    //        Destroy(g);
    //        yield return null;
    //    }
    //    foreach (GameObject g in waypointOutObjects) {
    //        Destroy(g);
    //        yield return null;
    //    }
    //    Destroy(gameObject);

    //}

    /// <summary>
    /// creates the waypoints which the enemy uses to move
    /// </summary>
    /// <param name="v2"> position of the waypoint</param>
    /// <returns> waypoint which was created</returns>
    private GameObject createNextWaypoint(Vector2 v2) {
        GameObject g = Instantiate(waypointPrefab, transform.parent);
        g.transform.localPosition = v2;
        // g.layer = gameObject.layer;
        //waypointInObjects.Add(g);
        g.SetActive(false);
        return g;
    }


    /// <summary>
    /// checks if a waypoint was hit
    /// </summary>
    /// <param name="collision"> collison object</param>
    private void OnTriggerEnter2D(Collider2D collision) {
        try {

            if (collision.gameObject == waypointInObjects[waypointIndex]) {

                waypointIndex = waypointIndex + 1;
                collision.gameObject.SetActive(false);
                return;
            }
        }
        catch {

        }
        try {

            if (collision.gameObject == waypointOutObjects[waypointIndex]) {

                waypointIndex = waypointIndex + 1;
                collision.gameObject.SetActive(false);
                return;
            }
        }
        catch {

        }

        try {


            if (collision.tag == Tag_enum.enemy_border.ToString()) {
                if (moveIn == true) {
                    GetComponentInChildren<Enemy_skills>().enabled = true;
                    //Debug.Log(collision);
                }
                //else {
                //    GetComponentInChildren<Enemy_skills>().enabled = false;
                //}

            }

        }
        catch {

        }
    }
    /// <summary>
    /// destorys the waypoints and the enemy
    /// </summary>
    private void OnDestroy() {
        foreach (GameObject g in waypointInObjects) {
            Destroy(g);
        }
        foreach (GameObject g in waypointOutObjects) {
            Destroy(g);
        }
    }
}
