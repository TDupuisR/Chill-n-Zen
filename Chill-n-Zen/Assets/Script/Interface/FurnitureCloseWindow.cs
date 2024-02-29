using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureCloseWindow : MonoBehaviour
{
    [SerializeField] Button _closeButton;

    private void OnEnable()
    {
        ItemSpawner._onItemSelected += CloseWindow;
    }

    private void OnDisable()
    {
        ItemSpawner._onItemSelected -= CloseWindow;
    }


    void CloseWindow()
    {
        _closeButton.onClick.Invoke();
    }
}
