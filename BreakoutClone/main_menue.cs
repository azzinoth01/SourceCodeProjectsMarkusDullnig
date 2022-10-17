
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleFileBrowser;

public class main_menue : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        globals.AlertScreen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void levelEditor() {

        SceneManager.LoadScene("map_builder", LoadSceneMode.Single);
    }

    public void exitGame() {
        Application.Quit();
        
    }

    public void arcadeStart() {
        globals.arcadeLevel = 1;
        globals.arcadeStart = 1;
        SceneManager.LoadScene("Scene1", LoadSceneMode.Single);
        
    }
    public void levelSelect() {
        
        SceneManager.LoadScene("select_level", LoadSceneMode.Single);
    }

    public void customLevel() {
        //string path = EditorUtility.OpenFilePanel("Custom Level", "", "json");
        
        FileBrowser.SetFilters(true, new FileBrowser.Filter("json", ".json"));
        FileBrowser.ShowLoadDialog((path) => { FileSelectSuccess(path); }, null,FileBrowser.PickMode.Files, false, null, null, "Select Folder", "Select" );
        //globals.customLevel = 1;
        //globals.customLevelPath = path;
        //SceneManager.LoadScene("Scene1", LoadSceneMode.Single);
        
    }

    private void FileSelectSuccess(string[] path) {
        globals.customLevel = 1;
        globals.customLevelPath = path[0];
        SceneManager.LoadScene("Scene1", LoadSceneMode.Single);
    }

    public void backToMainMenue() {
        SceneManager.LoadScene("main_menue", LoadSceneMode.Single);
    }
}
