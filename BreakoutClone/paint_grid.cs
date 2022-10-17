using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;

public class paint_grid : MonoBehaviour {

   
    private float tempPanelY;
    private float tempPanelX;
    private float screenX;
    private float screenY;
    public float sizeX;
    public float sizeY;
    private Color color;


    public bool grid;
    public int xTiles;
    public int yTiles;
    public GameObject paintObj;
    public int currentlayer;
    public SpriteRenderer tilePrefab;

    public float spriteSizeX;
    public float spriteSizeY;

    public Map editmap;
    public List<Edit_tile> tileList;
    public Text layerText;
    public Button upButton;
    public Button downButton;
    public Toggle toggleGrid;
    public InputField xField;
    public InputField yField;
    public RectTransform info_panel;
    private bool info_panel_state;

    // Start is called before the first frame update
    void Start() {

        

        grid = true;
        color = new Color(255, 255, 255);
        editmap = new Map(xTiles, yTiles);
        currentlayer = 1;
        tileList = new List<Edit_tile>();

        info_panel_state = true;

        infoButton();
    }

    // grid mit gui zeichnen weil linerender2d manche linen einfach nicht zeichnet
    void OnGUI() {


        

        RectTransform panel = gameObject.GetComponent<RectTransform>();

        float x = panel.rect.width;
        float y = panel.rect.height;



        // nicht zeichenn während screen sich resized
        if (screenX == Screen.width && screenY == Screen.height && xTiles > 0 && yTiles > 0 ) {
            Vector3[] v3 = drawGrid(panel);
            if(paintObj != null) {
                List<Cellmap> cells = cellPositions(v3[0],false);
            }
            
            int lineCount = (yTiles + 1 + yTiles + xTiles + 1 + xTiles) + 2;

            Camera cam = Camera.main;


            if (grid == true) {
                //Vector3 offset = localToWorldOffset(panel);
                for (int i = 0; i < (lineCount - 1);) {
                    //Vector3 wav3 = new Vector3(v3[i].x + offset.x, (v3[i].y + ((-1) * offset.y)) * (-1), v3[i].z);
                    //Vector3 wbv3 = new Vector3(v3[i + 1].x + offset.x, (v3[i + 1].y + ((-1) * offset.y)) * (-1), v3[i + 1].z);

                    Vector3 wav3 = panel.TransformPoint(v3[i]);
                    Vector3 wbv3 = panel.TransformPoint(v3[i + 1]);




                    Vector3 a3 = cam.WorldToScreenPoint(wav3);
                    Vector3 b3 = cam.WorldToScreenPoint(wbv3);
                    draw_line.DrawLine(new Vector2(a3.x, a3.y), new Vector2(b3.x, b3.y), color, 1);

                    i = i + 2;
                }
            }
        }
      
        screenX = Screen.width;
        screenY = Screen.height;
    }


