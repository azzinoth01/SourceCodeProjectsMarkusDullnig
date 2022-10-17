using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class set_tiles : MonoBehaviour {
    public List<Tilemap> tilemap;
    private float tempPanelY;
    private float tempPanelX;
    private float xoffset;
    private float yoffset;

    public Grid grid;
    public int xTiles;
    public int yTiles;
    public float tilesize;
    public Tilemap prefab_map;
    public RectTransform currentpanel;
    public RectTransform gridTransform;

    public float sizeX;
    public float sizeY;
    public Map m;
    public int maximumLayer;
    public List<string> powerupList;

    public SpriteRenderer powerUpPrefab;
    private SpriteRenderer powerUp;

    public List<Vector3Int> TopLayerCells;
    public List<Vector3Int> LowerLayerCells;




    // Start is called before the first frame update
    void Start() {
        globals.mainGrid = gameObject.GetComponent<Grid>();
        globals.powerupGameObjectList = new List<GameObject>();
        TopLayerCells = new List<Vector3Int>();
        LowerLayerCells = new List<Vector3Int>();
        maximumLayer = 0;


        //background = Instantiate(prefab_map, grid.transform);
        //if (background.TryGetComponent<TilemapRenderer>(out var backgroundRender)) {
        //    backgroundRender.sortingOrder = 0;
        //}


        //Map m = new Map(xTiles, yTiles);
        //Tileplace tile = new Tileplace("Assets/pallets/Tiles_forest/42.asset", 0);

        //Camera.main.transform.position = new Vector3(tilesize * ((float)xTiles / 2), tilesize * ((float)yTiles / 2), -10);

        //Tile t = AssetDatabase.LoadAssetAtPath("Assets/pallets/Tiles_forest/42.asset", typeof(Tile)) as Tile;
        //for (int x = 0; x < xTiles;) {
        //    for (int y = 0; y < yTiles;) {
        //        Vector3Int v3 = new Vector3Int(x, y, 0);
        //        background.SetTile(v3, t);
        //        tile.addTiles(v3);

        //        y = y + 1;
        //    }
        //    x = x + 1;
        //}

        //m.addTiles(tile);
        //string json = JsonUtility.ToJson(m);

        //string path = EditorUtility.SaveFilePanel("test", "", "map.json", "json");

        //StreamWriter writer = new StreamWriter(path);
        //writer.Write(json);
        //writer.Flush();
        //writer.Close();
        //Debug.Log(json);
        //Debug.Log("test");

        globals.map_tiles = 0;

        // string path = EditorUtility.OpenFilePanel("test", "", "json");
        string path = "";

        string json;
        if (globals.customLevel == 1) {
            path = globals.customLevelPath;
            json = File.ReadAllText(path);
        }
        else if (globals.arcadeLevel != 0) {
            // liste von zu ladenenden levels

            //path = "Assets/Resources/maps/map_" + globals.arcadeLevel + ".json";
            path = "maps/map_" + globals.arcadeLevel;
            TextAsset text = Resources.Load(path, typeof(TextAsset)) as TextAsset;
            json = text.text;
        }
        else {
            // zum main menu zurück weil es kein zu ladendes level gibt
            SceneManager.LoadScene("main_menue", LoadSceneMode.Single);
            return;
        }

        //Debug.Log(path);

        // StreamReader reader = new StreamReader(path);
        //string json = reader.ReadToEnd();




        //string json = File.ReadAllText(path);





        //Debug.Log(json);
        // Debug.Log("test");
        m = JsonUtility.FromJson<Map>(json);

        xTiles = m.XTiles;
        yTiles = m.YTiles;

        sizeMap(currentpanel);



        float scaleX = sizeX / grid.cellSize.x;
        float scaleY = sizeY / grid.cellSize.y;



        //Debug.Log("scaleX");
        //Debug.Log(scaleX);
        //Debug.Log("scaleY");
        //Debug.Log(scaleY);

        //grid.transform.localScale.Set(scaleX, scaleY, 1);
        Vector3 scale = new Vector3(scaleX, scaleY, 1);

        gridTransform.localScale = scale;


        gridTransform.SetLeft(xoffset);
        gridTransform.SetRight(xoffset);
        gridTransform.SetBottom(yoffset);






        //   Camera.main.transform.position = new Vector3(tilesize * ((float)m.XTiles / 2), tilesize * ((float)m.YTiles / 2), -10);

        tilemap = new List<Tilemap>();

        foreach (Tileplace tile in m.getTiles()) {
            Tilemap map = null;

            if (maximumLayer < tile.Layer) {
                maximumLayer = tile.Layer;
            }
            if (tilemap.Count() != 0) {

                int ibreak = 0;
                foreach (Tilemap tm in tilemap) {
                    if (tm.TryGetComponent<TilemapRenderer>(out var render)) {
                        if (render.sortingOrder == tile.Layer) {
                            map = tm;
                            ibreak = 1;
                            break;
                        }

                    }
                }
                if (ibreak == 0) {
                    map = Instantiate(prefab_map, grid.transform);
                    if (map.TryGetComponent<TilemapRenderer>(out var render)) {
                        render.sortingOrder = tile.Layer;
                    }
                    tilemap.Add(map);
                }

            }
            else {
                map = Instantiate(prefab_map, grid.transform);
                if (map.TryGetComponent<TilemapRenderer>(out var render)) {
                    render.sortingOrder = tile.Layer;

                }
                tilemap.Add(map);
            }
            string filename = tile.Filename;
            Debug.Log(filename);
            // Korrektur für die map data nach umbau von AssetDatabase.LoadAssetAtPath() nach Resources.Load()
            filename = filename.Replace("Assets/", "");
            filename = filename.Replace("Tiles/", "pallets/Tiles/");
            filename = filename.Replace(".png", "");
            Debug.Log(filename);


            //Tile t = AssetDatabase.LoadAssetAtPath(tile.Filename, typeof(Tile)) as Tile;
            //Tile t = AssetDatabase.LoadAssetAtPath("Assets/pallets/Tiles/02-Breakout-Tiles.asset", typeof(Tile)) as Tile;
            //Tile t = AssetDatabase.LoadAssetAtPath(filename, typeof(Tile)) as Tile;
            Tile t = Resources.Load(filename, typeof(Tile)) as Tile;
            //Debug.Log(t.sprite);
            foreach (Vector3 v3 in tile.getTiles()) {
                Vector3Int v3int = new Vector3Int((int)v3.x, (int)v3.y, (int)v3.z);
                map.SetTile(v3int, t);

                //Tile data = (Tile) map.GetTile(v3int);
                //Debug.Log(data.sprite);

                globals.map_tiles = globals.map_tiles + 1;

                //Debug.Log("Vector");
                //Debug.Log(v3int);
                //Debug.Log("world");
                //Debug.Log(map.CellToWorld(v3int));
                //Debug.Log("Local");
                //Debug.Log(map.CellToLocal(v3int));
                int iBreak = 0;
                foreach (string s in powerupList) {
                    //  Debug.Log(s.Length);
                    string s2 = t.sprite.ToString();
                    s2 = s2.Replace(" (UnityEngine.Sprite)", ""); // scheinbar schreibt es das jedesmal in den string mit rein beim converten

                    if (s == s2) {

                        globals.map_tiles = globals.map_tiles - 1;

                        //Debug.Log("test");
                        map.SetTile(v3int, null);
                        powerUp = Instantiate(powerUpPrefab, transform.parent.parent);
                        //AssetDatabase.LoadAssetAtPath("Assets/Tiles/" + t.sprite + ".png", typeof(Sprite)) as Sprite;
                        powerUp.sprite = t.sprite;
                        powerUp.transform.localScale = grid.transform.localScale;
                        powerUp.transform.position = map.CellToWorld(v3int); // damit die origin position des tiles verwendet wird
                        Vector3 v3pos = powerUp.transform.position;
                        //  Debug.Log("position");
                        //  Debug.Log(v3pos);
                        //  Debug.Log("position after");
                        //  Debug.Log("scale");
                        //  Debug.Log(grid.transform.localScale.x);
                        v3pos.x = v3pos.x + ((powerUp.transform.GetComponent<RectTransform>().rect.width * grid.transform.localScale.x) / 2);
                        v3pos.y = v3pos.y + ((powerUp.transform.GetComponent<RectTransform>().rect.height * grid.transform.localScale.y) / 2);
                        //  Debug.Log(v3pos);
                        powerUp.transform.position = v3pos;
                        powerUp.sortingOrder = tile.Layer;

                        globals.powerupGameObjectList.Add(powerUp.gameObject);
                        iBreak = 1;
                        break;
                    }

                }
                if (iBreak == 0) {
                    Vector3Int cell = new Vector3Int(v3int.x, v3int.y, tile.Layer);
                    Vector3Int cell2 = TopLayerCells.Find(l => l.x == cell.x && l.y == cell.y);

                    if (cell.z > cell2.z) {
                        TopLayerCells.Remove(cell2);
                        TopLayerCells.Add(cell);

                        LowerLayerCells.Add(cell2);
                    }
                    else {
                        LowerLayerCells.Add(cell);
                    }

                }



            }
        }
        // Debug.Log("start");

        globals.TopLayerCells = TopLayerCells;

        globals.LowerLayerCells = LowerLayerCells;

        globals.tilemapList = tilemap;


        globals.start = 1;
    }

    // Update is called once per frame
    void Update() {




        sizeMap(currentpanel);



        float scaleX = sizeX / grid.cellSize.x;
        float scaleY = sizeY / grid.cellSize.y;



        //Debug.Log("scaleX");
        //Debug.Log(scaleX);
        //Debug.Log("scaleY");
        //Debug.Log(scaleY);

        //grid.transform.localScale.Set(scaleX, scaleY, 1);
        Vector3 scale = new Vector3(scaleX, scaleY, 1);

        globals.levelScale = scale;

        gridTransform.localScale = scale;


        gridTransform.SetLeft(xoffset);
        gridTransform.SetRight(xoffset);
        gridTransform.SetBottom(yoffset);






    }

    public void sizeMap(RectTransform panel) {


        // -4 damits die linen schöner aufs pannel gezeichnet werden
        tempPanelX = panel.rect.width - 4;
        tempPanelY = panel.rect.height - 4;

        xoffset = 2;
        yoffset = 2;

        float sizeRealationX = grid.cellSize.y / grid.cellSize.x;
        float sizeRealationY = grid.cellSize.x / grid.cellSize.y;

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




        yoffset = yoffset + ((tempPanelY - (sizeY * yTiles)) / 2);
        xoffset = xoffset + ((tempPanelX - (sizeX * xTiles)) / 2);


        //Debug.Log("yoffset After ");
        //Debug.Log(yoffset);
        //Debug.Log("xoffset After");
        //Debug.Log(xoffset);
    }
}

