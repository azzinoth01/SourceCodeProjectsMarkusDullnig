using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ballmove : MonoBehaviour {

    public int yDirection;
    public int xDirection;
    public float speed;
    private int col;
    public float angel;
    public RectTransform panel;
    public Grid grid;
    private List<Collision2D> collist;
    public List<string> powerupList;
    public GameObject padle;
    public GameObject ballPrefab;
    public int stay;
    public float waitTime;
    public int pierce;
    public int extraBall;
    public AudioSource hit_audio;
    private SpriteRenderer animation;
    public SpriteRenderer animationPrefab;

    private Vector3[] oldBallPosition;
    private int cellListLoaded;


    // Start is called before the first frame update
    void Start() {
        if (extraBall == 1) {
            stay = 0;
            waitTime = 0;
        }
        else {
            yDirection = 1;
            xDirection = 1;
            globals.ballList = new List<GameObject>();
            angel = 45;
            waitTime = 1;
            stay = 1;

        }
        cellListLoaded = 0;
        pierce = 0;
        col = 0;
        oldBallPosition = new Vector3[4];

        collist = new List<Collision2D>();


        globals.ballList.Add(gameObject);


        //float startY;
        //float startX;

        //startX = padle.transform.position.x;

        //Debug.Log("StartY");
        //Debug.Log(startY);
        //transform.position = new Vector3(startX, startY, -90);

    }

    // Update is called once per frame
    void FixedUpdate() {
        // block wenn alert screen pops up
        if (globals.AlertScreen == true) {
            return;
        }

        if (globals.TopLayerCells == null) {
            globals.TopLayerCells = new List<Vector3Int>();
        }
        if (globals.LowerLayerCells == null) {
            globals.LowerLayerCells = new List<Vector3Int>();
        }
        if (globals.tilemapList == null) {
            globals.tilemapList = new List<Tilemap>();
        }
        if (globals.TopLayerCells.Count != 0 && globals.LowerLayerCells.Count != 0 && globals.tilemapList.Count != 0 && cellListLoaded == 0) {
            cellListLoaded = 1;
        }


        transform.localScale = globals.levelScale / 2.6f;

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0)) {
            stay = 0;




        }

        if (stay == 1) {


            //float oldY = transform.position.y;
            Camera cam = Camera.main;
            Vector3 v3 = Input.mousePosition;
            Vector3 pos = cam.ScreenToWorldPoint(v3);

            float ballSize = (GetComponent<RectTransform>().rect.height * transform.localScale.y) / 2;
            float startY = padle.transform.position.y;
            float startX = padle.transform.position.x;

            // pos = new Vector3(pos.x, startY, pos.z);

            // transform.position = pos;


            // transform.position = new Vector3(transform.position.x,startY, 0);
            transform.position = new Vector3(startX, startY, 0);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + ballSize + ((padle.GetComponent<RectTransform>().rect.height * padle.transform.localScale.y) / 2), transform.localPosition.z);
        }

        else if (stay == 0) {
            if (waitTime >= 0) {
                waitTime = waitTime - Time.fixedDeltaTime;
            }


            float xMove = 0;
            float yMove = 0;

            //int MaxLayer = 0;




            //foreach (Collision2D collision in collist) {
            //    Tilemap tilemap = collision.gameObject.GetComponent<Tilemap>();
            //    if (tilemap.TryGetComponent<TilemapRenderer>(out var render)) {

            //        if (MaxLayer < render.sortingOrder) {
            //            MaxLayer = render.sortingOrder;
            //        }

            //    }
            //}


            //foreach (Collision2D collision in collist.Where(x => x.gameObject.GetComponent<TilemapRenderer>().sortingOrder == MaxLayer)) {
            //    Tilemap tilemap = collision.gameObject.GetComponent<Tilemap>();

            //    //Debug.Log("Contact Point");

            //    int xchange = 0;
            //    int ychange = 0;

            //    Vector3 hitPosition = Vector3.zero;

            //    foreach (ContactPoint2D hit in collision.contacts) {

            //        hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
            //        hitPosition.y = hit.point.y - 0.01f * hit.normal.y;

            //        if (tilemap.GetTile(tilemap.WorldToCell(hitPosition)) != null) {
            //            globals.map_tiles = globals.map_tiles - 1;
            //          //  Debug.Log("map tiles remaining");
            //           // Debug.Log(globals.map_tiles);
            //        }
            //        tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);

            //        tilemap.CellToWorld(new Vector3Int(0, 0, 0));
            //        // Debug.Log("MaxLayer");
            //        //Debug.Log(MaxLayer);
            //        //check below layer if power up




            //        if( pierce == 0) {
            //            if (hit.normal.x != 0) {

            //                xchange = 1;
            //            }
            //            if (hit.normal.y != 0) {
            //                ychange = 1;
            //            }
            //        }

            //    }


            //    if (xchange == 1) {


            //        xDirection = xDirection * -1;
            //       // Debug.Log("x change");
            //       // Debug.Log(angel);

            //        angel = 180 - angel;
            //    }
            //    if (ychange == 1) {
            //        yDirection = yDirection * -1;
            //    }

            //}


            //collist.Clear();




            if ((panel.rect.width - (panel.rect.width / 2)) < transform.localPosition.x) {
                xDirection = -1;
                // Debug.Log("border hit change");
                //  Debug.Log(angel);
                angel = 180 - angel;
            }
            else if ((transform.localPosition.x + (panel.rect.width / 2)) <= 0) {
                xDirection = 1;
                //  Debug.Log("border hit change");
                // Debug.Log(angel);
                angel = 180 - angel;
            }

            if ((panel.rect.height - (panel.rect.height / 2)) < transform.localPosition.y) {
                yDirection = -1;
            }
            else if ((transform.localPosition.y + (panel.rect.height / 2)) <= 0) {
                // yDirection = 1;

                globals.ballList.Remove(gameObject);
                if (globals.ballList.Count == 0) {


                    globals.lives = globals.lives - 1;
                    globals.padle.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 5);
                    if (globals.lives > 0) {


                        GameObject newBall = Instantiate(ballPrefab, transform.parent);
                        newBall.GetComponent<ballmove>().grid = grid;
                        newBall.GetComponent<ballmove>().panel = panel;
                        newBall.GetComponent<ballmove>().padle = padle;
                        newBall.GetComponent<ballmove>().ballPrefab = ballPrefab;
                        newBall.GetComponent<ballmove>().hit_audio = hit_audio;
                        newBall.GetComponent<ballmove>().animationPrefab = animationPrefab;

                        padle.GetComponent<move_padle>().resetPos();
                    }
                }
                Destroy(gameObject);


                //Debug.Log("ball zerstört");

                //Debug.Log(globals.lives);



            }
            if (angel == 180 && xDirection == -1) {
                angel = 0;
            }
            else if (angel == 0 && xDirection == 1) {
                angel = 180;
            }
            else if (angel == 180 && xDirection == 1) {
                angel = 180;
            }
            else if (angel == 0 && xDirection == -1) {
                angel = 0;
            }
            else if ((xDirection == 1 && yDirection == 1) || (xDirection == -1 && yDirection == -1)) {

                if (angel > 90) {
                    //  Debug.Log("current angel >90");
                    // Debug.Log(angel);
                    angel = (angel - 180) * -1;
                }
            }
            else if ((xDirection == 1 && yDirection == -1) || (xDirection == -1 && yDirection == 1)) {
                if (angel < 90) {
                    //  Debug.Log("current angel <90");
                    //  Debug.Log(angel);
                    angel = 180 - angel;

                }
            }

            yMove = Mathf.Sin(angel * Mathf.Deg2Rad) * (speed * globals.levelScale.x) * yDirection;
            if (angel == 90) {
                xMove = 0;
            }
            else {
                xMove = Mathf.Sqrt((((speed * globals.levelScale.x) * (speed * globals.levelScale.x)) - (yMove * yMove))) * xDirection;
            }




            gameObject.GetComponent<RectTransform>().GetWorldCorners(oldBallPosition);

            transform.localPosition = new Vector3(transform.localPosition.x + xMove, transform.localPosition.y + yMove, -10);





            if (cellListLoaded == 1) {
                //Debug.Log("world position of cell 0,0,0");
                //Debug.Log(tilemapList[0].CellToWorld(new Vector3Int(0, 0, 0)));

                //Debug.Log("mouse position cell");
                //Debug.Log(tilemapList[0].WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));

                Vector3[] ballCorners = new Vector3[4];
                List<Vector3Int> checkCells = new List<Vector3Int>();
                gameObject.GetComponent<RectTransform>().GetWorldCorners(ballCorners);
                int i = 0;
                int moved = 0;

                while (i < 4) {


                    //Debug.Log(ballCorners[i]);

                    Vector3Int cell = grid.WorldToCell(ballCorners[i]);
                    //Vector3Int cell = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));


                    if (checkCells.Contains(cell)) {
                        i = i + 1;
                        continue;
                    }
                    else {
                        checkCells.Add(cell);
                    }
                    //Debug.Log("Mouse Pos");
                    //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));

                    // wenn pierce aktive dann dauerhaft ausführen bis keine cells mehr gefunden werden
                    //do while(pierce == 1) 
                    do {
                        if (globals.TopLayerCells.Exists(l => l.x == cell.x && l.y == cell.y)) {
                            Vector3Int cell2 = globals.TopLayerCells.First(l => l.x == cell.x && l.y == cell.y);
                            //Debug.Log("cell2");
                            //Debug.Log(cell2);
                            Tilemap tm = globals.tilemapList.Find(l => l.GetComponent<TilemapRenderer>().sortingOrder == cell2.z);
                            if (tm != null) {


                                tm.SetTile(new Vector3Int(cell2.x, cell2.y, 0), null);
                                hit_audio.Play();
                                if (moved == 0 && pierce == 0) {

                                    Vector3 cellCenterLocal = tm.GetCellCenterLocal(new Vector3Int(cell2.x, cell2.y, 0));

                                    Vector3[] cellCornerLocal = new Vector3[4];
                                    cellCornerLocal[0] = new Vector3(cellCenterLocal.x - (grid.cellSize.x / 2), cellCenterLocal.y + (grid.cellSize.y / 2), 0);
                                    cellCornerLocal[1] = new Vector3(cellCenterLocal.x - (grid.cellSize.x / 2), cellCenterLocal.y - (grid.cellSize.y / 2), 0);
                                    cellCornerLocal[2] = new Vector3(cellCenterLocal.x + (grid.cellSize.x / 2), cellCenterLocal.y - (grid.cellSize.y / 2), 0);
                                    cellCornerLocal[3] = new Vector3(cellCenterLocal.x + (grid.cellSize.x / 2), cellCenterLocal.y + (grid.cellSize.y / 2), 0);

                                    //Debug.Log("Local Corner0");
                                    //Debug.Log(cellCornerLocal[0]);
                                    //Debug.Log("Local Corner1");
                                    //Debug.Log(cellCornerLocal[1]);
                                    //Debug.Log("Local Corner2");
                                    //Debug.Log(cellCornerLocal[2]);
                                    //Debug.Log("Local Corner3");
                                    //Debug.Log(cellCornerLocal[3]);

                                    Vector3[] cellCornerWorld = new Vector3[4];

                                    cellCornerWorld[0] = tm.LocalToWorld(cellCornerLocal[0]);
                                    cellCornerWorld[1] = tm.LocalToWorld(cellCornerLocal[1]);
                                    cellCornerWorld[2] = tm.LocalToWorld(cellCornerLocal[2]);
                                    cellCornerWorld[3] = tm.LocalToWorld(cellCornerLocal[3]);

                                    Vector3 closestCorner = cellCornerWorld[0];
                                    int x = 1;
                                    int c = 0;
                                    while (x < 4) {

                                        if (Mathf.Abs(closestCorner.x - oldBallPosition[i].x) >= Mathf.Abs(cellCornerWorld[x].x - oldBallPosition[i].x)) {
                                            if (Mathf.Abs(closestCorner.y - oldBallPosition[i].y) >= Mathf.Abs(cellCornerWorld[x].y - oldBallPosition[i].y)) {
                                                closestCorner = cellCornerWorld[x];
                                                c = x;
                                            }
                                        }

                                        x = x + 1;
                                    }

                                    float b;
                                    float a;
                                    float beta;
                                    int s = 0;

                                    b = oldBallPosition[i].y - closestCorner.y;
                                    a = oldBallPosition[i].x - closestCorner.x;
                                    // möglichen nevativen werte entfernen
                                    b = Mathf.Sqrt(b * b);
                                    a = Mathf.Sqrt(a * a);

                                    beta = Mathf.Atan(a / b) * Mathf.Rad2Deg;

                                    //c = 0 linke obere Ecke
                                    //c = 1 linke untere Ecke
                                    //c = 2 Rechte untere Ecke
                                    //c = 3 Rechte obere Ecke

                                    if (xDirection == -1 && c == 3 && oldBallPosition[i].y < closestCorner.y) {
                                        //Debug.Log("ball hitet rechte Seite 1");
                                        s = 0;
                                    }
                                    else if (xDirection == -1 && c == 2 && oldBallPosition[i].y > closestCorner.y) {
                                        //Debug.Log("ball hitet rechte Seite 2");
                                        s = 0;
                                    }
                                    else if (xDirection == 1 && c == 0 && oldBallPosition[i].y < closestCorner.y) {
                                        //Debug.Log("ball hitet linke Seite 1");
                                        s = 2;
                                    }
                                    else if (xDirection == 1 && c == 1 && oldBallPosition[i].y > closestCorner.y) {
                                        //Debug.Log("ball hitet linke Seite 2");
                                        s = 2;
                                    }
                                    else if (yDirection == 1 && c == 1 && oldBallPosition[i].x > closestCorner.x && oldBallPosition[i].y < closestCorner.y) {
                                        //Debug.Log("ball hitet untere Seite 1");
                                        s = 1;
                                    }
                                    else if (yDirection == 1 && c == 2 && oldBallPosition[i].x < closestCorner.x && oldBallPosition[i].y < closestCorner.y) {
                                        //Debug.Log("ball hitet untere Seite 2");
                                        s = 1;
                                    }
                                    else if (yDirection == -1 && c == 0 && oldBallPosition[i].x > closestCorner.x && oldBallPosition[i].y > closestCorner.y) {
                                        //Debug.Log("ball hitet obere Seite 1");
                                        s = 3;
                                    }
                                    else if (yDirection == -1 && c == 3 && oldBallPosition[i].x < closestCorner.x && oldBallPosition[i].y > closestCorner.y) {
                                        //Debug.Log("ball hitet obere Seite 2");
                                        s = 3;
                                    }
                                    else if (yDirection == 1 && xDirection == -1) {
                                        beta = beta + 90;
                                        if (angel > beta) {
                                            Debug.Log("ball hitet untere Seite 3");
                                            s = 1;
                                        }
                                        else {
                                            Debug.Log("ball hitet rechte Seite 3");
                                            s = 0;
                                        }
                                    }
                                    else if (yDirection == 1 && xDirection == 1) {
                                        beta = 90 - beta;
                                        if (angel < beta) {
                                            Debug.Log("ball hitet untere Seite 4");
                                            s = 1;
                                        }
                                        else {
                                            Debug.Log("ball hitet linke Seite 3");
                                            s = 2;
                                        }
                                    }
                                    else if (yDirection == -1 && xDirection == 1) {
                                        beta = beta + 90;
                                        if (angel > beta) {
                                            Debug.Log("ball hitet obere Seite 3");
                                            s = 3;
                                        }
                                        else {
                                            Debug.Log("ball hitet linke Seite 4");
                                            s = 2;
                                        }
                                    }
                                    else if (yDirection == -1 && xDirection == -1) {
                                        beta = 90 - beta;
                                        if (angel < beta) {
                                            Debug.Log("ball hitet obere Seite 4");
                                            s = 3;
                                        }
                                        else {
                                            Debug.Log("ball hitet rechte Seite 4");
                                            s = 0;
                                        }
                                    }



                                    if (s == 0) {
                                        //ball hitet rechte Seite
                                        if (globals.TopLayerCells.Exists(l => l.x == (cell.x + 1) && l.y == cell.y)) {
                                            // auf der rechtenseite existiert noch ein block
                                            if (i == 3) {
                                                // es ist die letzte prüfung und es kamm noch zu keinen move des balles
                                                a = ballCorners[i].x - tm.LocalToWorld(new Vector3(cellCenterLocal.x + (grid.cellSize.x / 2), cellCenterLocal.y, cellCenterLocal.z)).x;
                                                a = Mathf.Abs(a);
                                                transform.position = new Vector3(transform.position.x + (2 * a), transform.position.y, transform.position.z);

                                                xDirection = xDirection * -1;
                                                moved = 1;
                                            }
                                        }
                                        else {
                                            a = ballCorners[i].x - tm.LocalToWorld(new Vector3(cellCenterLocal.x + (grid.cellSize.x / 2), cellCenterLocal.y, cellCenterLocal.z)).x;
                                            a = Mathf.Abs(a);
                                            transform.position = new Vector3(transform.position.x + (2 * a), transform.position.y, transform.position.z);

                                            xDirection = xDirection * -1;
                                            moved = 1;
                                        }



                                    }
                                    else if (s == 1) {
                                        //ball hitet untere Seite
                                        if (globals.TopLayerCells.Exists(l => l.x == cell.x && l.y == (cell.y - 1))) {
                                            // auf der unterenseite existiert noch ein block
                                            if (i == 3) {
                                                // es ist die letzte prüfung und es kamm noch zu keinen move des balles
                                                b = ballCorners[i].y - tm.LocalToWorld(new Vector3(cellCenterLocal.x, cellCenterLocal.y - (grid.cellSize.y / 2), cellCenterLocal.z)).y;
                                                b = Mathf.Abs(b);
                                                transform.position = new Vector3(transform.position.x, transform.position.y - (2 * b), transform.position.z);

                                                yDirection = yDirection * -1;
                                                moved = 1;
                                            }
                                        }
                                        else {
                                            b = ballCorners[i].y - tm.LocalToWorld(new Vector3(cellCenterLocal.x, cellCenterLocal.y - (grid.cellSize.y / 2), cellCenterLocal.z)).y;
                                            b = Mathf.Abs(b);
                                            transform.position = new Vector3(transform.position.x, transform.position.y - (2 * b), transform.position.z);

                                            yDirection = yDirection * -1;
                                            moved = 1;
                                        }
                                    }
                                    else if (s == 2) {
                                        //ball hitet linke Seite
                                        if (globals.TopLayerCells.Exists(l => l.x == (cell.x - 1) && l.y == cell.y)) {
                                            // auf der linkenseite existiert noch ein block
                                            if (i == 3) {
                                                // es ist die letzte prüfung und es kamm noch zu keinen move des balles
                                                a = tm.LocalToWorld(new Vector3(cellCenterLocal.x - (grid.cellSize.x / 2), cellCenterLocal.y, cellCenterLocal.z)).x - ballCorners[i].x;
                                                a = Mathf.Abs(a);
                                                transform.position = new Vector3(transform.position.x - (2 * a), transform.position.y, transform.position.z);

                                                xDirection = xDirection * -1;
                                                moved = 1;
                                            }
                                        }
                                        else {
                                            a = tm.LocalToWorld(new Vector3(cellCenterLocal.x - (grid.cellSize.x / 2), cellCenterLocal.y, cellCenterLocal.z)).x - ballCorners[i].x;
                                            a = Mathf.Abs(a);
                                            transform.position = new Vector3(transform.position.x - (2 * a), transform.position.y, transform.position.z);

                                            xDirection = xDirection * -1;
                                            moved = 1;
                                        }
                                    }
                                    else if (s == 3) {
                                        //ball hitet obere Seite
                                        if (globals.TopLayerCells.Exists(l => l.x == cell.x && l.y == (cell.y + 1))) {
                                            // auf der oberenseite existiert noch ein block
                                            if (i == 3) {
                                                // es ist die letzte prüfung und es kamm noch zu keinen move des balles
                                                b = tm.LocalToWorld(new Vector3(cellCenterLocal.x, cellCenterLocal.y + (grid.cellSize.y / 2), cellCenterLocal.z)).y - ballCorners[i].y;
                                                b = Mathf.Abs(b);
                                                transform.position = new Vector3(transform.position.x, transform.position.y + (2 * b), transform.position.z);

                                                yDirection = yDirection * -1;
                                                moved = 1;
                                            }
                                        }
                                        else {
                                            b = tm.LocalToWorld(new Vector3(cellCenterLocal.x, cellCenterLocal.y + (grid.cellSize.y / 2), cellCenterLocal.z)).y - ballCorners[i].y;
                                            b = Mathf.Abs(b);
                                            transform.position = new Vector3(transform.position.x, transform.position.y + (2 * b), transform.position.z);

                                            yDirection = yDirection * -1;
                                            moved = 1;
                                        }
                                    }
                                }


                                //Debug.Log("World Corner0");
                                //Debug.Log(cellCornerWorld[0]);
                                //Debug.Log("World Corner1");
                                //Debug.Log(cellCornerWorld[1]);
                                //Debug.Log("World Corner2");
                                //Debug.Log(cellCornerWorld[2]);
                                //Debug.Log("World Corner3");
                                //Debug.Log(cellCornerWorld[3]);

                                globals.map_tiles = globals.map_tiles - 1;
                                globals.TopLayerCells.Remove(cell2);


                                if (globals.LowerLayerCells.Exists(l => l.x == cell.x && l.y == cell.y)) {
                                    int first = 0;
                                    Vector3Int topLayerCell = new Vector3Int(0, 0, 0);
                                    foreach (Vector3Int topCell in globals.LowerLayerCells.Where(l => l.x == cell.x && l.y == cell.y)) {
                                        if (first == 0) {
                                            first = 1;
                                            topLayerCell = topCell;

                                        }
                                        else if (topCell.z >= topLayerCell.z) {
                                            topLayerCell = topCell;
                                        }
                                    }
                                    globals.LowerLayerCells.Remove(topLayerCell);
                                    globals.TopLayerCells.Add(topLayerCell);
                                }

                            }
                            else {
                                break;

                            }
                        }
                        else {
                            break;
                        }
                    } while (pierce == 1);

                    i = i + 1;
                }

                if (pierce == 1) {
                    if (animation == null) {
                        animation = Instantiate(animationPrefab, transform);
                        pierce_ball_Animation_pos();
                    }
                    else {
                        pierce_ball_Animation_pos();
                    }


                }
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        //Debug.Log("enter");

        // block wenn alert screen pops up
        if (globals.AlertScreen == true) {
            return;
        }

        //if (extraBall == 1) {
        //   // Debug.Log("extra ball hit");
        //    //Debug.Log(collision.gameObject);
        //}


        if (collision.gameObject == padle && waitTime <= 0) {

            //Debug.Log("Contact Point");


            //Debug.Log("collision");

            hit_audio.Play();


            yDirection = 1;

            float splitAngle;
            float angleDif;

            splitAngle = padle.GetComponent<move_padle>().angle + 90;
            //   Debug.Log("split angle");
            //  Debug.Log(splitAngle);



            angleDif = angel - splitAngle;

            //  Debug.Log("angle Dif");
            // Debug.Log(angleDif);



            angel = splitAngle - angleDif;



            if (angel < 0) {
                angel = 0;
            }
            else if (angel > 180) {
                angel = 180;
            }

            if (angel > 90) {

                xDirection = -1;
            }
            else {
                xDirection = 1;
            }

        }

        //Tilemap tilemap = collision.gameObject.GetComponent<Tilemap>();
        //if (tilemap != null && waitTime <= 0) {
        //    collist.Add(collision);   
        //}


        //if(col == 0) {

        //    yDirection = yDirection * -1;
        //    col = 1;
        //    Tilemap tilemap= collision.gameObject.GetComponent<Tilemap>();

        //    Vector3 hitPosition = Vector3.zero;
        //    foreach (ContactPoint2D hit in collision.contacts) {

        //        hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
        //        hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
        //        Debug.Log(hitPosition);
        //        tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
        //    }
        //}
    }

    //private void OnCollisionStay2D(Collision2D collision) {

    //    // block wenn alert screen pops up
    //    if (globals.AlertScreen == true) {
    //        return;
    //    }

    //    Tilemap tilemap = collision.gameObject.GetComponent<Tilemap>();
    //    if (tilemap != null && waitTime <= 0) {
    //        collist.Add(collision);
    //    }
    //}

    private void pierce_ball_Animation_pos() {





        float newangel = 0;
        float top = 0;
        float bot = 0;
        float left = 0;
        float right = 0;


        if (xDirection == 1 && yDirection == -1) {
            // 360/0 - 270
            newangel = angel + 180;
        }
        else if (xDirection == 1 && yDirection == 1) {
            // 360/0 - 90
            newangel = angel;
        }
        else if (xDirection == -1 && yDirection == 1) {
            // 90 - 180
            newangel = angel;
        }
        else if (xDirection == -1 && yDirection == -1) {
            //180 -  270
            newangel = angel + 180;
        }

        // 0 - 45
        if (newangel <= 45) {
            top = (0.65f / 45) * newangel;
            bot = (-1) * (0.65f / 45) * newangel;
            left = -0.65f;
            right = 0.65f;
        }


        // 45 - 90
        else if (newangel <= 90) {
            left = (-1) * (0.65f / 45) * (90 - newangel);
            right = (0.65f / 45) * (90 - newangel);
            top = 0.65f;
            bot = -0.65f;
        }


        // 90 - 135
        else if (newangel <= 135) {
            left = (0.65f / 45) * (newangel - 90);
            right = (-1) * (0.65f / 45) * (newangel - 90);
            top = 0.65f;
            bot = -0.65f;
        }



        // 135 - 180
        else if (newangel <= 180) {
            top = (0.65f / 45) * (180 - newangel);
            bot = (-1) * (0.65f / 45) * (180 - newangel);
            left = 0.65f;
            right = -0.65f;
        }


        // 180 - 225
        else if (newangel <= 225) {
            top = (-1) * (0.65f / 45) * (newangel - 180);
            bot = (0.65f / 45) * (newangel - 180);
            left = 0.65f;
            right = -0.65f;
        }


        // 225 - 270
        else if (newangel <= 270) {
            left = (0.65f / 45) * (270 - newangel);
            right = (-1) * (0.65f / 45) * (270 - newangel);
            top = -0.65f;
            bot = 0.65f;
        }


        // 270 - 315
        else if (newangel <= 315) {
            left = (-1) * (0.65f / 45) * (newangel - 270);
            right = (0.65f / 45) * (newangel - 270);
            top = -0.65f;
            bot = 0.65f;
        }


        // 315 - 360/0
        else if (newangel <= 360) {
            top = (-1) * (0.65f / 45) * (360 - newangel);
            bot = (0.65f / 45) * (360 - newangel);
            left = -0.65f;
            right = 0.65f;
        }

        animation.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, newangel);
        animation.GetComponent<RectTransform>().SetLeft(left);
        animation.GetComponent<RectTransform>().SetRight(right);
        animation.GetComponent<RectTransform>().SetTop(top);
        animation.GetComponent<RectTransform>().SetBottom(bot);

        // top -0.65  bot 0.65 left -0.65 right 0.65 315 - 360/0 - 45 top 0.65 bot -0.65 left -0.65 right 0.65

        // top 0.65 bot -0.65 left -0.65 right 0.65  45 - 135 top 0.65 bot -0.65 left 0.65 right -0.65

        // top 0.65 bot -0.65 left 0.65 right -0.65 135 - 225 top -0.65 bot 0.65 left 0.65 right -0.65

        // top 0.65 bot -0.65 left 0.65 right -0.65 225 - 315 top -0.65 bot 0.65 left -0.65 right 0.65
    }

}

