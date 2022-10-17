using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class create_background_music : MonoBehaviour
{
    public AudioSource backgroundMusic;

    // Start is called before the first frame update
    void Start()
    {
        if(globals.backgroundMusic != true) {
            Instantiate(backgroundMusic);
            globals.backgroundMusic = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
