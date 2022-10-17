using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edit_tile
{
    private GameObject tile;
    private Vector3 position;
    private int layer;
    private string filename;

    public Edit_tile(GameObject tile, Vector3 position, int layer, string filename) {
        this.tile = tile;
        this.position = position;
        this.layer = layer;
        this.filename = filename;
    }

    public GameObject Tile { get => tile; set => tile = value; }
    public Vector3 Position { get => position; set => position = value; }
    public int Layer { get => layer; set => layer = value; }
    public string Filename { get => filename; set => filename = value; }
}
