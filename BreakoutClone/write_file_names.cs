using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class write_file_names : MonoBehaviour {
    static int SortByString(string n1, string n2) {
        return n1.CompareTo(n2);
    }

    // Start is called before the first frame update
    void Start() {
        Debug.Log("start");
        string resourcsPath = Application.dataPath + "/Resources/pallets/Tiles";

        DirectoryInfo dir = new DirectoryInfo(resourcsPath);
        FileInfo[] info = dir.GetFiles("*.asset");

        FileNameInfo fileInfo = new FileNameInfo(info.Select(x => System.IO.Path.ChangeExtension("pallets/Tiles/" + x.Name, null)).ToList());

        // wollte diese nicht verschieben auf grund von möglichen komplicationen
        // deswegen einfach nur der liste nehmen
        fileInfo.fileNames.Remove("Tiles/eraser_icon");
        fileInfo.fileNames.Remove("Tiles/58-Breakout-Tiles");
        fileInfo.fileNames.Remove("Tiles/padle");
        fileInfo.fileNames.Remove("Tiles/background");
        fileInfo.fileNames.Remove("Tiles/fire");
        fileInfo.fileNames.Remove("Tiles/fire2");
        fileInfo.fileNames.Remove("Tiles/fire3");
        fileInfo.fileNames.Remove("Tiles/fire4");


        fileInfo.fileNames.Sort(SortByString);

        string fileInfoJson = JsonUtility.ToJson(fileInfo);
        File.WriteAllText(Application.dataPath + "/Resources/Text/PalletNames.json", fileInfoJson);


        resourcsPath = Application.dataPath + "/Resources/Tiles";

        dir = new DirectoryInfo(resourcsPath);
        info = dir.GetFiles("*.png");


        fileInfo = new FileNameInfo(info.Select(x => System.IO.Path.ChangeExtension("Tiles/" + x.Name, null)).ToList());

        // wollte diese nicht verschieben auf grund von möglichen komplicationen
        // deswegen einfach nur der liste nehmen
        fileInfo.fileNames.Remove("Tiles/eraser_icon");
        fileInfo.fileNames.Remove("Tiles/58-Breakout-Tiles");
        fileInfo.fileNames.Remove("Tiles/padle");
        fileInfo.fileNames.Remove("Tiles/background");
        fileInfo.fileNames.Remove("Tiles/fire");
        fileInfo.fileNames.Remove("Tiles/fire2");
        fileInfo.fileNames.Remove("Tiles/fire3");
        fileInfo.fileNames.Remove("Tiles/fire4");

        fileInfo.fileNames.Sort(SortByString);

        fileInfoJson = JsonUtility.ToJson(fileInfo);
        File.WriteAllText(Application.dataPath + "/Resources/Text/SpritesNames.json", fileInfoJson);



        //AssetDatabase.Refresh();

        Debug.Log("finished");
    }

    // Update is called once per frame
    void Update() {

    }


}
//class PreBuildFileNamesSaver : IPreprocessBuildWithReport {
//    public int callbackOrder { get { return 0; } }
//    public void OnPreprocessBuild(UnityEditor.Build.Reporting.BuildReport report) {
//        //The Resources folder path
//        string resourcsPath = Application.dataPath + "/Resources";

//        //Get file names except the ".meta" extension
//        string[] fileNames = Directory.GetFiles(resourcsPath)
//            .Where(x => Path.GetExtension(x) != ".meta").ToArray();

//        //Convert the Names to Json to make it easier to access when reading it
//        FileNameInfo fileInfo = new FileNameInfo(fileNames);
//        string fileInfoJson = JsonUtility.ToJson(fileInfo);

//        //Save the json to the Resources folder as "FileNames.txt"
//        File.WriteAllText(Application.dataPath + "/Resources/FileNames.txt", fileInfoJson);

//        AssetDatabase.Refresh();
//    }
//}