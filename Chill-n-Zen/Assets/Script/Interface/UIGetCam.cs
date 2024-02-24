using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGetCam : MonoBehaviour
{
    [SerializeField] CameraControls _cam;

    public CameraControls cam
    {
        get => _cam;
    }

    private void Awake()
    {
        if(_cam == null)
        {
            Debug.LogError("Missing camera in UI GET CAM");
        }
    }

    public void ResetLowerLeft()
    {
        _cam.ResetDownLeftCamActionZone();
    }

    public void ResetUpperRight()
    {
        _cam.ResetUpRightCamActionZone();
    }
}
