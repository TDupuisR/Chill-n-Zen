using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureWindowManager : MonoBehaviour
{
    [SerializeField] Button _openButton;
    [SerializeField] Button _closeButton;
    [SerializeField] WindowScroll _detailWindow;
    Coroutine _waitPlacementCoroutine;

    public static Action wasItemPlaced;

    private void OnEnable()
    {
        ItemSpawner.onItemSelected += CloseWindow;
        ItemSpawner.onItemTouched += AppearDetailWindow;
        ItemInput.OnCallDescription += AppearDetailWindowFromItem;
    }



    private void OnDisable()
    {
        ItemSpawner.onItemSelected -= CloseWindow;
        ItemSpawner.onItemTouched -= AppearDetailWindow;
        ItemInput.OnCallDescription -= AppearDetailWindowFromItem;
    }

    public void AppearWindow(bool scrollDetail = true)
    {
        _openButton.onClick.Invoke();

        //Hide detail panel bite
        DisplayDetailWindow(false);
    }

    void CloseWindow()
    {
        _closeButton.onClick.Invoke();
        _waitPlacementCoroutine = StartCoroutine(waitForObjectPlacement());
    }

    private void AppearDetailWindowFromItem(ItemBehaviour behaviour) 
    {
        CloseWindow();
        DisplayDetailWindow(true); 
    }
    public void AppearDetailWindow(Vector2 pos) => DisplayDetailWindow(true);
    public void DisplayDetailWindow(bool display)
    {
        if(_detailWindow.Displayed == !display)
            _detailWindow.StartScroll();
    }

    IEnumerator waitForObjectPlacement()
    {
        yield return new WaitUntil(() => TileSystem.Instance.IsSceneVacant);
        wasItemPlaced?.Invoke();
        AppearWindow();
    }
}