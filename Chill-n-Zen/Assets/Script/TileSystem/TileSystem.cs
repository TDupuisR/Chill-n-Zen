using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using System;

public class TileSystem : MonoBehaviour
{
    [SerializeField] Grid _isoGrid;
    [SerializeField] GameObject _prefabTile;
    [SerializeField] Transform _floorParent;

    List<GameObject> _gridTilesList = new List<GameObject>();

    [SerializeField] Vector2Int TESTStartPos;
    [SerializeField] Vector2Int TESTGridSize;

    public delegate void OnShowGridDelegate();
    public static event OnShowGridDelegate OnShowGrid;

    private void OnValidate()
    {
        if (_isoGrid != null)
        {
            if (_isoGrid.cellSize.x <= 0 || _isoGrid.cellSize.y <= 0)
            {
                Debug.LogError(" (error : 2x1) Grid's tile size is null or below 0 ");
            }
        }
        else Debug.LogError(" (error : 2x0) There is no isometric grid assigned ", _isoGrid);
    }

    private void Awake()
    {
        _isoGrid = GetComponent<Grid>();
    }

    public Vector2 WorldToGrid(Vector2 position)
    {
        Vector3Int gridPos = _isoGrid.WorldToCell(position);
        return new Vector2Int(gridPos.x, gridPos.y);
    }
    public Vector2 GridToWorld(int x, int y)
    {
        Vector3 worldPos = _isoGrid.CellToWorld(new Vector3Int(x, y, 0));
        return new Vector2(worldPos.x, worldPos.y);
    }

    private void GetGridTilesList()
    {
        _gridTilesList.Clear();

        foreach (Transform child in _floorParent)
        {
            Debug.Log(child.name);
            _gridTilesList.Add(child.gameObject);
        }

        Debug.Log(_gridTilesList.Count);
    }

    public void GenerateGrid(Vector2Int startPosGrid, Vector2Int gridSize)
    {
        if (gridSize.x == 0 || gridSize.y == 0)
        {
            Debug.LogError(" (error : 2x2) Grid's Spawn Size is null, x or y = 0 ");
        }
        else
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    if (CheckTileExist(startPosGrid.x + x, startPosGrid.y + y) <= -1)
                    {
                        GameObject newTile = Instantiate(_prefabTile, _floorParent);

                        Vector2 pos = GridToWorld(startPosGrid.x + x, startPosGrid.y + y);
                        newTile.transform.position = pos;

                        newTile.transform.name = (startPosGrid.x + x) + "|" + (startPosGrid.y + y);
                    }
                }
            }

            GetGridTilesList();
        }
    }
    public void DeleteGrid(Vector2Int startPosGrid, Vector2Int gridSize)
    {
        if (gridSize.x == 0 || gridSize.y == 0)
        {
            Debug.LogWarning(" (error : 2x3) Grid's Delete Size is null, x or y = 0 ");
        }
        else
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    int index = CheckTileExist(startPosGrid.x + x, startPosGrid.y + y);
                    if (index > -1)
                    {
                        _gridTilesList[index].transform.SetParent(null);
                        Destroy(_gridTilesList[index]);
                    }
                }
            }

            GetGridTilesList();
        }
    }
    public void DeleteGrid()
    {
        for (int i = 0; i < _gridTilesList.Count; i++)
        {
            _gridTilesList[i].transform.SetParent(null);
            Destroy(_gridTilesList[i]);
        }

        GetGridTilesList();
    }


    public int CheckTileExist(int x, int y) // Return position in TileList, or -1 if null //
    {
        int res = -1;

        for (int i = 0; i < _gridTilesList.Count; i++)
        {
            if (_gridTilesList[i].name == x + "|" + y)
            {
                res = i; break;
            }
        }

        return res;
    }

    [Button]
    public void ShowGrid()
    {
        if (_gridTilesList.Count != 0) OnShowGrid.Invoke();
    }

    // TEST DEBUG METHOD //
    [Button]
    private void TESTGenerateGrid()
    {
        GenerateGrid(TESTStartPos, TESTGridSize);
    }
    [Button]
    private void TESTDeleteGrid()
    {
        DeleteGrid(TESTStartPos, TESTGridSize);
    }
    [Button]
    private void TESTDeleteAllGrid()
    {
        DeleteGrid();
    }
}
