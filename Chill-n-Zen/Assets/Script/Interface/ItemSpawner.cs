using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSpawner : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] GameObject _itemPrefab;
    [SerializeField] FurnitureReadData _data;
    Coroutine _waitHoldRoutine;

    public void OnPointerDown(PointerEventData eventData)
    {
        print("pointerDown");
        _waitHoldRoutine = StartCoroutine(WaitForHold());
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        print("pointerUp");
        StopCoroutine(_waitHoldRoutine);
    }

    IEnumerator WaitForHold()
    {
        bool checking = true;
        while (checking)
        {
            if (GameplayScript.Instance.IsHold)
            {
                print("spawn");
                GameObject spawnedItem = Instantiate(_itemPrefab);
                spawnedItem.GetComponent<ItemBehaviour>().Initialize(_data.Furniture);
                
                checking = false;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
