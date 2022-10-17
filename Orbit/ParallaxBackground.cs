using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// not in use anymore
/// </summary>
public class ParallaxBackground : MonoBehaviour
{
    public GameObject Camera;
    public float speed;
    private float StartPosX;
    private float StartPosY;

    // Start is called before the first frame update
    void Start() {
        StartPosX = transform.position.x;
        StartPosY = transform.position.y;
    }

    // Update is called once per frame
    void Update() {
        float distancex = (Camera.transform.position.x * speed);
        float distancey = (Camera.transform.position.y * speed);

        transform.position = new Vector3(StartPosX + distancex, StartPosY + distancey, transform.position.z);
    }
}
