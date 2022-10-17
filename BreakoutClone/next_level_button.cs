using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class next_level_button : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(globals.arcadeStart != 1 || globals.lives <= 0 || globals.arcadeLevel == 12) {
            gameObject.SetActive(false);
        }
    }

    public void nextLevelButton() {
        globals.arcadeLevel = globals.arcadeLevel + 1;
        globals.AlertScreen = false;
        globals.start = 0;
        Debug.Log("next level button");
        SceneManager.LoadScene("Scene1", LoadSceneMode.Single);
    }
}
