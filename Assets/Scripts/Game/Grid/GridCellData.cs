using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridCellData
{
    public Vector3 coordinate;
    public bool isEmpty;

    public GridCellData() { }
    public GridCellData(Vector3 coord, bool state)
    {
        coordinate = coord;
        isEmpty = state;
    }
}