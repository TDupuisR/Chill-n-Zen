using GameManagerSpace;
using System.Collections.Generic;
using UnityEngine;

public class ItemPointsChecker : MonoBehaviour
{
    [SerializeField] ItemBehaviour _itemBehave;
    private int _lastScore;

    private Item OwnItem { get { return _itemBehave.OwnItem; } }

    private void OnEnable()
    {
        _lastScore = 0;
    }

    public int GivePoints()
    {
        int res = 0;

        if (_itemBehave.ConstraintValid && _itemBehave.DoorValid)
        {
            Vector2Int gridPos = TileSystem.Instance.WorldToGrid(_itemBehave.transform.position);

            res += OwnItem.score;

            if (OwnItem.frontBonus > 0)
                if (CheckFrontBonus(gridPos))
                    res += OwnItem.frontBonus;

            if (OwnItem.proxiBonus > 0)
                if (CheckProxiBonus(gridPos))
                    res += OwnItem.proxiBonus;

            if (OwnItem.onBonus > 0)
                if (CheckOnBonus(gridPos))
                    res += OwnItem.onBonus;
        }

        _lastScore = res;
        return res;
    } 

    private bool CheckFrontBonus(Vector2Int gridPos)
    {
        int decal;
        List<Vector2Int> list = new List<Vector2Int>();

        if (_itemBehave.Orientation == 0 || _itemBehave.Orientation == 180)
        {
            if (_itemBehave.Orientation == 0) decal = -1;
            else decal = _itemBehave.RotationSize.x;

            for (int i = 0; i < _itemBehave.OwnItem.size.y; i++)
            {
                list.Add(new Vector2Int(gridPos.x + decal, gridPos.y + i));
            }
        }
        else if (_itemBehave.Orientation == 90 || _itemBehave.Orientation == 270)
        {
            if (_itemBehave.Orientation == 90) decal = -1;
            else decal = _itemBehave.RotationSize.y;

            for (int i = 0; i < _itemBehave.OwnItem.size.y; i++)
            {
                list.Add(new Vector2Int(gridPos.x + i, gridPos.y + decal));
            }
        }

        bool res = false;

        foreach (GMStatic.tagUsage usage in OwnItem.frontUsageBonus)
        {   
            foreach (Vector2Int pos in list)
            {
                if (TileSystem.Instance.CheckForBonus(usage, pos.x, pos.y))
                    res = true;

                if (res) break;
            }

            if (res) break;
        }
        foreach (Item item in OwnItem.frontItemBonus)
        {
            foreach (Vector2Int pos in list)
            {
                if (TileSystem.Instance.CheckForBonus(item, pos.x, pos.y))
                    res = true;

                if (res) break;
            }

            if (res) break;
        }

        return res;
    }
    private bool CheckProxiBonus(Vector2Int gridPos)
    {
        List<Vector2Int> list = new List<Vector2Int>();

        for (int i = 0; i < _itemBehave.RotationSize.x; i++)
        {
            list.Add(new Vector2Int(gridPos.x + i, gridPos.y - 1));
            list.Add(new Vector2Int(gridPos.x + i, gridPos.y + _itemBehave.RotationSize.y));
        }
        for (int j = 0; j < _itemBehave.RotationSize.y; j++)
        {
            list.Add(new Vector2Int(gridPos.x - 1, gridPos.y + j));
            list.Add(new Vector2Int(gridPos.x + _itemBehave.RotationSize.x, gridPos.y + j));
        }

        bool res = false;

        foreach (GMStatic.tagUsage usage in OwnItem.proxiUsageBonus)
        {
            foreach (Vector2Int pos in list)
            {
                if (TileSystem.Instance.CheckForBonus(usage, pos.x, pos.y))
                    res = true;

                if (res) break;
            }

            if (res) break;
        }
        foreach (Item item in OwnItem.proxiItemBonus)
        {
            foreach (Vector2Int pos in list)
            {
                if (TileSystem.Instance.CheckForBonus(item, pos.x, pos.y))
                    res = true;

                if (res) break;
            }

            if (res) break;
        }

        return res;
    }
    private bool CheckOnBonus(Vector2Int gridPos)
    {
        List<Vector2Int> list = new List<Vector2Int>();

        for (int x = 0; x < _itemBehave.RotationSize.x; x++)
        {
            for (int y = 0; y < _itemBehave.RotationSize.y; y++)
            {
                list.Add(new Vector2Int(gridPos.x + x, gridPos.y + y));
            }
        }

        bool res = false;

        foreach (GMStatic.tagUsage usage in OwnItem.onUsageBonus)
        {
            foreach (Vector2Int pos in list)
            {
                if (TileSystem.Instance.CheckForBonus(usage, pos.x, pos.y))
                    res = true;

                if (res) break;
            }

            if (res) break;
        }
        foreach (Item item in OwnItem.onItemBonus)
        {
            foreach (Vector2Int pos in list)
            {
                if (TileSystem.Instance.CheckForBonus(item, pos.x, pos.y))
                    res = true;

                if (res) break;
            }

            if (res) break;
        }

        return res;
    }
}