    // Update is called once per frame
    void Update()
    {   

    }

 
    private Vector3[] drawGrid(RectTransform panel) {
        
        // -4 damits die linen schöner aufs pannel gezeichnet werden
        tempPanelX = panel.rect.width -4;
        tempPanelY = panel.rect.height -4 ;

        float xoffset = 2;
        float yoffset = 2;

        float sizeRealationX = spriteSizeY / spriteSizeX;
        float sizeRealationY = spriteSizeX / spriteSizeY;

        //if(sizeRealationX > sizeRealationY) {
        //    sizeRealationY = 1;
        //}
        //else {
        //    sizeRealationX = 1;
        //}
        //Debug.Log("XRealation");
        //Debug.Log(sizeRealationX);
        //Debug.Log("YRealation");
        //Debug.Log(sizeRealationY);

        float xTempSize = tempPanelX / xTiles;
        float yTempSize = tempPanelY / yTiles;


        //

        float xyTempSize = xTempSize * sizeRealationX; 
        float yxTempSize = yTempSize * sizeRealationY;

        //Debug.Log("X");
        //Debug.Log(xTempSize);
        //Debug.Log("XY");
        //Debug.Log(xyTempSize);
        //Debug.Log("Y");
        //Debug.Log(yTempSize);
        //Debug.Log("YX");
        //Debug.Log(yxTempSize);

        if (xTempSize < yxTempSize && yTempSize > xyTempSize) {
            sizeX = xTempSize;
            sizeY = xTempSize * sizeRealationX;

            //Debug.Log("x kleiner");
        }
        else {
            sizeY = yTempSize;
            sizeX = yTempSize * sizeRealationY;
          //  Debug.Log("y kleiner");
        }


        yoffset = yoffset + ((tempPanelY - (sizeY * yTiles))/2);
        xoffset = xoffset + ((tempPanelX - (sizeX * xTiles)) / 2);

        //tiles + 1 = linen anzahl
        // tiles = verbindungslinen anzahl
        // + 2 für anfangspunkt
        int lineCount = (yTiles + 1 + yTiles + xTiles + 1 + xTiles) + 2;
        
        Vector3[] v3 = new Vector3[lineCount];
        float y = sizeY * yTiles;
        int yt1 = 1;
        int yt2 = 1;
        int xt1 = 0;
        int xt2 = 1;
        float x = 0;
        int i;
        for (i = 0; i < (xTiles + 1 + xTiles + 1);) {

            //v3[0] = new Vector3(0 + xoffset, (-size * yTiles) - yoffset, 0);

            x = x + ((xt1) * sizeX);
            v3[i] = new Vector3(x + xoffset,( yt1 * (-y)) - yoffset , 0);

            // Reihenfolge: 1 0 0 1 1 0 0 1 1...
            yt1 = (yt1 + yt2) % 2;

            yt2 = (yt2 + 1) % 2;

            // Reiehenfolge: 0 0 1 0 1...
            xt1 = i % 2;
            i = i + 1;
        }
        y = yt1 * (-y);
        x = sizeX * xTiles;
        yt1 = 0;
        yt2 = 1;
        xt1 = 0;
        xt2 = 1;


        int yUp = 1;
        if (((xTiles + 1) % 2) == 1) {
            yUp = -1;
        }

        for (; i < lineCount;) {

            y = y + (((yt1) * sizeY) * yUp);
            v3[i] = new Vector3((xt1 * (x)) + xoffset, y - yoffset, 0);

            // Reihenfolge: 0 1 1 0 0 1 1...
            xt1 = (xt1 + xt2) % 2;

            xt2 = (xt2 + 1) % 2;

            // Reiehenfolge: 0 0 1 0 1...
            yt1 = i % 2;
            i = i + 1;

        }
        return v3;
    }

    public void setPaintObj(GameObject newPaintObj) {
        paintObj = newPaintObj;
    }

    // gibt die position in abhänigkeit des relativen Vecotrs ( cellVector) zurück
    public Vector3 cellVectorToCellPos(Vector3 v3) {

        Vector3 returnVector = new Vector3(0, 0, 0);
        RectTransform panel = gameObject.GetComponent<RectTransform>();


        // -4 damits die linen schöner aufs pannel gezeichnet werden
        tempPanelX = panel.rect.width - 4;
        tempPanelY = panel.rect.height - 4;

        float xoffset = 2;
        float yoffset = 2;

        float sizeRealationX = spriteSizeY / spriteSizeX;
        float sizeRealationY = spriteSizeX / spriteSizeY;

        //if(sizeRealationX > sizeRealationY) {
        //    sizeRealationY = 1;
        //}
        //else {
        //    sizeRealationX = 1;
        //}
        //Debug.Log("XRealation");
        //Debug.Log(sizeRealationX);
        //Debug.Log("YRealation");
        //Debug.Log(sizeRealationY);

        float xTempSize = tempPanelX / xTiles;
        float yTempSize = tempPanelY / yTiles;


        //

        float xyTempSize = xTempSize * sizeRealationX;
        float yxTempSize = yTempSize * sizeRealationY;

        //Debug.Log("X");
        //Debug.Log(xTempSize);
        //Debug.Log("XY");
        //Debug.Log(xyTempSize);
        //Debug.Log("Y");
        //Debug.Log(yTempSize);
        //Debug.Log("YX");
        //Debug.Log(yxTempSize);

        if (xTempSize < yxTempSize && yTempSize > xyTempSize) {
            sizeX = xTempSize;
            sizeY = xTempSize * sizeRealationX;

            
        }
        else {
            sizeY = yTempSize;
            sizeX = yTempSize * sizeRealationY;
            
        }
        Debug.Log("x");
        Debug.Log(sizeX);
       


        yoffset = yoffset + ((tempPanelY - (sizeY * yTiles)) / 2);
        xoffset = xoffset + ((tempPanelX - (sizeX * xTiles)) / 2);

        Debug.Log("xoffset");
        Debug.Log(xoffset);

        Vector3 origin = new Vector3(0 + xoffset, (-sizeY * yTiles) - yoffset, 0);

        Vector3 wv3 = panel.TransformPoint(new Vector3(origin.x + (sizeX * v3.x), origin.y + (sizeY * v3.y), origin.z));


        Rect cell = new Rect(wv3.x, wv3.y, sizeX, sizeY);

        returnVector = cell.center;

        return returnVector;
    }

