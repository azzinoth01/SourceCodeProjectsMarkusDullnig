using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Map
{
    [SerializeField] private int xTiles;
    [SerializeField] private int yTiles;
    [SerializeField] private List<Tileplace> tiles;

    public Map(int xTiles, int yTiles) {
        this.xTiles = xTiles;
        this.yTiles = yTiles;
        tiles = new List<Tileplace>();
    }

    public int YTiles { get => yTiles; set => yTiles = value; }
    public int XTiles { get => xTiles; set => xTiles = value; }

    public void addTiles(Tileplace tile) {
        tiles.Add(tile);
    }
    public void setTiles(List<Tileplace> copyList) {
        tiles = new List<Tileplace>(copyList);
    }
    public void removeTiles(int index) {
        try {
            tiles.RemoveAt(index);
        }
        catch (Exception e) {

           // Debug.Log(e.ToString());
        }

    }
    public List<Tileplace> getTiles() {
        return tiles;
    }
}
