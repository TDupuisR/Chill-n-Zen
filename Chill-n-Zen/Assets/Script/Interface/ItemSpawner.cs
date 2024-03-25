using GameManagerSpace;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSpawner : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] GameObject _itemPrefab;
    [SerializeField] FurnitureReadData _data;
    Coroutine _waitHoldRoutine;

    public Transform ObjectParent { get; set; }
    public FurnitureReadData DetailWindow { get; set; }
    public SwipeScrollbar Scrollbar { get; set; }

    public static Action<Vector2> onItemTouched;
    public static Action onItemSelected;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (TileSystem.Instance.IsSceneVacant)
        {
            _waitHoldRoutine = StartCoroutine(WaitForHold());
            onItemTouched?.Invoke(transform.position);
            DetailWindow.Furniture = _data.Furniture;
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
            if (GameManager.gameplayScript.IsHold && !Scrollbar.IsScrolling)
            {
                GameObject spawnedItem = Instantiate(_itemPrefab, ObjectParent);
                spawnedItem.GetComponent<ItemBehaviour>().Initialize(_data.Furniture);
                TileSystem.Instance.ObjectOnScene(false);

                onItemSelected?.Invoke();
                checking = false;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
