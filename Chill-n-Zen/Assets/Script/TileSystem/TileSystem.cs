using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using GameManagerSpace;

public class TileSystem : MonoBehaviour
{
    public static TileSystem Instance;

    [Header("Setup")]
    [SerializeField] Grid _isoGrid;
    [SerializeField] WallBehavior _wallBehavior;
    [SerializeField] GameObject _prefabTile;
    [SerializeField] Transform _floorParent;
    [SerializeField] Transform _objectParent;
    [SerializeField] GameObject _door;

    List<GameObject> _tilesList = new List<GameObject>();
    List<TileBehaviour> _tileBehaveList = new List<TileBehaviour>();

    List<GameObject> _objectList = new List<GameObject>();
    List<ItemBehaviour> _itemBehaveList = new List<ItemBehaviour>();

    List<Vector2Int> _nextPF = new List<Vector2Int>();
    List<Vector2Int> _donePF = new List<Vector2Int>();

    ItemBehaviour _doorBehave;

    enum objAction { Add, Remove }
    public Vector3 CellSize { get { return _isoGrid.cellSize; }  }
    public bool IsSceneVacant { get; private set; }
    public List<TileBehaviour> TilesList { get { return _tileBehaveList; } }
    public List<ItemBehaviour> ItemList { get { return _itemBehaveList; } }
    public int TotalScore { get; private set; }
    public float TopItemSize { get; set; }

    public delegate void OnShowGridDelegate();
    public static event OnShowGridDelegate OnShowGrid;
    public delegate void OnShowGridSpecifiedDelegate(bool state);
    public static event OnShowGridSpecifiedDelegate OnShowGridSpecified;

    public delegate void OnItemAddedDelegate(Item item);
    public static event OnItemAddedDelegate OnItemAdded;
    public delegate void OnItemRemovedDelegate(Item item);
    public static event OnItemRemovedDelegate OnItemRemoved;

    public delegate void OnSceneChangedDelegate();
    public static event OnSceneChangedDelegate OnSceneChanged;
    public delegate void OnScoreChangedDelegate(int score);
    public static event OnScoreChangedDelegate OnScoreChanged;

    private void OnValidate()
    {
        if (_isoGrid != null)
        {
            if (CellSize.x <= 0 || CellSize.y <= 0)
            {
                Debug.LogError(" (error : 2x2) Grid's tile size is null or below 0 ");
            }
        }
        else Debug.LogError(" (error : 2x1) No isometric grid assigned ", _isoGrid.gameObject);

        if (_floorParent == null) Debug.LogError(" (error : 2x5a) No Floor Parent assigned ", _floorParent.gameObject);
        if (_objectParent == null) Debug.LogError(" (error : 2x5b) No Object Parent assigned ", _objectParent.gameObject);
        if (_door == null) Debug.LogError(" (error : 2x5c) No Object Door assigned ", _door);
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

        IsSceneVacant = true;
        _doorBehave = _door.GetComponent<ItemBehaviour>();

        TotalScore = 0;
        InitializeDoor();
    }

    private void OnEnable()
    {
        OnSceneChanged += RoomScanning;
    }
    private void OnDisable()
    {
        OnSceneChanged -= RoomScanning;
    }

    public Vector2Int WorldToGrid(Vector2 position)
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
        _tileBehaveList.Clear();

