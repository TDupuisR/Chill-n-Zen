using UnityEngine;
using NaughtyAttributes;

public class TileSystem : MonoBehaviour
{
    [SerializeField] Grid _isoGrid;
    [SerializeField] GameObject _prefabTile;
    [SerializeField] Transform _floorParent;

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

    private void Start()
    {
        GenerateGrid(TESTStartPos, TESTGridSize);
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

    public void GenerateGrid(Vector2Int StartPosGrid, Vector2Int GridSize)
    {
        if (GridSize.x == 0 || GridSize.y == 0)
        {
            Debug.LogError(" (error : 2x2) Grid's Spawn Size is null, x or y = 0");
        }
        else
        {
            for (int x = 0; x < GridSize.x; x++)
            {
                for (int y = 0; y < GridSize.y; y++)
                {
                    GameObject newTile = Instantiate(_prefabTile, _floorParent);

                    Vector2 pos = GridToWorld(x, y);
                    newTile.transform.position = pos;

                    newTile.transform.name = "Tile " + x + " | " + y;
                }
            }
        }
    }

    [Button]
    public void ShowGrid()
    {
        OnShowGrid.Invoke();
    }        
}
