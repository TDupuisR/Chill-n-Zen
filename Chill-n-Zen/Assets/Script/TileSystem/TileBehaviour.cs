using System.Collections.Generic;
using UnityEngine;
using GameManagerSpace;

public class TileBehaviour : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRender;
    [SerializeField] LineRenderer _lineRender;

    List<Item> _presentItems = new List<Item>();
    Vector3[] _patternPosition = new Vector3[5];

    private void OnEnable()
    {
        TileSystem.OnShowGrid += ShowGrid;
        TileSystem.OnShowGridSpecified += ShowGrid;

        ChangeAesthetic();
        GridInitialise();
    }
    private void OnDisable()
    {
        TileSystem.OnShowGrid -= ShowGrid;
        TileSystem.OnShowGridSpecified -= ShowGrid;
    }

    private void ChangeAesthetic()
    {
        Debug.LogWarning(" (error : 3x0) Missing Floor texture ", gameObject);
        // _renderer.sprite = // GameManager get Floor Texture //
        _spriteRender.color = GameManager.colorData.GroundColor;
    }

    private void GridReset()
    {
        _lineRender.startColor = Color.white;
        _lineRender.endColor = Color.white;

        for (int i = 0; i < 5; i++)
        {
            _lineRender.SetPosition(i, _patternPosition[i] + transform.position);
        }
    }
    private void GridInitialise()
    {
        for (int i = 0; i < 5; i++)
        {
            _patternPosition[i] = _lineRender.GetPosition(i);
        }
        _lineRender.enabled = false;
    }

    private void ShowGrid()
    {
        GridReset();
        _lineRender.enabled = !_lineRender.enabled;
    }
    private void ShowGrid(bool state)
    {
        GridReset();
        _lineRender.enabled = state;
    }

    public bool CheckIfPlacable(Item placing)
    {
        bool res = true;

        foreach (Item item in _presentItems)
        {
            if (
                (item.type == placing.type) || (item.type == GMStatic.tagType.Null) || 
                (item.type == GMStatic.tagType.Furniture && placing.type == GMStatic.tagType.Object && !item.listUsage.Contains(GMStatic.tagUsage.Top)) || 
                (item.type == GMStatic.tagType.Object && placing.type == GMStatic.tagType.Furniture) || 
                (item.type == GMStatic.tagType.Furniture && placing.type == GMStatic.tagType.Mural && placing.size.z < item.size.z) ||
                (item.type == GMStatic.tagType.Mural && placing.type == GMStatic.tagType.Furniture && item.size.z < placing.size.z) ||
                (placing.type == GMStatic.tagType.Ceiling && item.type != GMStatic.tagType.Mural && item.size.z >= placing.size.z)
               )
            {
                res = false; break;
            }
        }

        return res;
    }
    public int CheckIfTop(Item placing)
    {
        int res = 0;

        foreach (Item item in _presentItems)
        {
            if (item.type == GMStatic.tagType.Furniture && placing.type == GMStatic.tagType.Object && item.listUsage.Contains(GMStatic.tagUsage.Top))
            {
                res += item.size.z;
            }
        }

        return res;
    }
    public bool CheckIfAccessible(GMStatic.constraint constr)
    {
        bool res = true;

        foreach (Item item in _presentItems)
        {
            if (
                (item.type == GMStatic.tagType.Furniture) ||
                (item.type == GMStatic.tagType.Object)
                )
            {
                if (constr == GMStatic.constraint.Seat || constr == GMStatic.constraint.Chair)
                {
                    foreach (GMStatic.tagUsage usage in item.listUsage)
                    {
                        if (usage == GMStatic.tagUsage.Table || usage == GMStatic.tagUsage.Desk || usage == GMStatic.tagUsage.CoffeeTable ) { res = true; break; }
                    }
                    if (!res) break;
                }
                else
                {
                    res = false; break;
                }
            }
        }

        return res;
    }
    public bool CheckConditionExist(Item checking)
    {
        bool res = false;

        foreach (Item item in _presentItems)
        {
            if (item == checking)
            {
                res =true; break;
            }
        }

        return res;
    }
    public bool CheckConditionExist(GMStatic.tagUsage checking)
    {
        bool res = false;

        foreach (Item item in _presentItems)
        {
            foreach (GMStatic.tagUsage usage in item.listUsage)
            {
                if (usage == checking)
                {
                    res = true; break;
                }
            }
        }

        return res;
    }

    public void PlaceItem(Item placing)
    {
        if (CheckIfPlacable(placing))
        {
            _presentItems.Add(placing);
        }
    }
    public void RemoveItem(Item current)
    {
        _presentItems.Remove(current);
    }
}
