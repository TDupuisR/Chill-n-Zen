using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartPopUp : MonoBehaviour
{
    [SerializeField] UIGetCam _cam;
    [SerializeField] GameObject _restartWindow;

    public void ShowPopUp()
    {
        _restartWindow.SetActive(true);
        _cam.cam.canMoveCamera = false;
    }

    public void HidePopUp()
    {
        _restartWindow.SetActive(false);
        _cam.cam.canMoveCamera = true;
    }
}
