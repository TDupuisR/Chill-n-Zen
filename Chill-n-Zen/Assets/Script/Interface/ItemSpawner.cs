using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSpawner : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] GameObject _itemPrefab;
    [SerializeField] FurnitureReadData _data;
    Coroutine _waitHoldRoutine;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (TileSystem.Instance.IsSceneVacant)
        {
            _waitHoldRoutine = StartCoroutine(WaitForHold());
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if(_waitHoldRoutine != null)
        {
            StopCoroutine(_waitHoldRoutine);
        }
    }

    IEnumerator WaitForHold()
    {
        bool checking = true;
        while (checking)
        {
            if (GameplayScript.Instance.IsHold)
            {
                GameObject spawnedItem = Instantiate(_itemPrefab);
                spawnedItem.GetComponent<ItemBehaviour>().Initialize(_data.Furniture);
                TileSystem.Instance.ObjectOnScene(false);
                checking = false;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