        foreach (Transform child in _floorParent)
        {
            _tilesList.Add(child.gameObject);
            _tileBehaveList.Add(child.GetComponent<TileBehaviour>());
        }
        _wallBehavior.InstantiateWall();
    }
    public int CheckTileExist(int x, int y) // Return index in TileList, or -1 if null //
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
            if (_tilesList.Count > 0) OnShowGridSpecified.Invoke(false);

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    if (CheckTileExist(startPosGrid.x + x, startPosGrid.y + y) <= -1)
                    {
                        GameObject newTile = Instantiate(_prefabTile, _floorParent);

                        Vector2 pos = GridToWorld(startPosGrid.x + x, startPosGrid.y + y);
                        newTile.transform.position = new Vector3(pos.x, pos.y, _floorParent.position.z + pos.y);

                        newTile.transform.name = (startPosGrid.x + x) + "|" + (startPosGrid.y + y);
                    }
                }
            }
            GetGridTilesList();

            OnShowGridSpecified.Invoke(true);
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
            OnShowGridSpecified.Invoke(true);
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

    private void InitializeDoor()
    {
        _door.transform.position = Vector2.zero;

        _doorBehave.Initialize(_doorBehave.OwnItem);
        IsSceneVacant = true;
        _doorBehave.CurrentState = GMStatic.State.Waiting;
    }
    public void PlaceDoor(Vector2Int gridPos)
    {
        _door.transform.position = GridToWorld(gridPos.x, gridPos.y);
        _doorBehave.Place();
    }
    public void MoveDoor(Vector2Int gridPos)
    {
        _doorBehave.Move();
        PlaceDoor(gridPos);
    }
    public void RotateDoor(int rotation)
    {
        _doorBehave.Rotation(rotation);
    }
    public void ColorDoor(Color color)
    {
        _doorBehave.ChangeSpriteColor(color);
    }

    public void ShowGrid()
    {
        OnShowGrid?.Invoke();
    }

    public bool CheckForPlacing(ItemBehaviour item, int x, int y)
    {
        bool res = false;

        for (int i = 0; i < item.RotationSize.x; i++)
        {
            for (int j = 0; j < item.RotationSize.y; j++)
            {
                int index = CheckTileExist(x + i, y + j);

                if (index > -1)
                {
                    res = _tileBehaveList[index].CheckIfPlacable(item.OwnItem);
                }
                else res = false;

                if (!res) break;
            }

            if (!res) break;
        }

        return res;
    }
    public float CheckItemTop(ItemBehaviour item, int x, int y) //if return is -1 consider false
    {
        float res = -1;
        TopItemSize = 0;

        for (int i = 0; i < item.RotationSize.x; i++)
        {
            for (int j = 0; j < item.RotationSize.y; j++)
            {
                int tempoRes = 0;
                int index = CheckTileExist(x + i, y + j);
                if (index > -1)
                {
                    tempoRes = _tileBehaveList[index].CheckIfTop(item.OwnItem);
                }

                if (tempoRes > 4 || (tempoRes != res && res >= 0))
                {
                    res = -1;
                    break;
                }
                else
                    res = tempoRes;
            }
            if (res < 0) break;
        }

        if (res > 0) res = TopItemSize;
        return res;
    }
    public bool CheckForAccessing(int x, int y, GMStatic.constraint constr = GMStatic.constraint.None)
    {
        bool res = false;

        int index = CheckTileExist(x, y);

        if (index > -1)
        {
            res = _tileBehaveList[index].CheckIfAccessible(constr);
        }

        return res;
    }
    public bool CheckForBonus(Item item, int x, int y)
    {
        bool res = false;

        int index = CheckTileExist(x, y);
        if (index > -1)
        {
            res = _tileBehaveList[index].CheckConditionExist(item);
        }

        return res;
    }
    public bool CheckForBonus(GMStatic.tagUsage usage, int x, int y)
    {
        bool res = false;

        int index = CheckTileExist(x, y);
        if (index > -1)
        {
            res = _tileBehaveList[index].CheckConditionExist(usage);
        }

        return res;
    }

    // Object List Gestion //
    public void ObjectOnScene(bool status)
    {
        IsSceneVacant = status;
    }
    private bool CheckIfObjectExist(GameObject obj)
    {
        bool res = false;
        foreach (GameObject current in _objectList)
        {
            if (current == obj)
            {
                res = true; break;
            }
        }

        return res;
    }
    private void ChangeObjectList(GameObject obj, ItemBehaviour item, objAction state)
    {
        switch (state)
        {
            case objAction.Add:
                {
                    if (!CheckIfObjectExist (obj))
                    {
                        _objectList.Add(obj);
                        _itemBehaveList.Add(item);
                        OnItemAdded?.Invoke(item.OwnItem);
                    }
                    break;
                }
            case objAction.Remove:
                {
                    if (CheckIfObjectExist(obj))
                    {
                        _objectList.Remove(obj);
                        _itemBehaveList.Remove(item);
                        OnItemRemoved?.Invoke(item.OwnItem);
                    }
                    break;
                }
            default: break;
        }
    }

    public void PlacingItem(GameObject obj, int x, int y)
    {
        ItemBehaviour behave = obj.GetComponent<ItemBehaviour>();
        ChangeObjectList(obj, behave, objAction.Add);

        for (int i = 0; i < behave.RotationSize.x; i++)
        {
            for (int j = 0; j < behave.RotationSize.y; j++)
            {
                int index = CheckTileExist(x + i, y + j);
                if (index > -1)
                {
                    _tileBehaveList[index].PlaceItem(behave.OwnItem);
                    OnSceneChanged?.Invoke();
                }
            }
        }

        SpriteLayersOrdering();
    }
    public void RemoveItem(GameObject obj, int x, int y)
    {
        ItemBehaviour behave = obj.GetComponent<ItemBehaviour>();
        ChangeObjectList(obj, behave, objAction.Remove);

        for (int i = 0; i < behave.RotationSize.x; i++)
        {
            for (int j = 0; j < behave.RotationSize.y; j++)
            {
                int index = CheckTileExist(x + i, y + j);
                if (index > -1)
                {
                    _tileBehaveList[index].RemoveItem(behave.OwnItem);
                    OnSceneChanged?.Invoke();
                }
            }
        }

        Destroy(obj);
    }
    public void MoveItem(GameObject obj, int x, int y)
    {
        ItemBehaviour behave = obj.GetComponent<ItemBehaviour>();

        for (int i = 0; i < behave.RotationSize.x; i++)
        {
            for (int j = 0; j < behave.RotationSize.y; j++)
            {
                int index = CheckTileExist(x + i, y + j);
                if (index > -1)
                {
                    _tileBehaveList[index].RemoveItem(behave.OwnItem);
                }
            }
        }

    }

    private void RoomScanning()
    {
        int freeTiles = 0;
        foreach (TileBehaviour tile in _tileBehaveList)
        {
            if (tile.CheckIfAccessible(GMStatic.constraint.None))
                freeTiles++;
        }
        GameManager.requestManager.FreeTiles = freeTiles;

        if (GameManager.budgetManager.CurrentBudget > 0)
        {
            int score = 0;
            foreach (ItemBehaviour furnit in _itemBehaveList)
            {
                if (furnit.gameObject != _door)
                {
                    score += furnit.PointsChecker.GivePoints();
                }
            }
            TotalScore = score;
        }

        OnScoreChanged?.Invoke(TotalScore);
    }
    private void SpriteLayersOrdering()
    {
        foreach (ItemBehaviour item in _itemBehaveList)
            item.SpriteLayer = 0;

        foreach (ItemBehaviour item in _itemBehaveList)
        {
            Vector2[] pos = item.GetLayerPoints();

            Vector2 segment = pos[1] - pos[0];
            Vector2 normal = new Vector2(-segment.y, segment.x);
            Vector2 middle = pos[0] + (segment / 2f);

            foreach (ItemBehaviour compare in _itemBehaveList)
            {
                if (compare != item && 
                    !(compare.OwnItem.type == GMStatic.tagType.Carpet) &&
                    !(compare.OwnItem.type == GMStatic.tagType.Ceiling))
                {
                    Vector2 priority;
                    Vector2[] posT = compare.GetLayerPoints();
                    Vector2 middleT = posT[0] + ((posT[1] - posT[0]) / 2f);

                    if (middleT.x >= middle.x)
                        priority = posT[1];
                    else
                        priority = posT[0];

                    float dot = Vector2.Dot(normal, priority - pos[0]);
                    if (dot > 0)
                        compare.SpriteLayer -= 1;
                    if (dot == 0 && compare.OwnItem.type == GMStatic.tagType.Mural)
                        compare.SpriteLayer -= 1;
                    if (dot == 0 && compare.OwnItem.listUsage.Contains(GMStatic.tagUsage.Top) &&
                        item.OwnItem.type == GMStatic.tagType.Object)
                        compare.SpriteLayer -= 1;
                }
                else if (compare.OwnItem.type == GMStatic.tagType.Carpet)
                {
                    compare.SpriteLayer -= 1;
                }
                else if (compare.OwnItem.type == GMStatic.tagType.Ceiling)
                {
                    compare.SpriteLayer += 1;
                }
            }
        }

        foreach (ItemBehaviour item in _itemBehaveList)
            item.ApplyLayer();

    }

    // Path Finding //
    public bool PathFinding(Vector2Int start, Vector2Int target) 
    {
        bool res = false;

        _nextPF.Clear();
        _donePF.Clear();

        Vector2Int potential = start;
        _nextPF.Add(potential);

        do
        {
            float distance = Mathf.Infinity;
            foreach (Vector2Int next in _nextPF)
            {
                if (Vector2.Distance(next, target) < distance)
                {
                    potential = next;
                    distance = Vector2.Distance(potential, target);
                }
            }

            _nextPF.Remove(potential);
            _donePF.Add(potential);

            for (int i = 0; i < 4; i++)
            {
                Vector2Int decal = Vector2Int.zero;
                if (i == 0) decal = new Vector2Int(1, 0);
                else if (i == 1) decal = new Vector2Int(0, 1);
                else if (i == 2) decal = new Vector2Int(-1, 0);
                else if (i == 3) decal = new Vector2Int(0, -1);

                if (CheckForAccessing(potential.x + decal.x, potential.y + decal.y) && !_donePF.Contains(potential + decal) && !_nextPF.Contains(potential + decal))
                {
                    _nextPF.Add(potential + decal);
                }
            }

            if (potential == target) res = true;

        } while (_nextPF.Count > 0 && res == false);

        return res;
    }
    public bool PathFinding(Vector2Int start) // Target door by default //
    {
        bool res = false;
        Vector2Int target = WorldToGrid(_door.transform.position);

        _nextPF.Clear();
        _donePF.Clear();

        Vector2Int potential = start;
        _nextPF.Add(potential);

        do
        {
            float distance = Mathf.Infinity;
            foreach (Vector2Int next in _nextPF)
            {
                if (Vector2.Distance(next, target) < distance)
                {
                    potential = next;
                    distance = Vector2.Distance(potential, target);
                }
            }

            _nextPF.Remove(potential);
            _donePF.Add(potential);

            for (int i = 0; i < 4; i++)
            {
                Vector2Int decal = Vector2Int.zero;
                if (i == 0) decal = new Vector2Int(1, 0);
                else if (i == 1) decal = new Vector2Int(0, 1);
                else if (i == 2) decal = new Vector2Int(-1, 0);
                else if (i == 3) decal = new Vector2Int(0, -1);

                if (CheckForAccessing(potential.x + decal.x, potential.y + decal.y) && !_donePF.Contains(potential + decal) && !_nextPF.Contains(potential + decal))
                {
                    _nextPF.Add(potential + decal);
                }
            }

            if (potential == target) res = true;

        } while (_nextPF.Count > 0 && res == false);

        return res;
    }

    #region // TEST DEBUG METHODS //
    [Button] private void TESTDeleteAllGrid()
    {
        DeleteGrid();
    }
    #endregion
}
