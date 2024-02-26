using UnityEngine;
using NaughtyAttributes;

public class ItemBehaviour : MonoBehaviour
{
    [SerializeField] GameObject _spriteGO;
    SpriteRenderer _spriteRender;

    [SerializeField] LineRenderer _lineRender;
    [SerializeField] Item _ownItem;

    Vector3 _offsetPos = Vector3.zero;
    Vector3 _lastPos;
    bool _canPlace = false;

    enum State { Placed, Moving, Waiting }
    State _state;
    Vector3[] _patternPosition = new Vector3[5];

    public Item OwnItem { get { return _ownItem; } }

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
        if (_state == State.Moving)
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

        _spriteRender.sprite = _ownItem.asset2D;
        OffsetPosCalcul(_ownItem.size.x, _ownItem.size.y);
        _spriteGO.transform.position = transform.position + _offsetPos;

        ResizeLineRenderer(_ownItem.size.x, _ownItem.size.y);

        _state = State.Moving;
    }
    private void InitPattern()
    {
        for (int i = 0; i < 5; i++)
        {
            _patternPosition[i] = _lineRender.GetPosition(i);
        }
    }

    private void OffsetPosCalcul(int sizeX, int sizeY)
    {
        Vector2 pos = TileSystem.Instance.GridToWorld(sizeX, sizeY);
        _offsetPos = pos / 2f;
    }
    private void ResizeLineRenderer(int sizeX, int sizeY)
    {
        for (int i = 0; i < 5; i++) _lineRender.SetPosition(i, _patternPosition[i]);

        //int negX = 1;
        //int negY = 1;
        //if (sizeX < 0) negX = -1;
        //if (sizeY < 0) negY = -1;

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                Vector2 pos = TileSystem.Instance.GridToWorld(x, y);

                if (_patternPosition[0].y + pos.y > _lineRender.GetPosition(0).y)
                {
                    _lineRender.SetPosition(0, new Vector3(_patternPosition[0].x + pos.x, _patternPosition[0].y + pos.y, 0));
                    _lineRender.SetPosition(4, new Vector3(_patternPosition[4].x + pos.x, _patternPosition[0].y + pos.y, 0));
                }
                if (_patternPosition[1].x + pos.x > _lineRender.GetPosition(1).x)
                    _lineRender.SetPosition(1, new Vector3(_patternPosition[1].x + pos.x, _patternPosition[1].y + pos.y, 0));
                if (_patternPosition[2].y + pos.y < _lineRender.GetPosition(2).y)
                    _lineRender.SetPosition(2, new Vector3(_patternPosition[2].x + pos.x, _patternPosition[2].y + pos.y, 0));
                if (_patternPosition[3].x + pos.x < _lineRender.GetPosition(3).x)
                    _lineRender.SetPosition(3, new Vector3(_patternPosition[3].x + pos.x, _patternPosition[3].y + pos.y, 0));
            }
        }
    }
    private void LineColor(Color color)
    {
        _lineRender.startColor = color;
        _lineRender.endColor = color;
    }

    private void SetPosFromPointer()
    {
        Vector2 pointer = GameplayScript.Instance.PrimaryPosition;

        transform.position = (Vector2)TileSystem.Instance.WorldToGrid(pointer + new Vector2(_offsetPos.x, _offsetPos.y));
    }
    private void CheckNewPos()
    {
        _lastPos = transform.position;
        Vector2Int gridPos = TileSystem.Instance.WorldToGrid(_lastPos);

        _canPlace = TileSystem.Instance.CheckForPlacing(OwnItem, gridPos.x, gridPos.y);

        if (!_canPlace) LineColor(Color.red);
        else LineColor(Color.green);
    }

    [Button] public void Rotation()
    {
        _ownItem.orientation = (int)Mathf.Repeat(_ownItem.orientation + 90, 360);
        Debug.Log(_ownItem.orientation);

    } // Rotate the Item when a button is pushed
    public void Place()
    {

    } // Place the Item on the grid and Change state for "placed" when a button is pushed
    public void Move()
    {

    } // Set the Item state from "placed" to "waiting" or "moving" when a button is pushed
    public void Remove()
    {

    } // Remove the Item from the scene, need to make sure every information of the item gets deleted

    [Button]
    private void Init()
    {
        Initialize(_ownItem);
    }
}
