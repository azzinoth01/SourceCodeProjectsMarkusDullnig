using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
/// class that saves the item Catalog and loads it
/// </summary>
[Serializable]
public class ItemCatalog
{
    [SerializeReference] private List<Item> itemList;


    /// <summary>
    /// list of item
    /// </summary>
    public List<Item> ItemList {
        get {
            return itemList;
        }

        set {
            itemList = value;
        }
    }

    /// <summary>
    /// standard constructor
    /// </summary>
    public ItemCatalog() {
        itemList = new List<Item>();
    }


    /// <summary>
    /// saves the itemCatalog
    /// </summary>
    public void savingSetting() {

        string json = JsonUtility.ToJson(this);
        using (FileStream file = File.Create("Assets/Catalog/itemCatalog.json")) {
            using (StreamWriter writer = new StreamWriter(file)) {
                writer.Write(json);

            }
        }

    }

    /// <summary>
    /// loads the data from the itemCatalog
    /// </summary>
    /// <returns> retruns the saved itemCatalog</returns>
    public static ItemCatalog loadSettings() {

        ItemCatalog s = new ItemCatalog();
        //Debug.Log("still loading");
        LoadAssets load = new LoadAssets();
        TextAsset text = load.loadText("Assets/Catalog/itemCatalog.json");

        // Debug.LogError("text loaded");

        //Debug.Log("still loading");

        // Debug.LogError(text.text);

        if (text != null) {
            //string json = File.ReadAllText("Assets/Catalog/itemCatalog.json");

            string json = text.text;


            //Debug.Log(json);
            if (json == null || json == "") {
                //  Debug.LogError("text empty");
                return null;
            }
            // Debug.LogError("liste for dem deserialiezen");

            s = JsonUtility.FromJson<ItemCatalog>(json);
            load.releaseAllHandle();
            // Debug.LogError("liste deseriallised");
            return s;
        }

        // Debug.LogError("text leer");
        return null;

    }
}
