using System.Collections.Generic;
using UnityEngine;

public class paint : MonoBehaviour {

    public RectTransform panel;
    private RectTransform rect;
    private Rect currentRect;




    public float spriteSizeX;
    public float spriteSizeY;
    public int delete;

    public string spritePath;
    public SpriteRenderer spriteRend;
    public SpriteRenderer tilePrefab;

    private Sprite sprite;
    public List<Cellmap> cells;

    private Vector3 currentCell;
    private Vector3Int currentCellPos;
    private Vector3 currentMousePos;
    private Vector3 noGridCellPos;
    private Vector3 noGridCell;
    private bool noCell;
    private int currentLayer;

    // Start is called before the first frame update
    void Start() {


    }

    private void Awake() {
        gameObject.TryGetComponent<SpriteRenderer>(out spriteRend);

    }

    public void setSprite(string sp) {
        spritePath = sp;
        //  spriteRend.sprite = AssetDatabase.LoadAssetAtPath(spritePath, typeof(Sprite)) as Sprite;
        spriteRend.sprite = Resources.Load(spritePath, typeof(Sprite)) as Sprite;
    }

    // Update is called once per frame
    void Update() {
        bool outside = true;
        if (panel != null) {

            currentLayer = panel.GetComponent<paint_grid>().currentlayer;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = currentLayer;
            Vector3 v3 = Input.mousePosition;
            Camera cam = Camera.main;
            transform.position = cam.ScreenToWorldPoint(v3);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            currentMousePos = transform.position;

            float scaleX = panel.GetComponent<paint_grid>().sizeX / spriteSizeX;
            float scaleY = panel.GetComponent<paint_grid>().sizeY / spriteSizeY;


            transform.localScale = new Vector3(scaleX, scaleY, 1);


            noCell = true;







            foreach (Cellmap cell in cells) {
                if (cell.Rect.Contains((Vector2)transform.position)) {

                    if (panel.GetComponent<paint_grid>().grid == false) {

                        Vector3 origin = cell.Rect.min;
                        float deltaX = currentMousePos.x - origin.x;
                        float deltaY = currentMousePos.y - origin.y;
                        float sizeX = panel.GetComponent<paint_grid>().sizeX;
                        float sizeY = panel.GetComponent<paint_grid>().sizeY;

                        //Debug.Log(deltaX);
                        //Debug.Log(deltaY);

                        // prozentualer offset 
                        noGridCellPos = new Vector3((float)System.Math.Round(cell.CellPos.x + (deltaX / sizeX), 3), (float)System.Math.Round(cell.CellPos.y + (deltaY / sizeY), 3), 0);

                        //Debug.Log(noGridCellPos);
                        currentCell = cell.Rect.center;
                        currentCellPos = cell.CellPos;


                        noGridCell = new Vector3(cell.Rect.xMin + ((noGridCellPos.x - currentCellPos.x) * sizeX), cell.Rect.yMin + ((noGridCellPos.y - currentCellPos.y) * sizeY), 0);

                        // size mit 0.99 multiplizieren damit angrenzende rects nicht gelöscht weren
                        currentRect = new Rect(transform.position.x - (((spriteSizeX * scaleX) / 2) * 0.99f), transform.position.y - (((spriteSizeY * scaleY) / 2) * 0.99f), (spriteSizeX * scaleX) * 0.99f, (spriteSizeY * scaleY) * 0.99f);
                        outside = false;
                        break;
                    }
                    else {


                        transform.position = cell.Rect.center;
                        currentCell = cell.Rect.center;

                        currentCellPos = cell.CellPos;
                        // size mit 0.99 multiplizieren damit angrenzende rects nicht gelöscht weren
                        currentRect = new Rect(transform.position.x - (((spriteSizeX * scaleX) / 2) * 0.99f), transform.position.y - (((spriteSizeY * scaleY) / 2) * 0.99f), (spriteSizeX * scaleX) * 0.99f, (spriteSizeY * scaleY) * 0.99f);

                        noCell = false;
                        outside = false;
                        break;
                    }
                }
            }


        }
        if (Input.GetMouseButtonDown(0) && outside == false) {
            // Debug.Log("The Left mouse button was pressed");
            float scaleX = panel.GetComponent<paint_grid>().sizeX / spriteSizeX;
            float scaleY = panel.GetComponent<paint_grid>().sizeY / spriteSizeY;
            if (noCell == false) {




                List<Edit_tile> editList = panel.GetComponent<paint_grid>().tileList;
                List<Edit_tile> curentLayer = new List<Edit_tile>();
                Edit_tile selectedTile = null;


                if (currentLayer < 4) {
                    selectedTile = editList.Find(x => x.Layer == currentLayer && x.Position == currentCellPos);
                }
                else {
                    curentLayer = editList.FindAll(x => x.Layer == currentLayer);

                    List<Edit_tile> destroyList = new List<Edit_tile>();
                    bool destroy = false;
                    foreach (Edit_tile selectedLayerTile in curentLayer) {


                        Rect selectedRect = new Rect(selectedLayerTile.Tile.transform.position.x - ((spriteSizeX * scaleX) / 2), selectedLayerTile.Tile.transform.position.y - ((spriteSizeY * scaleY) / 2), spriteSizeX * scaleX, spriteSizeY * scaleY);

                        if (selectedRect.Overlaps(currentRect)) {

                            destroyList.Add(selectedLayerTile);
                            destroy = true;


                        }
                    }
                    if (destroy == true) {
                        foreach (Edit_tile selectedLayerTile in destroyList) {
                            panel.GetComponent<paint_grid>().tileList.Remove(selectedLayerTile);

                            Destroy(selectedLayerTile.Tile);
                        }
                    }
                }

                //Debug.Log(index);




                if (selectedTile != null) {

                    panel.GetComponent<paint_grid>().tileList.Remove(selectedTile);

                    Destroy(selectedTile.Tile);

                    //panel.GetComponent<paint_grid>().tileList.RemoveAt(index);


                }

                if (delete == 0) {
                    SpriteRenderer tile = Instantiate(tilePrefab, panel);
                    tile.transform.localScale = new Vector3(scaleX, scaleY, 1);


                    //tile.GetComponent<tile_layer>().sp = AssetDatabase.LoadAssetAtPath(spritePath, typeof(Sprite)) as Sprite;
                    tile.GetComponent<tile_layer>().sp = Resources.Load(spritePath, typeof(Sprite)) as Sprite;
                    tile.sortingOrder = currentLayer;


                    tile.transform.position = new Vector3(currentCell.x, currentCell.y, 0);
                    tile.transform.localPosition = new Vector3(tile.transform.localPosition.x, tile.transform.localPosition.y, 0);


                    selectedTile = new Edit_tile(tile.gameObject, currentCellPos, currentLayer, spritePath);

                    panel.GetComponent<paint_grid>().tileList.Add(selectedTile);
                }


            }
            else {
                List<Edit_tile> editList = panel.GetComponent<paint_grid>().tileList;
                List<Edit_tile> curentLayer = new List<Edit_tile>();


                curentLayer = editList.FindAll(x => x.Layer == currentLayer);
                List<Edit_tile> destroyList = new List<Edit_tile>();
                bool destroy = false;
                foreach (Edit_tile selectedTile in curentLayer) {


                    Rect selectedRect = new Rect(selectedTile.Tile.transform.position.x - ((spriteSizeX * scaleX) / 2), selectedTile.Tile.transform.position.y - ((spriteSizeY * scaleY) / 2), spriteSizeX * scaleX, spriteSizeY * scaleY);

                    if (selectedRect.Overlaps(currentRect)) {

                        destroyList.Add(selectedTile);
                        destroy = true;


                    }
                }
                if (destroy == true) {
                    foreach (Edit_tile selectedTile in destroyList) {
                        panel.GetComponent<paint_grid>().tileList.Remove(selectedTile);

                        Destroy(selectedTile.Tile);
                    }
                }
                // here if setzen

                if (delete == 0) {
                    Edit_tile newTile = null;

                    SpriteRenderer tile = Instantiate(tilePrefab, panel);
                    tile.transform.localScale = new Vector3(scaleX, scaleY, 1);


                    // tile.GetComponent<tile_layer>().sp = AssetDatabase.LoadAssetAtPath(spritePath, typeof(Sprite)) as Sprite;
                    tile.GetComponent<tile_layer>().sp = Resources.Load(spritePath, typeof(Sprite)) as Sprite;
                    tile.sortingOrder = currentLayer;

                    //Debug.Log(noGridCell);
                    tile.transform.position = new Vector3(noGridCell.x, noGridCell.y, 0);
                    tile.transform.localPosition = new Vector3(tile.transform.localPosition.x, tile.transform.localPosition.y, 0);

                    newTile = new Edit_tile(tile.gameObject, noGridCellPos, currentLayer, spritePath);

                    panel.GetComponent<paint_grid>().tileList.Add(newTile);
                }


            }


        }
        if (Input.GetMouseButtonDown(1)) {
            //Debug.Log("The right mouse button was pressed");
            Destroy(gameObject);
        }
    }
}
