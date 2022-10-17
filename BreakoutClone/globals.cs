using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class globals
{
    public static int lives;
    public static int map_tiles;
    public static int start;
    public static List<GameObject> ballList;
    public static List<GameObject> powerupGameObjectList;
    public static GameObject padle;
    public static RectTransform mainPanel;
    public static Grid mainGrid;
    public static List<string> powerupList;
    public static int arcadeLevel;
    public static int arcadeStart;
    public static string customLevelPath;
    public static int customLevel;
    public static bool AlertScreen;
    public static string afterGameScreenText;
    public static Vector3 levelScale;
    public static List<Vector3Int> TopLayerCells;
    public static List<Vector3Int> LowerLayerCells;
    public static List<Tilemap> tilemapList;
    public static bool backgroundMusic;
    


}
