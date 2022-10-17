using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class level_select_buttons : MonoBehaviour {
    public GameObject prefabPanel;
    public Button prefabButton;

    // Start is called before the first frame update
    void Start() {


        GameObject g = null;
        for (int i = 1; i < 13;) {
            if (((i % 3)) == 1) {

                g = Instantiate(prefabPanel, transform);
            }
            Button b = Instantiate(prefabButton, g.transform);
            // Sprite sp = AssetDatabase.LoadAssetAtPath("Assets/Level_images/level_" + i.ToString() + ".png", typeof(Sprite)) as Sprite;
            Sprite sp = Resources.Load("Level_images/level_" + i.ToString(), typeof(Sprite)) as Sprite;

            b.GetComponent<Image>().sprite = sp;
            int tempInt = i;
            b.GetComponentInChildren<Text>().text = "Level " + i.ToString();
            b.onClick.AddListener(delegate {
                selectLevel(tempInt);
            });
            i = i + 1;
        }

    }

    private void selectLevel(int level) {
        //Debug.Log(level);
        globals.arcadeLevel = level;
        globals.arcadeStart = 0;
        SceneManager.LoadScene("Scene1", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update() {

    }
}
