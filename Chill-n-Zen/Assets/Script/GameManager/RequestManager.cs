using GameManagerSpace;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RequestManager : MonoBehaviour
{
    GMStatic.Request _primaryList;
    GMStatic.Request _secondaryList;

    public int FreeTiles { get; set; }

    public static Action OnFinishInitialisation;

    public void Initialisation(GMStatic.Request primary, GMStatic.Request secondary)
    {
        _primaryList = primary;
        _secondaryList = secondary;

        OnFinishInitialisation?.Invoke();
    }

    public List<string> ReturnDescriptions(bool primary)
    {
        List<string> ret = new List<string>();

        GMStatic.Request list;
        if (primary) list = _primaryList;
        else list = _secondaryList;

        if(list.obj != null)
            foreach (GMStatic.requestObj current in list.obj)
                ret.Add(current.phraseClient);
        if (list.usage != null)
            foreach (GMStatic.requestUsage current in list.usage)
                ret.Add(current.phraseClient);
        if (list.color != null)
            foreach (GMStatic.requestColor current in list.color)
                ret.Add(current.phraseClient);
        if (list.material != null)
            foreach (GMStatic.requestMaterial current in list.material)
                ret.Add(current.phraseClient);
        if (list.proximity != null)
            foreach (GMStatic.requestProximity current in list.proximity)
                ret.Add(current.phraseClient);
        if (list.freeSpace != null)
            foreach (GMStatic.requestFreeSpace current in list.freeSpace)
                ret.Add(current.phraseClient);

        return ret;
    }
    public List<string> ReturnSolution(bool primary)
    {
        List<string> ret = new List<string>();

        GMStatic.Request list;
        if (primary) list = _primaryList;
        else list = _secondaryList;

        if (list.obj != null)
            foreach (GMStatic.requestObj current in list.obj)
                ret.Add(current.solution);
        if (list.usage != null)
            foreach (GMStatic.requestUsage current in list.usage)
                ret.Add(current.solution);
        if (list.color != null)
            foreach (GMStatic.requestColor current in list.color)
                ret.Add(current.solution);
        if (list.material != null)
            foreach (GMStatic.requestMaterial current in list.material)
                ret.Add(current.solution);
        if (list.proximity != null)
            foreach (GMStatic.requestProximity current in list.proximity)
                ret.Add(current.solution);
        if (list.freeSpace != null)
            foreach (GMStatic.requestFreeSpace current in list.freeSpace)
                ret.Add(current.solution);

        return ret;
    }
    public List<bool> ReturnStatus(bool primary)
    {
        List<bool> ret = new List<bool>();

        GMStatic.Request list;
        if (primary) list = _primaryList;
        else list = _secondaryList;
        
        if(list.obj != null)
            foreach (GMStatic.requestObj current in list.obj)
                ret.Add(CheckObjRequest(current));
        if (list.usage != null)
            foreach (GMStatic.requestUsage current in list.usage)
                ret.Add(CheckTypeRequest(current));
        if (list.color != null)
            foreach (GMStatic.requestColor current in list.color)
                ret.Add(CheckColorRequest(current));
        if (list.material != null)
            foreach (GMStatic.requestMaterial current in list.material)
                ret.Add(CheckMaterialRequest(current));
        if (list.proximity != null)
            foreach (GMStatic.requestProximity current in list.proximity)
                ret.Add(CheckProximityRequest(current));
        if (list.freeSpace != null)
            foreach (GMStatic.requestFreeSpace current in list.freeSpace)
                ret.Add(CheckFreeSpaceRequest(current));

        return ret;
    }

    public bool CheckObjRequest(GMStatic.requestObj request)
    {
        bool res = false;

        List<int> resL = new List<int>(new int[request.itemRequested.Count]);
        int count = 0;
        foreach (ItemBehaviour item in TileSystem.Instance.ItemList)
        {
            if (request.needAll)
            {
                for (int i = 0; i < request.itemRequested.Count; i++)
                {
                    if (item.OwnItem == request.itemRequested[i])
                        resL[i]++;
                }

                if (resL.Count >= 1) count = resL[0];
                for (int c = 1; c < resL.Count; c++)
                    if (resL[c] < count) count = resL[c];
            }
            else
            {
                foreach (Item current in request.itemRequested)
                {
                    if (item.OwnItem == current)
                    {
                        count++; break;
                    }
                }
            }

            if (count >= request.nbRequested) res = true;
            if (res) break;
        }

        return res;
    }
    public bool CheckTypeRequest(GMStatic.requestUsage request)
    {
        bool res = false;

        List<int> resL = new List<int>(new int[request.usageRequested.Count]);

        int count = 0;
        foreach (ItemBehaviour item in TileSystem.Instance.ItemList)
        {
            if (request.needAll)
            {
                foreach (GMStatic.tagUsage usage in item.OwnItem.listUsage)
                {
                    for (int i = 0; i < request.usageRequested.Count; i++)
                    {
                        if (usage == request.usageRequested[i])
                        {
                            if (GMStatic.tagUsage.Bed == usage || GMStatic.tagUsage.Seat == usage) resL[i] += item.OwnItem.size.y;
                            else resL[i]++;
                        }
                    }
                }

                if (resL.Count >= 1) count = resL[0];
                for (int c = 1; c < resL.Count; c++)
                    if (resL[c] < count) count = resL[c];
            }
            else
            {
                foreach (GMStatic.tagUsage usage in item.OwnItem.listUsage)
                {
                    int buffer = count;
                    foreach (GMStatic.tagUsage current in request.usageRequested)
                    {
                        if (current == usage)
                        {
                            if (GMStatic.tagUsage.Bed == usage || GMStatic.tagUsage.Seat == usage)
                                count += item.OwnItem.size.y;
                            else
                                count++;

                            if (buffer != count) break;
                        }
                    }

                    if (buffer != count) break;
                }
            }

            if (count >= request.nbRequested) res = true;
            if (res) break;
        }

        return res;
    }
    public bool CheckColorRequest(GMStatic.requestColor request)
    {
        bool res = false;

        int count = 0;
        foreach (ItemBehaviour item in TileSystem.Instance.ItemList)
        {
            //print(item.ItemColor + "==" + request.colorRequested);
            Color colorToCheck = request.colorRequested;
            colorToCheck.a = 1;
            if (item.ItemColor == colorToCheck)
                count++;

            if (count >= request.nbRequested) res = true;
            if (res) break;
        }

        return res;
    }
    public bool CheckMaterialRequest(GMStatic.requestMaterial request)
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
    public bool CheckProximityRequest(GMStatic.requestProximity request)
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
            if (!request.isNotProxi)
            {
                if (count >= request.nbRequested) res = true;
            }
            else if (request.isNotProxi)
            {
                if (count < request.nbRequested) res = true;
            }

            if ((res && !request.isNotProxi) || (!res && request.isNotProxi)) break;
        }

        return res;
    }
    public bool CheckFreeSpaceRequest(GMStatic.requestFreeSpace request)
    {
        bool res = false;

        if (FreeTiles >= request.nbRequested)
            res = true;

        return res;
    }
}
