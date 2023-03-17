using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;

    private List<Vector3> _emptyCells;
    private Vector3 _tileSize;
    private List<Vector3> _cellsToRemove;
    private bool _gridComplete;

    public List<Vector3> EmptyCells => _emptyCells;
    public Vector3 TileSize => _tileSize;
    public List<Vector3> CellsToRemove => _cellsToRemove;
    public bool GridComplete => _gridComplete;

    void Start()
    {
        _emptyCells = new List<Vector3>();
        _tileSize = _tilemap.size;

        GetEmptyCells();
        RemoveLeftCorner();
    }

    private void GetEmptyCells()
    {
        List<Vector3> emptyCells = new List<Vector3>();

        BoundsInt bounds = _tilemap.cellBounds;
        TileBase[] allTiles = _tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile == null)
                {
                    emptyCells.Add(new Vector3(x, y, 0f));
                }
            }
        }
        _emptyCells = emptyCells;
        _gridComplete = true;
    }
    private void RemoveLeftCorner()
    {
        Vector3 firstEmptyCell = _emptyCells.First();

        _cellsToRemove = new List<Vector3>{
            new Vector3(firstEmptyCell.x, firstEmptyCell.y, 0f),
            new Vector3(firstEmptyCell.x, firstEmptyCell.y + 1f, 0f),
            new Vector3(firstEmptyCell.x + 1f, firstEmptyCell.y, 0f),
            new Vector3(firstEmptyCell.x + 1f ,firstEmptyCell.y + 1f, 0f)
        };

        for (int i = 0; i < _emptyCells.Count; i++)
        {
            for (int toRemove = 0; toRemove < _cellsToRemove.Count; toRemove++)
            {
                if (_emptyCells[i].Equals(_cellsToRemove[toRemove]))
                {
                    _emptyCells.Remove(_emptyCells[i]);
                }
            }
        }
    }
}