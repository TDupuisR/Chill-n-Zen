using UnityEngine;
using NaughtyAttributes;
using GameManagerSpace;

public class ItemBehaviour : MonoBehaviour
{
    [SerializeField] GameObject _spriteGO;
    SpriteRenderer _spriteRender;
    [SerializeField] ItemUI _itemUI;

    [SerializeField] LineRenderer _lineRender;
    [SerializeField] Item _ownItem;

    Vector3Int _rotationSize = Vector3Int.zero;
    int _rotation = 0;
    Vector3 _offsetPos = Vector3.zero;
    Vector3 _lastPos;
    bool _canPlace = false;

    Vector3[] _patternPosition = new Vector3[5];

    public Item OwnItem { get { return _ownItem; } }
    public Vector3Int RotationSize { get { return _rotationSize; } }
    public bool CanPlace { get { return _canPlace;} }
    public GMStatic.State CurrentState { get; set; }

    private void OnValidate()
    {
        if (_lineRender == null) 
            Debug.LogError(" (error : 4x0) No LineRenderer assigned ) ", gameObject);
        if (_spriteGO == null)
            Debug.LogError(" (error : 4x1) No Sprite child GameObject assigned ) ", gameObject);
    }

    private void OnEnable()
    {
        _spriteRender = _spriteGO.GetComponent<SpriteRenderer>();

        InitPattern();
    }

    private void Update()
    {
        if (CurrentState == GMStatic.State.Moving)
        {
            SetPosFromPointer();

            if (transform.position != _lastPos) CheckNewPos();
        }
    }

    public void Initialize(Item item)
    {
        _ownItem = item;
        if (_ownItem.size.x <= 0 || _ownItem.size.y <= 0 || _ownItem.size.z <= -1)
        {
            Debug.LogError(" (error : 4x2) Size of the item out of bound (null or negative values) ", gameObject);
            Remove(); return;
        }

        TileSystem.Instance.ObjectOnScene(false);

        _rotationSize = OwnItem.size;
        _rotation = 0;

        _spriteRender.sprite = _ownItem.spriteOneFixed;
        OffsetPosCalcul();
        _spriteGO.transform.position = transform.position + _offsetPos;
        ResetLineRenderer(RotationSize.x, RotationSize.y);

        SpriteAppearance();

        CurrentState = GMStatic.State.Moving;
    }
    private void ResetInfos()
    {
        OffsetPosCalcul();
        _spriteGO.transform.position = transform.position + _offsetPos;
        ResetLineRenderer(RotationSize.x, RotationSize.y);

        SpriteAppearance();
    }

    private void InitPattern()
    {
        for (int i = 0; i < 5; i++)
        {
            _patternPosition[i] = _lineRender.GetPosition(i);
        }
    }
    private void ResetLineRenderer(int sizeX, int sizeY, Vector2 pos = new Vector2())
    {
        for (int i = 0; i < 5; i++) _lineRender.SetPosition(i, _patternPosition[i]);
        //Vector2 posW = TileSystem.Instance.GridToWorld((int)pos.x, (int)pos.y);

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                Vector2 worldPos = TileSystem.Instance.GridToWorld(x/* + (int)posW.x*/, y/* + (int)posW.y*/);

                if (_patternPosition[0].y + worldPos.y > _lineRender.GetPosition(0).y)
                {
                    _lineRender.SetPosition(0, new Vector3(_patternPosition[0].x + worldPos.x, _patternPosition[0].y + worldPos.y, 0));
                    _lineRender.SetPosition(4, new Vector3(_patternPosition[4].x + worldPos.x, _patternPosition[0].y + worldPos.y, 0));
                }
                if (_patternPosition[1].x + worldPos.x > _lineRender.GetPosition(1).x)
                    _lineRender.SetPosition(1, new Vector3(_patternPosition[1].x + worldPos.x, _patternPosition[1].y + worldPos.y, 0));
                if (_patternPosition[2].y + worldPos.y < _lineRender.GetPosition(2).y)
                    _lineRender.SetPosition(2, new Vector3(_patternPosition[2].x + worldPos.x, _patternPosition[2].y + worldPos.y, 0));
                if (_patternPosition[3].x + worldPos.x < _lineRender.GetPosition(3).x)
                    _lineRender.SetPosition(3, new Vector3(_patternPosition[3].x + worldPos.x, _patternPosition[3].y + worldPos.y, 0));
            }
        }

        for (int i = 0; i < 5; i++) _lineRender.SetPosition(i, _lineRender.GetPosition(i) + transform.position);
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
        _lastPos = transform.position;
        Vector2Int gridPos = TileSystem.Instance.WorldToGrid(_lastPos);

        ResetLineRenderer(RotationSize.x, RotationSize.y, gridPos);
        _canPlace = TileSystem.Instance.CheckForPlacing(this, gridPos.x, gridPos.y);

        if (!_canPlace) LineColor(Color.red);
        else LineColor(Color.green);
    }

    private void SpriteAppearance()
    {
        if (_rotation == 90 || _rotation == 180)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        else transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        if (!(OwnItem.spriteTwoFixed == null || OwnItem.spriteTwoColored == null))
        {
            if (_rotation == 0 || _rotation == 90)
            {
                _spriteRender.sprite = OwnItem.spriteOneFixed;
            }
            else
            {
                _spriteRender.sprite = OwnItem.spriteTwoFixed;
            }
        }
    }

    [Button] public void Rotation()
    {
        if (CurrentState == GMStatic.State.Waiting || CurrentState == GMStatic.State.Moving)
        {
            _rotation = (int)Mathf.Repeat(_rotation + 90, 360); // 0 - 90 - 180 - 270 // 0 at spawn //

            if (_rotation == 90 || _rotation == 270)
            {
                _rotationSize = new Vector3Int(OwnItem.size.y, OwnItem.size.x, OwnItem.size.z);
            }
            else
            {
                _rotationSize = new Vector3Int(OwnItem.size.x, OwnItem.size.y, OwnItem.size.z);
            }

            ResetInfos();
        }

        Debug.Log(RotationSize + " | " + _rotation);
    } // Rotate the Item when a button is pushed
    public void Place()
    {
        if (CurrentState != GMStatic.State.Placed)
        {
            Vector2Int gridPos = TileSystem.Instance.WorldToGrid(_lastPos);
            TileSystem.Instance.PlacingItem(gameObject, gridPos.x, gridPos.y);

            CurrentState = GMStatic.State.Placed;
            _lineRender.enabled = false;

            _itemUI.ActivateUI(false);

            TileSystem.Instance.ObjectOnScene(true);
        }

    } // Place the Item on the grid and Change state for "placed" when a button is pushed
    public void Move()
    {
        if (CurrentState == GMStatic.State.Placed)
        {
            Vector2Int gridPos = TileSystem.Instance.WorldToGrid(_lastPos);
            TileSystem.Instance.MoveItem(gameObject, gridPos.x, gridPos.y);

            CurrentState = GMStatic.State.Waiting;
            _lineRender.enabled = true;

            _itemUI.SetupLeftButton();
        }
    } // Set the Item state from "placed" to "waiting" or "moving" when a button is pushed
    public void Remove()
    {
        TileSystem.Instance.ObjectOnScene(true);

        Vector2Int gridPos = TileSystem.Instance.WorldToGrid(_lastPos);
        TileSystem.Instance.RemoveItem(gameObject, gridPos.x, gridPos.y);
    } // Remove the Item from the scene, need to make sure every information of the item gets deleted

    [Button]
    private void Init()
    {
        Initialize(_ownItem);
    }
}
