using UnityEngine;
using NaughtyAttributes;
using GameManagerSpace;
using System.Collections.Generic;

public class ItemBehaviour : MonoBehaviour
{
    [Header("Serialized Infos")]
    [SerializeField] ItemConstraint _constraint;
    [SerializeField] ItemPointsChecker _pointsChecker;
    [SerializeField] GameObject _spriteUnCllrGmObj;
    SpriteRenderer _spriteUnClrRender;
    [SerializeField] GameObject _spriteCllrGmObj;
    SpriteRenderer _spriteClrRender;
    [SerializeField] ItemUI _itemUI;
    [SerializeField] LineRenderer _lineRender;
    [SerializeField] private GameObject _smoke;

    [Header("TEST ONLY")]
    [SerializeField] Item _ownItem;

    Vector3Int _rotationSize = Vector3Int.zero;
    int _orientation = 0;
    Vector3 _offsetPos = Vector3.zero;
    Vector3 _lastPos;
    bool _canPlace = false;

    Vector3[] _patternPosition = new Vector3[4];

    public Item OwnItem { get { return _ownItem; } }
    public SpriteRenderer SpriteRenderer { get { return _spriteUnClrRender; } }
    public ItemPointsChecker PointsChecker { get { return _pointsChecker; } }
    public GMStatic.State CurrentState { get; set; }

    public Vector3 OffsetPos { get { return _offsetPos; } }
    public Vector3Int RotationSize { get { return _rotationSize; } }
    public int Orientation { get { return _orientation; } }
    public Color ItemColor { get; private set; }

    public bool CanPlace { get { return _canPlace;} }
    public bool ConstraintValid { get { return _constraint.IsConstraintValid; } }
    public bool DoorValid { get { return _constraint.IsDoorValid; } }


    private void OnValidate()
    {
        if (_lineRender == null) 
            Debug.LogError(" (error : 4x0) No LineRenderer assigned ) ", gameObject);
        if (_spriteUnCllrGmObj == null || _spriteCllrGmObj == null)
            Debug.LogError(" (error : 4x1) No Sprite child GameObject assigned ) ", gameObject);
    }

    private void OnEnable()
    {
        TileSystem.OnSceneChanged += CheckWhenPlaced;

        _spriteUnClrRender = _spriteUnCllrGmObj.GetComponent<SpriteRenderer>();
        _spriteClrRender = _spriteCllrGmObj.GetComponent<SpriteRenderer>();
        InitPattern();
    }
    private void OnDisable()
    {
        TileSystem.OnSceneChanged -= CheckWhenPlaced;
    }

    private void Update()
    {

        if (CurrentState == GMStatic.State.Moving)
        {
            SetPosFromPointer();

            if (transform.position != _lastPos) CheckNewPos();
        }
    }

    public void Initialize(Item item) // Color color
    {
        if (item == null)
        {
            Debug.LogError(" (error : 4x2) No Item assigned before initialisation ", gameObject);
            Remove(); return;
        }

        _ownItem = item;
        if (_ownItem.size.x <= 0 || _ownItem.size.y <= 0 || _ownItem.size.z <= -1)
        {
            Debug.LogError(" (error : 4x3) Size of the item out of bound (null or negative values) ", gameObject);
            Remove(); return;
        }

        TileSystem.Instance.ObjectOnScene(false);

        _rotationSize = OwnItem.size;
        _orientation = 0;

        OffsetPosCalcul();
        _spriteUnCllrGmObj.transform.position = transform.position + _offsetPos;
        ResetLineRenderer(RotationSize.x, RotationSize.y);
        _lineRender.enabled = true;
        LineColor(Color.red);

        SpriteAppearance();
        //ChangeSpriteColor(color);

        //Constraint Methods
        _constraint.ResetConstraint(transform.position);

        CurrentState = GMStatic.State.Moving;
    }
    private void ResetInfos()
    {
        OffsetPosCalcul();
        _spriteUnCllrGmObj.transform.position = transform.position + _offsetPos;

        SpriteAppearance();
        CheckNewPos();
    }

