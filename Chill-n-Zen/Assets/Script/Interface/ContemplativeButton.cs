using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContemplativeButton : MonoBehaviour
{
    [SerializeField] GameObject _gameplayObjects;
    [SerializeField] Button _button;
    bool _active = false;

    private void OnEnable()
    {
        ItemSpawner.onItemSelected += DisableButton;
        FurnitureWindowManager.wasItemPlaced += EnableButton;
    }
    private void OnDisable()
    {
        ItemSpawner.onItemSelected -= DisableButton;
        FurnitureWindowManager.wasItemPlaced -= EnableButton;
    }

    void EnableButton() { _button.interactable = true; }
    void DisableButton() { _button.interactable = false; }

    public void Activate()
    {
        _gameplayObjects.SetActive(_active);
        _active = !_active;
    }
}
