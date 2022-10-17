using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class add_tiles_button : MonoBehaviour {
    public Button prefab;
    public List<Button> buttonlist;
    public SpriteRenderer paintPrefab;
    private SpriteRenderer paint;
    public RectTransform panel;
    //public RectTransform content;

    // Start is called before the first frame update
    void Start() {

        //TextAsset textFile = AssetDatabase.LoadAssetAtPath("Assets/Text/SpritesNames.json", typeof(TextAsset)) as TextAsset;
        TextAsset textFile = Resources.Load("Text/SpritesNames", typeof(TextAsset)) as TextAsset;
        string json = textFile.text;

        FileNameInfo info = JsonUtility.FromJson<FileNameInfo>(json);

        foreach (string s in info.fileNames) {
            Button b = Instantiate(prefab, this.transform);
            //  Sprite sp = AssetDatabase.LoadAssetAtPath(s, typeof(Sprite)) as Sprite;
            Sprite sp = Resources.Load(s, typeof(Sprite)) as Sprite;
            b.GetComponent<Image>().sprite = sp;

            buttonlist.Add(b);

            b.onClick.AddListener(delegate {
                CreatePaintObject(s, 0);
            });
        }

        // lösch button hinzufügen, damit man tiles auch löschen kann
        // seperat hinzugefügt damit es der letzte Button in der liste ist

        Button b2 = Instantiate(prefab, this.transform);
        // Sprite sp2 = AssetDatabase.LoadAssetAtPath("Assets/Tiles/eraser_icon.png", typeof(Sprite)) as Sprite;
        Sprite sp2 = Resources.Load("Tiles/eraser_icon", typeof(Sprite)) as Sprite;
        b2.GetComponent<Image>().sprite = sp2;

        buttonlist.Add(b2);

        //   b2.onClick.AddListener(delegate { CreatePaintObject("Assets/Tiles/eraser_icon.png",1); });
        b2.onClick.AddListener(delegate {
            CreatePaintObject("Tiles/eraser_icon", 1);
        });
    }

    // Update is called once per frame
    void Update() {

    }

    public void CreatePaintObject(string input, int delete) {

        paint = Instantiate(paintPrefab);

        paint.GetComponent<paint>().panel = panel;
        paint.GetComponent<paint>().setSprite(input);

        paint.GetComponent<paint>().delete = delete;


        panel.GetComponent<paint_grid>().setPaintObj(paint.gameObject);



        // Debug.Log(input);
    }

}
