using GameManagerSpace;
using System.Collections.Generic;
using UnityEngine;

public class RequestManager : MonoBehaviour
{
    GMStatic.Request _primaryList;
    List<bool> _primaryCheck;
    GMStatic.Request _secondaryList;
    List<bool> _secondaryCheck;


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
    private bool CheckTypeRequest(GMStatic.requestType request)
    {
        bool res = false;

        int count = 0;
        foreach (ItemBehaviour item in TileSystem.Instance.ItemList)
        {
            foreach (GMStatic.tagUsage usage in item.OwnItem.listUsage)
            {
                if (usage == request.typeRequested)
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
}
