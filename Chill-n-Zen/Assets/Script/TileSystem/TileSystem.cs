using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

public class TileSystem : MonoBehaviour
{
    public static TileSystem Instance;

    [Header("Setup")]
    [SerializeField] Grid _isoGrid;
    [SerializeField] GameObject _prefabTile;
    [SerializeField] Transform _floorParent;

    List<GameObject> _tilesList = new List<GameObject>();

    public Vector3 CellSize { get { return _isoGrid.cellSize; }  }

    [Space(7)]
    [Header("TEST VARIABLES")]
    [SerializeField] Vector2Int TESTStartPos;
    [SerializeField] Vector2Int TESTGridSize;

    public delegate void OnShowGridDelegate();
    public static event OnShowGridDelegate OnShowGrid;

    private void OnValidate()
    {
        if (_isoGrid != null)
        {
            if (CellSize.x <= 0 || CellSize.y <= 0)
            {
                Debug.LogError(" (error : 2x2) Grid's tile size is null or below 0 ");
            }
        }
        else Debug.LogError(" (error : 2x1) No isometric grid assigned ", _isoGrid);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            if (Instance != null) Destroy(gameObject);
            Instance = this;
        }
        else
        {
            Debug.LogError(" (error : 2x0) Too many TileSystem instance ", gameObject);
        }

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
        _tilesList.Clear();

        foreach (Transform child in _floorParent)
        {
            _tilesList.Add(child.gameObject);
        }
    }
    private int CheckTileExist(int x, int y) // Return index in TileList, or -1 if null //
    {
        int res = -1;

        for (int i = 0; i < _tilesList.Count; i++)
        {
            if (_tilesList[i].name == x + "|" + y)
            {
                res = i; break;
            }
        }

        return res;
    }

    public void GenerateGrid(Vector2Int startPosGrid, Vector2Int gridSize)
    {
        if (gridSize.x == 0 || gridSize.y == 0)
        {
            Debug.LogError(" (error : 2x3) Grid's Spawn Size is null, x or y = 0 ");
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
            Debug.LogWarning(" (error : 2x4) Grid's Delete Size is null, x or y = 0 ");
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
                        _tilesList[index].transform.SetParent(null);
                        Destroy(_tilesList[index]);
                    }
                }
            }

            GetGridTilesList();
        }
    }
    public void DeleteGrid()
    {
        for (int i = 0; i < _tilesList.Count; i++)
        {
            _tilesList[i].transform.SetParent(null);
            Destroy(_tilesList[i]);
        }

        GetGridTilesList();
    }

    public bool CheckForPlacing(Item item, int x, int y)
    {
        bool res = false;

        int index = CheckTileExist(x, y);
        if (index > -1)
        {
            TileBehaviour script = _tilesList[index].GetComponent<TileBehaviour>();
            res = script.CheckIfAccessible(item);
        }

        return res;
    }

    [Button]
    public void ShowGrid()
    {
        if (_tilesList.Count != 0) OnShowGrid.Invoke();
    }

    #region // TEST DEBUG METHODS //
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
    #endregion
}
