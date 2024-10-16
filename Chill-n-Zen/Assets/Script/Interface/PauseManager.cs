using GameManagerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;

public class PauseManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject _pauseObject;
    [SerializeField] GameObject _restartWindow;
    [SerializeField] List<GameObject> _buttonToHide;
    [SerializeField] InputActionReference _pauseInput;

    private void OnEnable()
    {
        _pauseInput.action.started += DisplayPauseInput;
    }
    private void OnDisable()
    {
        _pauseInput.action.started -= DisplayPauseInput;
    }

    private void DisplayPauseInput(InputAction.CallbackContext context) => DisplayPause(!_pauseObject.activeSelf);

    public void DisplayPause(bool activate)
    {
        //Hide Gameplay buttons
        foreach (GameObject button in _buttonToHide)
        {
            button.SetActive(!activate);
        }

        _pauseObject.SetActive(activate);
    }

    public void ShowRestartPopUp(bool active) { _restartWindow.SetActive(active); }
    public void RestartLevel() => GameManager.Instance.ChangeScene(SceneManager.GetActiveScene().buildIndex);

    public void BackToMainMenu()
    {
        GameManager.Instance.ChangeScene(0);
    }
}
