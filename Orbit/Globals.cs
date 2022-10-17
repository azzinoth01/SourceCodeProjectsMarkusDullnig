using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// static class to manage global variables
/// </summary>
public static class Globals
{
    /// <summary>
    /// global variable to set pause
    /// </summary>
    public static bool pause;
    /// <summary>
    /// current main camera
    /// </summary>
    public static Camera currentCamera;
    /// <summary>
    /// player object
    /// </summary>
    public static GameObject player;
    /// <summary>
    /// bulletpool list
    /// </summary>
    public static List<Skill> bulletPool;
    /// <summary>
    /// globale variable to set menuHandler
    /// </summary>
    public static Menu_handler menuHandler;
    /// <summary>
    /// current wincondition
    /// </summary>
    public static Win_condition currentWinCondition;
    /// <summary>
    /// spawner list
    /// </summary>
    public static List<Enemy_Spawner> spawnerListe;
    /// <summary>
    /// boss UI
    /// </summary>
    public static GameObject bossUI;
    /// <summary>
    /// boss hp bar
    /// </summary>
    public static Image bossHpBar;
    /// <summary>
    /// virtual mouse object
    /// </summary>
    public static VirtualMouse virtualMouse;
    /// <summary>
    /// current possesed money
    /// </summary>
    public static int money;
    /// <summary>
    /// list of inventory objects
    /// </summary>
    public static List<Inventar_Object> inventory;
    /// <summary>
    /// money icon
    /// </summary>
    public static Sprite moneyIcon;
    /// <summary>
    /// money drop prefab
    /// </summary>
    public static GameObject moneyDrop;
    /// <summary>
    /// item catalog
    /// </summary>
    public static ItemCatalog catalog;
    /// <summary>
    /// curent item which the cursor holds
    /// </summary>
    public static Item currentItem;
    /// <summary>
    /// enemy hit sound
    /// </summary>
    public static AudioSource tempEnemyHit;
    /// <summary>
    /// list of objects which are in the don't destroy on load list
    /// </summary>
    public static List<string> dontDestoryOnLoadObjectID;
    /// <summary>
    /// last scene index
    /// </summary>
    public static int lastSceneIndex;
    /// <summary>
    /// list of spawner for the endless mode
    /// </summary>
    public static List<Enemy_Spawner> infityWaveSpawner;
    /// <summary>
    /// wave controler for the endless mode
    /// </summary>
    public static WaveControler waveControler;
    /// <summary>
    /// tooltips actiated or deactivated
    /// </summary>
    public static ToolTip tooltip;
    /// <summary>
    /// skips the intro cutscene
    /// </summary>
    public static bool skipStartCutscene;

}
