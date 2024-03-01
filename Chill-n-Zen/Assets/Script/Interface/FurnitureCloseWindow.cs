using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureCloseWindow : MonoBehaviour
{
    [SerializeField] Button _closeButton;

    private void OnEnable()
    {
        ItemSpawner.onItemSelected += CloseWindow;
    }

    private void OnDisable()
    {
        ItemSpawner.onItemSelected -= CloseWindow;
    }


    void CloseWindow()
    {
        _closeButton.onClick.Invoke();
    }
}
