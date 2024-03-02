using UnityEngine;
using GameManagerSpace;
using System.Collections.Generic;

public class ItemConstraint : MonoBehaviour
{
    [SerializeField] ItemBehaviour _itemBehaviour;
    [SerializeField] LineRenderer _lineRender;

    List<Vector2Int> _listTilesPos = new List<Vector2Int>();
    List<bool> _listAccessible = new List<bool>();

    Vector3[] _patternPosition = new Vector3[4];

    public bool IsValide { get; private set; }

    public void OnEnable()
    {
        for (int i = 0; i < 4; i++)
        {
            _patternPosition[i] = _lineRender.GetPosition(i);
        }

        LineColor(Color.yellow);
    }

    private void ResetLineRenderer(Vector2Int pos)
    {
        Vector3 firstPos = TileSystem.Instance.GridToWorld(_listTilesPos[0].x - pos.x, _listTilesPos[0].y - pos.y);
        for (int i = 0; i < 4; i++) _lineRender.SetPosition(i, _patternPosition[i] + firstPos);

        foreach (Vector2Int tile in _listTilesPos)
        {
            Vector2 worldPos = TileSystem.Instance.GridToWorld(tile.x - pos.x, tile.y - pos.y);

            if (_patternPosition[0].y + worldPos.y > _lineRender.GetPosition(0).y)
                _lineRender.SetPosition(0, new Vector3(_patternPosition[0].x + worldPos.x, _patternPosition[0].y + worldPos.y, 0));
            if (_patternPosition[1].x + worldPos.x > _lineRender.GetPosition(1).x)
                _lineRender.SetPosition(1, new Vector3(_patternPosition[1].x + worldPos.x, _patternPosition[1].y + worldPos.y, 0));
            if (_patternPosition[2].y + worldPos.y < _lineRender.GetPosition(2).y)
                _lineRender.SetPosition(2, new Vector3(_patternPosition[2].x + worldPos.x, _patternPosition[2].y + worldPos.y, 0));
            if (_patternPosition[3].x + worldPos.x < _lineRender.GetPosition(3).x)
                _lineRender.SetPosition(3, new Vector3(_patternPosition[3].x + worldPos.x, _patternPosition[3].y + worldPos.y, 0));
        }

        for (int i = 0; i < 4; i++) _lineRender.SetPosition(i, _lineRender.GetPosition(i) + transform.position);
    }
    private void LineColor(Color color)
    {
        _lineRender.startColor = color;
        _lineRender.endColor = color;
    }
    public void RenderLine(bool state)
    {
        _lineRender.enabled = state;
    }

    public void ResetConstraint(Vector2 worldPos)
    {
        Vector2Int gridPos = TileSystem.Instance.WorldToGrid(worldPos);
        _listTilesPos.Clear();
        _listAccessible.Clear();

        if (_itemBehaviour.OwnItem.constraint != GMStatic.constraint.None)
        {
            if (_itemBehaviour.OwnItem.constraint == GMStatic.constraint.Front || _itemBehaviour.OwnItem.constraint == GMStatic.constraint.Seat)
            {
                Front(gridPos);

                foreach (Vector2Int tilePos in _listTilesPos)
                {
                    _listAccessible.Add(TileSystem.Instance.CheckForAccessing(tilePos.x, tilePos.y, _itemBehaviour.OwnItem.constraint));
                }
            }
            else if (_itemBehaviour.OwnItem.constraint == GMStatic.constraint.Bed)
            {
                Bed(gridPos);

                foreach (Vector2Int tilePos in _listTilesPos)
                {
                    _listAccessible.Add(TileSystem.Instance.CheckForAccessing(tilePos.x, tilePos.y, _itemBehaviour.OwnItem.constraint));
                }
            }

            if (_listTilesPos.Count > 0) ResetLineRenderer(gridPos);
            _lineRender.enabled = true;
        }
        else { _lineRender.enabled = false; }

        IsValide = ConstraintValidation();

        Debug.Log("Tiles: " + _listTilesPos.Count + " | Access: " + _listAccessible.Count + " | Valid: " + IsValide);
    }
    public void CheckConstraint()
    {
        if (_itemBehaviour.OwnItem.constraint == GMStatic.constraint.Front || _itemBehaviour.OwnItem.constraint == GMStatic.constraint.Seat)
        {
            foreach (Vector2Int tilePos in _listTilesPos)
            {
                _listAccessible.Add(TileSystem.Instance.CheckForAccessing(tilePos.x, tilePos.y, _itemBehaviour.OwnItem.constraint));
            }
        }
        else if (_itemBehaviour.OwnItem.constraint == GMStatic.constraint.Bed)
        {
            foreach (Vector2Int tilePos in _listTilesPos)
            {
                _listAccessible.Add(TileSystem.Instance.CheckForAccessing(tilePos.x, tilePos.y, _itemBehaviour.OwnItem.constraint));
            }
        }

        IsValide = ConstraintValidation();
    }

    private bool ConstraintValidation()
    {
        bool res = true;

        if (_itemBehaviour.OwnItem.constraint == GMStatic.constraint.Front || _itemBehaviour.OwnItem.constraint == GMStatic.constraint.Seat)
        {
            foreach (bool flag in _listAccessible)
            {
                if (!flag) res = false;
            }
        }
        else if (_itemBehaviour.OwnItem.constraint == GMStatic.constraint.Bed)
        {
            int count = 0;

            foreach (bool flag in _listAccessible)
            {
                if (flag) count += 1;
            }

            if (count < 1 ) res = false;
        }

        if (_itemBehaviour.OwnItem.constraint != GMStatic.constraint.None && _listAccessible.Count <= 0) res = false;

        return res;
    }

    private void Front(Vector2Int pos)
    {
        int decal;

        if (_itemBehaviour.Orientation == 0 || _itemBehaviour.Orientation == 180)
        {
            if (_itemBehaviour.Orientation == 0) decal = -1;
            else decal = _itemBehaviour.RotationSize.x;

            for (int i = 0; i < _itemBehaviour.OwnItem.size.y; i++)
            {
                _listTilesPos.Add(new Vector2Int(pos.x + decal, pos.y + i));
            }
        }
        else if (_itemBehaviour.Orientation == 90 || _itemBehaviour.Orientation == 270)
        {
            if (_itemBehaviour.Orientation == 90) decal = -1;
            else decal = _itemBehaviour.RotationSize.y;

            for (int i = 0; i < _itemBehaviour.OwnItem.size.y; i++)
            {
                _listTilesPos.Add(new Vector2Int(pos.x + i, pos.y + decal));
            }
        }
    }
    private void Bed(Vector2Int pos)
    {
        if (_itemBehaviour.Orientation == 0 || _itemBehaviour.Orientation == 180)
        {
            for (int i = 0; i < _itemBehaviour.OwnItem.size.x; i++)
            {
                _listTilesPos.Add(new Vector2Int(pos.x + i, pos.y - 1));
                _listTilesPos.Add(new Vector2Int(pos.x + i, pos.y + _itemBehaviour.OwnItem.size.y));
            }
        }
        else if (_itemBehaviour.Orientation == 90 || _itemBehaviour.Orientation == 270)
        {
            for (int i = 0; i < _itemBehaviour.OwnItem.size.x; i++)
            {
                _listTilesPos.Add(new Vector2Int(pos.x - 1, pos.y + i));
                _listTilesPos.Add(new Vector2Int(pos.x + _itemBehaviour.OwnItem.size.y, pos.y + i));
            }
        }
    }
}