    public List<Cellmap> cellPositions(Vector3 v3, bool noPaintObj) {


        List<Cellmap> cells = new List<Cellmap>();

        int cellsNum = yTiles * xTiles;
       

        cells.Clear();

        
        int col = 0;
        int row = 0;
        RectTransform panel = gameObject.GetComponent<RectTransform>();
      
        for (int i = 0; i < cellsNum;) {

            Vector3 wv3 = panel.TransformPoint(new Vector3 (v3.x + (sizeX * col), v3.y + (sizeY * row), v3.z));
            

            Cellmap cell = new Cellmap(new Rect(wv3.x, wv3.y, sizeX, sizeY),new Vector3Int(col,row,0));  
            
            
             

            cells.Add(cell);
            row = row + 1;
            
            

            i = i + 1;
            if (i % yTiles == 0) {

                col = col + 1;
                row = 0;
            }

        }

        if( noPaintObj == false) {
            paintObj.GetComponent<paint>().cells = cells;
        }
       

        return cells;

    }

    private void cellChange() {
        RectTransform panel = gameObject.GetComponent<RectTransform>();
        Vector3[] v3 = drawGrid(panel);
        List<Cellmap>  cells = cellPositions(v3[0],true);
        List<Edit_tile> delete = new List<Edit_tile>();
        foreach(Edit_tile tile in tileList) {
            
            if (tile.Position.x >= xTiles || tile.Position.y >= yTiles) {
                Destroy(tile.Tile);
                delete.Add(tile);
            }
            
            else {
                tile.Tile.transform.position = cellVectorToCellPos(tile.Position);
                float scaleX = sizeX / tile.Tile.GetComponent<RectTransform>().rect.width;
                float scaleY = sizeY / tile.Tile.GetComponent<RectTransform>().rect.height;
                tile.Tile.transform.localScale = new Vector3(scaleX, scaleY, 1);
                 
            }
        }
        foreach (Edit_tile tile in delete) {
            tileList.Remove(tile);
        }

    }

    public void onchangedxTiles(string input) {
        xTiles = int.Parse(input);
        editmap.XTiles =xTiles;
       // cellChange();
    }
    public void onchangedyTiles(string input) {
        yTiles = int.Parse(input);
        editmap.YTiles = yTiles;
        //cellChange();
    }

    public void onEndChangedxTiles(string input) {
        xTiles = int.Parse(input);
        editmap.XTiles = xTiles;
        cellChange();
    }
    public void onEndChangedyTiles(string input) {
        yTiles = int.Parse(input);
        editmap.YTiles = yTiles;
        cellChange();
    }

    public void onchangedGrid(Boolean input) {
        grid = input;
    }
    public void onchangedColor(Boolean input) {
        if (input == true) {
            color = new Color(255, 255, 255);
        }
        else {
            color = new Color(0, 0, 0);
        }
    }
    public void onclickBackground() {

        currentlayer = 1;
        layerText.text = "1";
        enableButton();
    }
    public void onclickRiver() {

        currentlayer = 2;
        layerText.text = "2";
        enableButton();
    }
    public void onclickRoad() {

        currentlayer = 3;
        layerText.text = "3";
        enableButton();
    }
    public void onclickDeko() {

        currentlayer = 4;
        layerText.text = "4";
        enableButton();
    }
    public void onclickUpDown(bool up) {
        if(up == true) {
            currentlayer = currentlayer + 1;
            layerText.text = currentlayer.ToString();
        }
        else {
            currentlayer = currentlayer - 1;
            layerText.text = currentlayer.ToString();
        }
        enableButton();


    }

    public void enableButton() {
        if (currentlayer < 1) {
            downButton.interactable = false;
            upButton.interactable = false;
            toggleGrid.interactable = false;

        }
        else if (currentlayer < 2) {
            downButton.interactable = false;
            upButton.interactable = true;
            toggleGrid.interactable = true;

        }
        else if (currentlayer >= 99) {
            upButton.interactable = false;
            downButton.interactable = true;
            toggleGrid.interactable = true;
        }
        else {
            downButton.interactable = true;
            upButton.interactable = true;
            toggleGrid.interactable = true;
        }
    }

