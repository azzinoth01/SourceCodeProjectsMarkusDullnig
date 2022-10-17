using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cellmap 
{
    private Rect rect;
    private Vector3Int cellPos;

    public Cellmap(Rect rect, Vector3Int cellPos) {
        this.rect = rect;
        this.cellPos = cellPos;
    }

    public Rect Rect { get => rect; set => rect = value; }
    public Vector3Int CellPos { get => cellPos; set => cellPos = value; }
}
