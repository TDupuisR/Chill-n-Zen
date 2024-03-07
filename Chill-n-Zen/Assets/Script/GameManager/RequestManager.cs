using GameManagerSpace;
using System.Collections.Generic;
using UnityEngine;

public class RequestManager : MonoBehaviour
{
    GMStatic.Request _primaryList;
    GMStatic.Request _secondaryList;

    public int FreeTiles { get; set; }

    public void Initialisation(GMStatic.Request primary, GMStatic.Request secondary)
    {
        _primaryList = primary;
        _secondaryList = secondary;
    }

    public List<string> ReturnDescriptions(bool primary)
    {
        List<string> ret = new List<string>();

        GMStatic.Request list;
        if (primary) list = _primaryList;
        else list = _secondaryList;

        foreach (GMStatic.requestObj current in list.obj)
            ret.Add(current.phraseClient);
        foreach (GMStatic.requestUsage current in list.usage)
            ret.Add(current.phraseClient);
        foreach (GMStatic.requestColor current in list.color)
            ret.Add(current.phraseClient);
        foreach (GMStatic.requestMaterial current in list.material)
            ret.Add(current.phraseClient);
        foreach (GMStatic.requestProximity current in list.proximity)
            ret.Add(current.phraseClient);
        foreach (GMStatic.requestFreeSpace current in list.freeSpace)
            ret.Add(current.phraseClient);

        return ret;
    }
    public List<bool> ReturnStatus(bool primary)
    {
        List<bool> ret = new List<bool>();

        GMStatic.Request list;
        if (primary) list = _primaryList;
        else list = _secondaryList;

        foreach (GMStatic.requestObj current in list.obj)
            ret.Add(CheckObjRequest(current));
        foreach (GMStatic.requestUsage current in list.usage)
            ret.Add(CheckTypeRequest(current));
        foreach (GMStatic.requestColor current in list.color)
            ret.Add(CheckColorRequest(current));
        foreach (GMStatic.requestMaterial current in list.material)
            ret.Add(CheckMaterialRequest(current));
        foreach (GMStatic.requestProximity current in list.proximity)
            ret.Add(CheckProximityRequest(current));
        foreach (GMStatic.requestFreeSpace current in list.freeSpace)
            ret.Add(CheckFreeSpaceRequest(current));

        return ret;
    }

    private bool CheckObjRequest(GMStatic.requestObj request)
    {
        bool res = false;

        int count = 0;
        foreach (ItemBehaviour item in TileSystem.Instance.ItemList)
        {
            if (item.OwnItem == request.itemRequested)
                count++;

            if (count >= request.nbRequested) res = true;
            if (res) break;
        }

        return res;
    }
    private bool CheckTypeRequest(GMStatic.requestUsage request)
    {
        bool res = false;

        int count = 0;
        foreach (ItemBehaviour item in TileSystem.Instance.ItemList)
        {
            foreach (GMStatic.tagUsage usage in item.OwnItem.listUsage)
            {
                if (usage == request.usageRequested)
                {
                    count++;
                    break;
                }
            }

            if (count >= request.nbRequested) res = true;
            if (res) break;
        }

        return res;
    }
    private bool CheckColorRequest(GMStatic.requestColor request)
    {
        bool res = false;

        int count = 0;
        foreach (ItemBehaviour item in TileSystem.Instance.ItemList)
        {
            if (item.ItemColor == request.colorRequested)
                count++;

            if (count >= request.nbRequested) res = true;
            if (res) break;
        }

        return res;
    }
    private bool CheckMaterialRequest(GMStatic.requestMaterial request)
    {
        bool res = false;

        int count = 0;
        foreach (ItemBehaviour item in TileSystem.Instance.ItemList)
        {
            if (item.OwnItem.material == request.materialRequested)
                count++;

            if (count >= request.nbRequested) res = true;
            if (res) break;
        }

        return res;
    }
    private bool CheckProximityRequest(GMStatic.requestProximity request)
    {
        bool res = false;

        int count = 0;
        foreach (ItemBehaviour item in TileSystem.Instance.ItemList)
        {
            bool currentRes = false;

            foreach (GMStatic.tagUsage usage in item.OwnItem.listUsage)
            {
                if (usage == request.closeFromRequested)
                {
                    Vector2Int gridPos = TileSystem.Instance.WorldToGrid(item.transform.position);
                    List<Vector2Int> list = new List<Vector2Int>();

                    for (int i = 0; i < item.RotationSize.x; i++)
                    {
                        list.Add(new Vector2Int(gridPos.x + i, gridPos.y - 1));
                        list.Add(new Vector2Int(gridPos.x + i, gridPos.y + item.RotationSize.y));
                    }
                    for (int j = 0; j < item.RotationSize.y; j++)
                    {
                        list.Add(new Vector2Int(gridPos.x - 1, gridPos.y + j));
                        list.Add(new Vector2Int(gridPos.x + item.RotationSize.x, gridPos.y + j));
                    }
                    for (int x = 0; x < item.RotationSize.x; x++)
                    {
                        for (int y = 0; y < item.RotationSize.y; y++)
                        {
                            list.Add(new Vector2Int(gridPos.x + x, gridPos.y + y));
                        }
                    }

                    foreach (Vector2Int pos in list)
                    {
                        foreach (GMStatic.tagUsage type in request.closeToRequested)
                        {
                            if (TileSystem.Instance.CheckForBonus(type, pos.x, pos.y))
                            { 
                                currentRes = true; break;
                            }

                            if (currentRes) break;
                        }
                    }

                    if (currentRes) break;
                }
            }

            if (currentRes) count++;
            if (count >= request.nbRequested) res = true;

            if (res) break;
        }

        return res;
    }
    private bool CheckFreeSpaceRequest(GMStatic.requestFreeSpace request)
    {
        bool res = false;

        if (FreeTiles >= request.nbRequested)
            res = true;

        return res;
    }
}