    private void InitPattern()
    {
        for (int i = 0; i < 4; i++)
        {
            _patternPosition[i] = _lineRender.GetPosition(i);
        }
    }
    private void ResetLineRenderer(int sizeX, int sizeY)
    {
        for (int i = 0; i < 4; i++) _lineRender.SetPosition(i, _patternPosition[i]);

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                Vector2 worldPos = TileSystem.Instance.GridToWorld(x, y);

                if (_patternPosition[0].y + worldPos.y > _lineRender.GetPosition(0).y)
                    _lineRender.SetPosition(0, new Vector3(_patternPosition[0].x + worldPos.x, _patternPosition[0].y + worldPos.y, 0));
                if (_patternPosition[1].x + worldPos.x > _lineRender.GetPosition(1).x)
                    _lineRender.SetPosition(1, new Vector3(_patternPosition[1].x + worldPos.x, _patternPosition[1].y + worldPos.y, 0));
                if (_patternPosition[2].y + worldPos.y < _lineRender.GetPosition(2).y)
                    _lineRender.SetPosition(2, new Vector3(_patternPosition[2].x + worldPos.x, _patternPosition[2].y + worldPos.y, 0));
                if (_patternPosition[3].x + worldPos.x < _lineRender.GetPosition(3).x)
                    _lineRender.SetPosition(3, new Vector3(_patternPosition[3].x + worldPos.x, _patternPosition[3].y + worldPos.y, 0));
            }
        }

        for (int i = 0; i < 4; i++) _lineRender.SetPosition(i, _lineRender.GetPosition(i) + transform.position);
    }

    private void LineColor(Color color)
    {
        _lineRender.startColor = color;
        _lineRender.endColor = color;
    }

    private void OffsetPosCalcul()
    {
        Vector2 pos = TileSystem.Instance.GridToWorld(RotationSize.x - 1, RotationSize.y - 1);
        _offsetPos = pos / 2f;
    }
    private void SetPosFromPointer()
    {
        Vector2 pointer = GameplayScript.Instance.MouseWorldPosition;

        Vector2Int gridPos = TileSystem.Instance.WorldToGrid(pointer - new Vector2(_offsetPos.x, _offsetPos.y));
        transform.position = TileSystem.Instance.GridToWorld(gridPos.x, gridPos.y);
    }
    private void CheckNewPos()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 100 + transform.position.y);
        _lastPos = transform.position;
        Vector2Int gridPos = TileSystem.Instance.WorldToGrid(transform.position);

        ResetLineRenderer(RotationSize.x, RotationSize.y);

        _canPlace = TileSystem.Instance.CheckForPlacing(this, gridPos.x, gridPos.y);
        if (_canPlace && OwnItem.type == GMStatic.tagType.Mural)
            _canPlace = CheckMuralPos(gridPos);
        else if (_canPlace && OwnItem.type == GMStatic.tagType.Object)
        {
            int height = TileSystem.Instance.CheckItemTop(this, gridPos.x, gridPos.y);
            if (height < 0) _canPlace = false;
            else
            {

            }
        }

        _constraint.ResetConstraint(transform.position);
        _itemUI.TextIssues(!ConstraintValid, !DoorValid);

        if (!_canPlace) LineColor(Color.red);
        else if (!ConstraintValid || !DoorValid) LineColor(new Color(255, 69, 0));
        else LineColor(Color.green);
    }
    private bool CheckMuralPos(Vector2Int pos)
    {
        bool res = true;

        int decal;
        List<Vector2Int> list = new List<Vector2Int>();

        if (Orientation == 0 || Orientation == 180)
        {
            if (Orientation == 0) decal = RotationSize.x;
            else decal = -1;

            for (int i = 0; i < OwnItem.size.y; i++)
            {
                list.Add(new Vector2Int(pos.x + decal, pos.y + i));
            }
        }
        else if (Orientation == 90 || Orientation == 270)
        {
            if (Orientation == 90) decal = RotationSize.y;
            else decal = -1;

            for (int i = 0; i < OwnItem.size.y; i++)
            {
                list.Add(new Vector2Int(pos.x + i, pos.y + decal));
            }
        }

        foreach (Vector2Int current in list)
        {
            if (TileSystem.Instance.CheckTileExist(current.x, current.y) >= 0)
            {
                res = false; break;
            }
        }

        return res;
    }

    public void ChangeSpriteColor(Color color)
    {
        _spriteClrRender.color = color;
        ItemColor = color;
    }
    private void SpriteAppearance()
    {
        if (_orientation == 90 || _orientation == 270)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        else transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        Debug.Log(OwnItem);

        if (_orientation == 0 || _orientation == 90)
        {
            _spriteUnClrRender.sprite = OwnItem.spriteOneFixed;
            _spriteClrRender.sprite = OwnItem.spriteOneColored;
        }
        else if (!(OwnItem.spriteTwoFixed == null || OwnItem.spriteTwoColored == null))
        {
            _spriteUnClrRender.sprite = OwnItem.spriteTwoFixed;
            _spriteClrRender.sprite = OwnItem.spriteTwoColored;
        }

        ColliderReset();
    }
    private void ColliderReset()
    {
        if (_spriteUnCllrGmObj.TryGetComponent<PolygonCollider2D>(out PolygonCollider2D compon))
            Destroy(compon);

        _spriteUnCllrGmObj.AddComponent<PolygonCollider2D>();
    }
    private void CheckWhenPlaced()
    {
        if (OwnItem.type != GMStatic.tagType.Null)
        {
            _constraint.CheckConstraint();
            _itemUI.TextIssues(!ConstraintValid, !DoorValid);

            if (!ConstraintValid || !DoorValid)
            {
                LineColor(new Color(255, 69, 0));
                _spriteUnClrRender.color = Color.yellow;
            }
            else
            {
                LineColor(Color.green);
                _spriteUnClrRender.color = Color.white;
            }
        }
    }

    [Button] public void Rotation(int rotation = -1)
    {
        if (CurrentState == GMStatic.State.Waiting || CurrentState == GMStatic.State.Moving)
        {
            int limit = 180;
            if (OwnItem.fullRotation) limit += 180;

            if (rotation == 0 || rotation == 90 || rotation == 180 || rotation == 270)
                _orientation = (int)Mathf.Repeat(rotation, limit); // 0 - 90 - 180 - 270 // 0 at spawn //
            else
                _orientation = (int)Mathf.Repeat(_orientation + 90, limit); // 0 - 90 - 180 - 270 // 0 at spawn //

            if (_orientation == 90 || _orientation == 270)
            {
                _rotationSize = new Vector3Int(OwnItem.size.y, OwnItem.size.x, OwnItem.size.z);
            }
            else
            {
                _rotationSize = new Vector3Int(OwnItem.size.x, OwnItem.size.y, OwnItem.size.z);
            }

            ResetInfos();
        }
    } // Rotate the Item is clicked at Waiting State
    public void Place()
    {
        if (CurrentState != GMStatic.State.Placed)
        {
            Vector2Int gridPos = TileSystem.Instance.WorldToGrid(transform.position);
            TileSystem.Instance.PlacingItem(gameObject, gridPos.x, gridPos.y);
            SpawnSmoke(transform);
            CurrentState = GMStatic.State.Placed;
            _lineRender.enabled = false;
            _constraint.RenderLine(false);

            if (_itemUI != null) _itemUI.ActivateUI(false);

            TileSystem.Instance.ObjectOnScene(true);
            CheckWhenPlaced();
        }
    } // Place the Item on the grid and Change state for "placed" when a button is pushed
    public void Move()
    {
        if (CurrentState == GMStatic.State.Placed)
        {
            Vector2Int gridPos = TileSystem.Instance.WorldToGrid(transform.position);
            TileSystem.Instance.MoveItem(gameObject, gridPos.x, gridPos.y);

            CurrentState = GMStatic.State.Waiting;
            _lineRender.enabled = true;
            _constraint.RenderLine(true);

            if (_itemUI != null) _itemUI.SetupLeftButton();
        }
    } // Set the Item state from "placed" to "waiting" or "moving" when a button is pushed
    public void Remove()
    {
        TileSystem.Instance.ObjectOnScene(true);
        SpawnSmoke(transform);
        Vector2Int gridPos = TileSystem.Instance.WorldToGrid(transform.position);
        TileSystem.Instance.RemoveItem(gameObject, gridPos.x, gridPos.y);
    } // Remove the Item from the scene, need to make sure every information of the item gets deleted

    private void SpawnSmoke(Transform pos)
    {
        Instantiate(_smoke, pos);
    }

    public void SpawnScoreEffect(int quantity, bool isCombo)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + OffsetPos);
        ScoreEffectManager.Instance.SpawnEffect(screenPos, quantity, isCombo);
    }

    [Button]
    private void Init()
    {
        Initialize(_ownItem);
    }
}
