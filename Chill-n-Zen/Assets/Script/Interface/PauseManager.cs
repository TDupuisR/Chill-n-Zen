using GameManagerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject _pauseObject;
    [SerializeField] List<GameObject> _buttonToHide;

    public void DisplayPause(bool activate)
    {
        //Hide Gameplay buttons
        foreach (GameObject button in _buttonToHide)
        {
            button.SetActive(!activate);
        }

        _pauseObject.SetActive(activate);
    }

    public void BackToMainMenu()
    {
        GameManager.Instance.ChangeScene(0);
    }
}
