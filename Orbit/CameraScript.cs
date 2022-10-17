using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// camera movement script
/// </summary>
public class CameraScript : MonoBehaviour
{

    private Rigidbody2D body;
    private Player player;
    private float playerMaxSpeed;
    private Vector2 offset;



    /// <summary>
    /// screen shake max duration
    /// </summary>
    public float screenShakeMaxDuration;
    /// <summary>
    /// screen shake min duration
    /// </summary>
    public float screenShakeMinDuration;

    /// <summary>
    /// screen shake max magnitude
    /// </summary>
    public float screenShakeMaxMagnitude;

    private Vector2 screenShakeOffset;
    private bool screenShakeRunning;

    /// <summary>
    /// sets the current main camera in the Global variables
    /// </summary>
    private void Awake() {
        Globals.currentCamera = gameObject.GetComponent<Camera>();
    }
    /// <summary>
    /// sets the base variables
    /// </summary>
    private void OnEnable() {
        body = GetComponent<Rigidbody2D>();
        player = Globals.player.GetComponent<Player>();
        playerMaxSpeed = player.maxSpeed;
        offset = Vector2.zero;

        screenShakeRunning = false;
    }

    /// <summary>
    /// moves the camera in movementdirection with a little offset, so one sees more in movementdirection
    /// </summary>
    private void Update() {
        if (Globals.pause == true) {
            return;
        }
        else {
            if (player == null) {
                body.velocity = Vector2.zero;
                return;
            }
            if (screenShakeRunning == true) {
                return;
            }

            //Debug.Log((Globals.player.transform.position - transform.position));
            offset = new Vector2((6 * (player.Impulse.x / player.force) * Time.deltaTime) + offset.x, (5 * (player.Impulse.y / player.force) * Time.deltaTime) + offset.y);

            //Vector2 Clampoffset = new Vector2(6 * (player.Impulse.x / player.force), 5 * (player.Impulse.y / player.force));
            Vector2 Clampoffset = new Vector2(6, 5);
            offset = new Vector2(Mathf.Clamp(offset.x, -Clampoffset.x, Clampoffset.x), Mathf.Clamp(offset.y, -Clampoffset.y, Clampoffset.y));




            Vector2 direction = ((Vector2)Globals.player.transform.position - ((Vector2)transform.position - offset)) * playerMaxSpeed;

            body.velocity = player.body.velocity + direction;
            //direction = direction - offset;

            //body.AddForce(direction);

            //body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -playerMaxSpeed, playerMaxSpeed), Mathf.Clamp(body.velocity.y, -playerMaxSpeed, playerMaxSpeed));

        }
    }


    /// <summary>
    /// starts the screen shake
    /// </summary>
    /// <returns></returns>
    public IEnumerator startScreenShake() {
        float wait = Random.Range(screenShakeMinDuration, screenShakeMaxDuration);

        screenShakeRunning = true;


        StartCoroutine(screenShake());


        yield return new WaitForSeconds(wait);


        screenShakeRunning = false;

    }


    /// <summary>
    /// executes the screen shake
    /// </summary>
    /// <returns></returns>
    private IEnumerator screenShake() {


        while (screenShakeRunning == true) {
            //Debug.Log("shaking");

            Vector2 shake = Random.insideUnitCircle * screenShakeMaxMagnitude * Time.deltaTime;

            screenShakeOffset = screenShakeOffset + shake;

            transform.position = transform.position + (Vector3)shake;


            yield return null;
        }

        //transform.position = transform.position - (Vector3)screenShakeOffset;
    }

}
