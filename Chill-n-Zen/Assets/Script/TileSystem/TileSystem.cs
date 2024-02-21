using UnityEngine;

public class TileSystem : MonoBehaviour
{
    [SerializeField] Grid _isoGrid;

    private void OnValidate()
    {
        if (_isoGrid != null)
        {
            if (_isoGrid.cellSize.x <= 0 || _isoGrid.cellSize.y <= 0)
            {
                Debug.LogError(" (error : 2x1) Grid's tile size is null or below 0 ");
            }
        }
        else Debug.LogError(" (error : 2x0) There is no isometric grid ", _isoGrid);
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

    public Vector2 GridToWorld(Vector2Int position)
    {
        Vector3 worldPos = _isoGrid.CellToWorld(new Vector3Int(position.x, position.y, 0));
        return new Vector2(worldPos.x, worldPos.y);
    }
}
