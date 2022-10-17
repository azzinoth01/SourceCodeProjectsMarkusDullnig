using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class map_status : MonoBehaviour
{
    public List<string> powerupList;
    public Text liveLabel;
    public GameObject Alert;
    public AudioSource gameOver_audio;
    private int gameOverPlayed;

    // Start is called before the first frame update
    void Start()
    {
        globals.lives = 3;
        globals.map_tiles = 0;
        globals.start = 0;
        globals.mainPanel = gameObject.GetComponent<RectTransform>();
        globals.powerupList = new List<string>();
        globals.afterGameScreenText = "";
        gameOverPlayed = 0;
        int size = powerupList.Count;
        int i = 0;
        while (i < size) {
            globals.powerupList.Insert(i, powerupList[i]);
            i = i + 1;
        }
        //foreach (string s in powerupList) {
        //    globals.powerupList.Add(s);
        //}
    }

    // Update is called once per frame
    void Update()
    {

        liveLabel.text = "Leben: " + globals.lives.ToString();

        if(globals.AlertScreen == false) {
            Alert.SetActive(false);
        }

        if (globals.lives <= 0 && globals.start == 1) {
            //Debug.Log("game over");
            globals.afterGameScreenText = "Game Over";
           // if (globals.customLevel == 1) {
                //globals.customLevel = 0;
                //globals.customLevelPath = "";
            globals.AlertScreen = true;
            Alert.SetActive(true);

            if(gameOverPlayed == 0) {
                gameOverPlayed = 1;
                gameOver_audio.Play();
            }
          

               // SceneManager.LoadScene("main_menue", LoadSceneMode.Single);
                
          //  }
        }
        if(globals.map_tiles <= 0 && globals.start == 1) {
          //  Debug.Log("win");
            globals.afterGameScreenText = "Level Abgeschlossen";
            //if (globals) {.customLevel == 1
                //globals.customLevel = 0;
                //globals.customLevelPath = "";
                globals.AlertScreen = true;
                Alert.SetActive(true);
               // SceneManager.LoadScene("main_menue", LoadSceneMode.Single);
            //}
            
        }
    }


   
}
