using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tileplace
{
    [SerializeField] private List<Vector3> tiles;
    [SerializeField] private string filename;
    [SerializeField] private int layer;

    public Tileplace(string filename, int layer) {
        this.filename = filename;
        this.layer = layer;
        tiles = new List<Vector3>();
    }

    public string Filename { get => filename; set => filename = value; }

    public int Layer { get => layer; set => layer = value; }

    public void addTiles(Vector3 tile) { 
        tiles.Add(tile);
    }
    public void setTiles(List<Vector3> copyList) {
        tiles = new List<Vector3>(copyList);
    }
    public void removeTiles(int index) {
        try {
            tiles.RemoveAt(index);
        }
        catch (Exception e) {

            Debug.Log(e.ToString());
        }
        
    }
    public List<Vector3> getTiles() {
        return tiles;

    }
}
