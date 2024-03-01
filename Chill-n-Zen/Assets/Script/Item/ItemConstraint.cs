using UnityEngine;
using GameManagerSpace;
using System.Collections.Generic;

public class ItemConstraint : MonoBehaviour
{
    [SerializeField] ItemBehaviour _itemBehaviour;
    List<Vector2Int> _listTilesPos = new List<Vector2Int>();
    List<bool> _listAccessible = new List<bool>();

    public bool IsValide { get; private set; }

    public void ResetConstraint(Vector2 worldPos)
    {
        Vector2Int gridPos = TileSystem.Instance.WorldToGrid(worldPos);
        _listTilesPos.Clear();
        _listAccessible.Clear();

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

        IsValide = ConstraintValidation();
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

        return res;
    }

    private void Front(Vector2Int pos)
    {
        //Vector2 direction = new Vector2(-Mathf.Cos(Mathf.Deg2Rad * _itemBehaviour.Orientation), -Mathf.Sin(Mathf.Deg2Rad * _itemBehaviour.Orientation));

        int decal;

        if (_itemBehaviour.Orientation == 0 || _itemBehaviour.Orientation == 180)
        {
            if (_itemBehaviour.Orientation == 0) decal = -1;
            else decal = _itemBehaviour.RotationSize.x;

            for (int i = 0; i < _itemBehaviour.OwnItem.size.y; i++)
            {
                int index = TileSystem.Instance.CheckTileExist(pos.x + decal, pos.y + i);
                if (index >= 0)
                {
                    _listTilesPos.Add(new Vector2Int(pos.x + decal, pos.y + i));
                }
            }
        }
        else if (_itemBehaviour.Orientation == 90 || _itemBehaviour.Orientation == 270)
        {
            if (_itemBehaviour.Orientation == 90) decal = -1;
            else decal = _itemBehaviour.RotationSize.y;

            for (int i = 0; i < _itemBehaviour.OwnItem.size.y; i++)
            {
                int index = TileSystem.Instance.CheckTileExist(pos.x + i, pos.y + decal);
                if (index >= 0)
                {
                    _listTilesPos.Add(new Vector2Int(pos.x + i, pos.y + decal));
                }
            }
        }
    }
    private void Bed(Vector2Int pos)
    {
        if (_itemBehaviour.Orientation == 0 || _itemBehaviour.Orientation == 180)
        {
            for (int i = 0; i < _itemBehaviour.OwnItem.size.x; i++)
            {
                int index = TileSystem.Instance.CheckTileExist(pos.x + i, pos.y - 1);
                if (index >= 0)
                {
                    _listTilesPos.Add(new Vector2Int(pos.x + i, pos.y - 1));
                }
                
                index = TileSystem.Instance.CheckTileExist(pos.x + i, pos.y + _itemBehaviour.OwnItem.size.y +1);
                if (index >= 0)
                {
                    _listTilesPos.Add(new Vector2Int(pos.x + i, pos.y + _itemBehaviour.OwnItem.size.y + 1));
                }
            }
        }
        else if (_itemBehaviour.Orientation == 90 || _itemBehaviour.Orientation == 270)
        {
            for (int i = 0; i < _itemBehaviour.OwnItem.size.x; i++)
            {
                int index = TileSystem.Instance.CheckTileExist(pos.x - 1, pos.y + i);
                if (index >= 0)
                {
                    _listTilesPos.Add(new Vector2Int(pos.x - 1, pos.y + i));
                }

                index = TileSystem.Instance.CheckTileExist(pos.x + _itemBehaviour.OwnItem.size.y + 1, pos.y + i);
                if (index >= 0)
                {
                    _listTilesPos.Add(new Vector2Int(pos.x + _itemBehaviour.OwnItem.size.y + 1, pos.y + i));
                }
            }
    }
}