    public void save() {
        Map saveMap = new Map(xTiles, yTiles);
        List<Edit_tile> saveList = new List<Edit_tile>();

        foreach(Edit_tile copyTile in tileList) {
            saveList.Add(copyTile);
        }

        while (saveList.Count() != 0) {


            Edit_tile saveTile = saveList.First();

            List<Edit_tile> saveTiles = tileList.FindAll(x => x.Filename == saveTile.Filename && x.Layer == saveTile.Layer);
            Tileplace saveTileplace = new Tileplace(saveTile.Filename, saveTile.Layer);
            foreach (Edit_tile savingTile in saveTiles) {
                saveTileplace.addTiles(savingTile.Position);
                saveList.Remove(savingTile);
                
            }
            saveMap.addTiles(saveTileplace);
        }

        string json = JsonUtility.ToJson(saveMap);

        //string path = EditorUtility.SaveFilePanel("Speichern", "", "map.json", "json");
        //File.WriteAllText(path, json);

        FileBrowser.SetFilters(true, new FileBrowser.Filter("json", ".json"));
        FileBrowser.ShowSaveDialog((path) => { fileSave(path, json); }, null, FileBrowser.PickMode.Files, false, null, "map.json", "Speichern");

       
        

    }

    private void fileSave(string[] path, string data) {
        File.WriteAllText(path[0], data);
    }


    public void infoButton() {

        if(info_panel_state == true) {
            info_panel_state = false;

        }
        else {
            info_panel_state = true;
        }

        info_panel.gameObject.SetActive(info_panel_state);
    }

    public void openMap() {

        // string path = EditorUtility.OpenFilePanel("Map", "", "json");

        FileBrowser.SetFilters(true, new FileBrowser.Filter("json", ".json"));
        FileBrowser.ShowLoadDialog((path) => { openMapSucces(path); }, null, FileBrowser.PickMode.Files, false, null, null, "Map");
        
    }

    private void openMapSucces(string[] path) {
        string json = File.ReadAllText(path[0]);

        int MaxLayer = 1;

        if (json == null || json == "") {
            // nichts machen da leeres file geladen
            return;
        }
        Map savedMap = JsonUtility.FromJson<Map>(json);

        if (savedMap == null) {
            // nichts machen da kein objekt erstellt worden ist
            return;
        }

        foreach (Edit_tile currentTiles in tileList) {
            Destroy(currentTiles.Tile);
        }
        tileList.Clear();

        xTiles = savedMap.XTiles;
        yTiles = savedMap.YTiles;

        xField.text = xTiles.ToString();
        yField.text = yTiles.ToString();

        float scaleX = sizeX / spriteSizeX;
        float scaleY = sizeY / spriteSizeY;

        foreach (Tileplace newTiles in savedMap.getTiles()) {
            foreach (Vector3 pos in newTiles.getTiles()) {

                SpriteRenderer tile = Instantiate(tilePrefab, transform);

                tile.transform.localScale = new Vector3(scaleX, scaleY, 1);
                Debug.Log(newTiles.Filename);

                // tile.GetComponent<tile_layer>().sp = AssetDatabase.LoadAssetAtPath(newTiles.Filename, typeof(Sprite)) as Sprite;
                tile.GetComponent<tile_layer>().sp = Resources.Load(newTiles.Filename, typeof(Sprite)) as Sprite;
                tile.sortingOrder = newTiles.Layer;

                Vector3 cellPos = cellVectorToCellPos(pos);

                tile.transform.position = new Vector3(cellPos.x, cellPos.y, 0);
                tile.transform.localPosition = new Vector3(tile.transform.localPosition.x, tile.transform.localPosition.y, 0);


                Edit_tile selectedTile = new Edit_tile(tile.gameObject, pos, newTiles.Layer, newTiles.Filename);

                tileList.Add(selectedTile);
            }
            if (MaxLayer < newTiles.Layer) {
                MaxLayer = newTiles.Layer;
            }
        }

        currentlayer = MaxLayer;
        layerText.text = currentlayer.ToString();
        enableButton();

        onEndChangedxTiles(xTiles.ToString());

        onEndChangedyTiles(yTiles.ToString());

    }
}
