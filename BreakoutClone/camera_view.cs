using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_view : MonoBehaviour
{

    //public float yTiles;
    public float xTiles;
    public float yTiles;
    private float pixelsize = 2.56f;
    public float y;
    public float x;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        y = (yTiles * pixelsize) / 2;
        float width = (xTiles * pixelsize) / 2;
       

        x = width / Camera.main.aspect;


        if (y > x) {
            Camera.main.orthographicSize = x;
        }
        else {
            Camera.main.orthographicSize = y;
        }



    }
}
