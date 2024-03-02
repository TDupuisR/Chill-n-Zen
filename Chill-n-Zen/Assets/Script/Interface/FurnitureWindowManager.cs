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

    private void OnEnable()
    {
        ItemSpawner.onItemSelected += CloseWindow;
    }

    private void OnDisable()
    {
        ItemSpawner.onItemSelected -= CloseWindow;
    }

    public void AppearWindow(bool scrollDetail = true)
    {
        _openButton.onClick.Invoke();
        if(scrollDetail) 
            _detailWindow.StartScroll();
    }

    void CloseWindow()
    {
        _closeButton.onClick.Invoke();
        _detailWindow.StartScroll();
        _waitPlacementCoroutine = StartCoroutine(waitForObjectPlacement());
    }

    IEnumerator waitForObjectPlacement()
    {
        yield return new WaitUntil(() => TileSystem.Instance.IsSceneVacant);
        AppearWindow();
        StartCoroutine(waitForTakingObject());
    }

    IEnumerator waitForTakingObject()
    {
        yield return new WaitWhile(() => TileSystem.Instance.IsSceneVacant);
        CloseWindow();
    }
}
