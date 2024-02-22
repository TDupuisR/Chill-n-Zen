using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CameraControls : MonoBehaviour
{
    [SerializeField] Camera _mainCamera;

    //[Header("Camera Movement")]
    Vector2 _cameraActionZonePoint1; //Upper left
    Vector2 _cameraActionZonePoint2; //Lower Right
    Vector4 _defaultCameraActionZone;
    Coroutine _CameraDecelerationCoroutine;

    [Header("Camera Zoom Fields")]
    [SerializeField][MinMaxSlider(1f, 100f)] Vector2 _minMaxZoom;
    [SerializeField] float _zoomSensitivity;
    Coroutine _zoomCoroutine;
    bool _isMovingCamera;

    private void Awake()
    {
        //Define camera action Zone
        _cameraActionZonePoint1 = new Vector2(0, 0);
        _cameraActionZonePoint2 = new Vector2(Screen.width, Screen.height);
        _defaultCameraActionZone = new Vector4(_cameraActionZonePoint1.x, _cameraActionZonePoint1.y, _cameraActionZonePoint2.x, _cameraActionZonePoint2.y);
    }

    private void OnEnable()
    {
        GameplayScript._onSwipe += CameraMovement;
    }

    private void OnDisable()
    {
        GameplayScript._onSwipe -= CameraMovement;

    }

    private void OnValidate()
    {
        if (_zoomSensitivity <= 0)
        {
            Debug.LogWarning("La sensibilité du Zoom ne peut pas être négative !");
            _zoomSensitivity = 1;
        }
    }

    void EndCamMovement(Vector3 lastVelocity)
    {
        //Perform deceleration routine if cam is moving
        if (_isMovingCamera)
        {
            _isMovingCamera = false;
            _CameraDecelerationCoroutine = StartCoroutine(DecelerationCameraRoutine(lastVelocity, GameplayScript.Instance.swipeDeceleration));
        }
    }



    /*
       Camera movement
     */

    void CameraMovement(Vector2 velocity, Vector2 pointerPosition)
    {
        if (IsTouchInCameraActionZone(pointerPosition))
        {
            Vector3 velocityV3 = new Vector3(velocity.x, velocity.y, 0.0f);
            _mainCamera.transform.position += velocityV3;
            _isMovingCamera = true;
        }
    }
    IEnumerator DecelerationCameraRoutine(Vector3 lastVelocity, float deceleration)
    {
        Vector3 _decelerationDirection = lastVelocity.normalized;
        float _decelerationMagnitude = lastVelocity.magnitude;
        while (_decelerationMagnitude >= 0)
        {
            _mainCamera.transform.position += _decelerationDirection * _decelerationMagnitude * Time.fixedDeltaTime;
            _decelerationMagnitude -= deceleration * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    bool IsTouchInCameraActionZone(Vector2 pointerPosition)
    {
        Vector2 touchPosition = pointerPosition;
        return touchPosition.x > _cameraActionZonePoint1.x && touchPosition.x < _cameraActionZonePoint2.x &&
               touchPosition.y > _cameraActionZonePoint1.y && touchPosition.y < _cameraActionZonePoint2.y;
    }

    public void ChangeCameraActionZone(bool firstPoint, Vector2 newPosition)
    {
        if (firstPoint) _cameraActionZonePoint1 = newPosition;
        else _cameraActionZonePoint2 = newPosition;
    }

    public void ResetCameraActionZone()
    {
        _cameraActionZonePoint1 = new Vector2(_defaultCameraActionZone.x, _defaultCameraActionZone.y);
        _cameraActionZonePoint2 = new Vector2(_defaultCameraActionZone.z, _defaultCameraActionZone.w);

    }

    /*
        Camera zoom pitch
     */

    private void StartZoom()
    {
        _zoomCoroutine = StartCoroutine(ZoomRoutine());
    }

    private void EndZoom()
    {
        if (_zoomCoroutine != null)
        {
            StopCoroutine(_zoomCoroutine);
        }
    }

    IEnumerator ZoomRoutine()
    {
        Vector2 primaryPosition = GameplayScript.Instance.PrimaryPosition;
        Vector2 secondaryPosition = GameplayScript.Instance.SecondaryPosition;

        float currentDistance = Vector2.Distance(primaryPosition, secondaryPosition);
        float oldDistance = currentDistance;

        while (true)
        {
            primaryPosition = GameplayScript.Instance.PrimaryPosition;
            secondaryPosition = GameplayScript.Instance.SecondaryPosition;

            currentDistance = Vector2.Distance(primaryPosition, secondaryPosition);
            if (currentDistance != oldDistance)
            {
                float distanceDelta = oldDistance - currentDistance;

                float newCameraSize = _mainCamera.orthographicSize;
                newCameraSize += distanceDelta * _zoomSensitivity;
                newCameraSize = Mathf.Clamp(newCameraSize, _minMaxZoom.x, _minMaxZoom.y);
                _mainCamera.orthographicSize = newCameraSize;

                oldDistance = currentDistance;
            }


            yield return new WaitForFixedUpdate();
        }
    }
}
