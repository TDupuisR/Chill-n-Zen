using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureWindowManager : MonoBehaviour
{
    [SerializeField] ObjectivesUI _objectivesUI;
    [SerializeField] Button _openButton;
    [SerializeField] Button _closeButton;
    [SerializeField] WindowScroll _detailWindow;
    [SerializeField] FurnitureReadData _detailData;

    Coroutine _waitPlacementCoroutine;

    public static Action wasItemPlaced;

    private void OnEnable()
    {
        ItemSpawner.onItemSelected += CloseWindow;
        ItemSpawner.onItemTouched += AppearDetailWindow;

        ItemInput.OnCallDescription += AppearDetailWindowFromItem;
        ItemInput.OnCallHideDescription += HideDetailWindow;
    }



    private void OnDisable()
    {
        ItemSpawner.onItemSelected -= CloseWindow;
        ItemSpawner.onItemTouched -= AppearDetailWindow;

        ItemInput.OnCallDescription -= AppearDetailWindowFromItem;
        ItemInput.OnCallHideDescription -= HideDetailWindow;
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
        _detailData.Furniture = behaviour.OwnItem;
        AppearDetailWindow(Vector2.zero);
    }
    public void AppearDetailWindow(Vector2 pos) => DisplayDetailWindow(true);
    public void HideDetailWindow() => DisplayDetailWindow(false);
    public void DisplayDetailWindow(bool display)
    {
        if(_detailWindow.Displayed == !display)
            _detailWindow.StartScroll();
    }

    IEnumerator waitForObjectPlacement()
    {
        CameraControls.Instance.CanMoveCamera = false;
        _objectivesUI.ActivateObjButton(false);
        yield return new WaitUntil(() => TileSystem.Instance.IsSceneVacant);
        CameraControls.Instance.CanMoveCamera = true;
        _objectivesUI.ActivateObjButton(true);
        wasItemPlaced?.Invoke();
        AppearWindow();
    }
}