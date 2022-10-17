using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exit_button : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(globals.arcadeStart != 1 || globals.lives <= 0 || globals.arcadeLevel == 12) {
            transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
        }
        else {
            transform.localPosition = new Vector3(-100, transform.localPosition.y, transform.localPosition.z);
            
        }
    }

    public void exitButton() {
        globals.customLevel = 0;
        globals.customLevelPath = "";
        globals.arcadeStart = 0;
        globals.arcadeLevel = 0;
        globals.start = 0;
        globals.AlertScreen = false;

        SceneManager.LoadScene("main_menue", LoadSceneMode.Single);
        
    }
}
