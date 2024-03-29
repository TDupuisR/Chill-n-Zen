using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

public class CameraControls : MonoBehaviour
{
    public static CameraControls Instance;
    [SerializeField] Camera _mainCamera;

    [Header("Camera Movement")]
    [SerializeField] float _maxLastVelocity;
    [SerializeField] [Foldout("Boundaries")] float _topCamBoundaries;
    [SerializeField] [Foldout("Boundaries")] float _bottomCamBoundaries;
    [SerializeField] [Foldout("Boundaries")] float _rightCamBoundaries;
    [SerializeField] [Foldout("Boundaries")] float _leftCamBoundaries;
    
    Vector2 _lastVelocity = Vector2.zero;
    Vector2 _cameraActionZonePointDL; //Upper left
    Vector2 _cameraActionZonePointUR; //Lower Right
    Vector4 _defaultCameraActionZone;
    Coroutine _CameraDecelerationCoroutine;
    bool _isMovingCamera;
    bool _isInActionZone;

    [Header("Camera Zoom Fields")]
    [SerializeField][MinMaxSlider(1f, 100f)] Vector2 _minMaxZoom;
    [SerializeField] float _zoomSensitivity;
    Coroutine _zoomCoroutine;

    public bool CanMoveCamera { get; set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError(" (error : 1x0) Too many CameraControls instance ", gameObject);
            Destroy(gameObject);
        }
        Instance = this;

        //Define camera action Zone
        _cameraActionZonePointDL = new Vector2(0, 0);
        _cameraActionZonePointUR = new Vector2(Screen.width, Screen.height);
        _defaultCameraActionZone = new Vector4(_cameraActionZonePointDL.x, _cameraActionZonePointDL.y, _cameraActionZonePointUR.x, _cameraActionZonePointUR.y);

        CanMoveCamera = true;
    }

    private void OnEnable()
    {
        GameplayScript.onStartPrimaryTouch += ChkValidMovement;
        GameplayScript.onSwipe += CameraMovement;
        GameplayScript.onEndPrimaryTouch += EndCamMovement;
        GameplayScript.onStartSecondaryTouch += StartZoom;
        GameplayScript.onEndSecondaryTouch += EndZoom;
    }

    private void OnDisable()
    {
        GameplayScript.onStartPrimaryTouch -= ChkValidMovement;
        GameplayScript.onSwipe -= CameraMovement;
        GameplayScript.onEndPrimaryTouch -= EndCamMovement;
        GameplayScript.onStartSecondaryTouch -= StartZoom;
        GameplayScript.onEndSecondaryTouch -= EndZoom;
    }

    private void OnValidate()
    {
        if (_zoomSensitivity <= 0)
        {
            Debug.LogWarning("La sensibilit� du Zoom ne peut pas �tre n�gative !");
            _zoomSensitivity = 1;
        }
    }



    /*
       Camera movement
     */

    private void ChkValidMovement(Vector2 vector)
    {
        _isInActionZone = IsTouchInCameraActionZone(GameplayScript.Instance.PrimaryPosition);
    }

    void CameraMovement(Vector2 velocity)
    {
        if (_isInActionZone && CanMoveCamera && GameplayScript.Instance.IsLongPress && IsTouchInCameraActionZone(GameplayScript.Instance.PrimaryPosition))
        {
            Vector3 velocityV3 = new Vector3(velocity.x, velocity.y, 0.0f);
            _mainCamera.transform.position += velocityV3;
            _mainCamera.transform.position = ClampCamPosition(_mainCamera.transform.position);

            _lastVelocity = velocityV3;
            _isMovingCamera = true;
        }
    }

    void EndCamMovement(Vector2 lastVelocity)
    {
        //Perform deceleration routine if cam is moving
        if (_isMovingCamera)
        {
            _isMovingCamera = false;
            _CameraDecelerationCoroutine = StartCoroutine(DecelerationCameraRoutine(_lastVelocity, GameplayScript.Instance.SwipeDeceleration));
        }
    }

    IEnumerator DecelerationCameraRoutine(Vector3 lastVelocity, float deceleration)
    {
        Vector3 _decelerationDirection = lastVelocity.normalized;
        float _decelerationMagnitude = Mathf.Clamp(lastVelocity.magnitude, 0, _maxLastVelocity);
        while (_decelerationMagnitude >= 0)
        {
            _mainCamera.transform.position += _decelerationDirection * _decelerationMagnitude * Time.fixedDeltaTime;
            _mainCamera.transform.position = ClampCamPosition(_mainCamera.transform.position);

            _decelerationMagnitude -= deceleration * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    bool IsTouchInCameraActionZone(Vector2 touchPosition)
    {
        //print(_cameraActionZonePointDL + " ; " + _cameraActionZonePointUR + " - mouse : " + touchPosition);
        return touchPosition.x > _cameraActionZonePointDL.x && touchPosition.x < _cameraActionZonePointUR.x &&
               touchPosition.y > _cameraActionZonePointDL.y && touchPosition.y < _cameraActionZonePointUR.y;
    }

    Vector3 ClampCamPosition(Vector2 position)
    {
        float clampedX = Mathf.Clamp(position.x, _leftCamBoundaries, _rightCamBoundaries);
        float clampedY = Mathf.Clamp(position.y, _bottomCamBoundaries, _topCamBoundaries);
        return new Vector3(clampedX, clampedY, _mainCamera.transform.position.z);
    }

    /*
        Camera zoom pitch
     */

    private void StartZoom(Vector2 lastPosition)
    {
        if (IsTouchInCameraActionZone(GameplayScript.Instance.PrimaryPosition) && IsTouchInCameraActionZone(GameplayScript.Instance.SecondaryPosition))
        {
            _zoomCoroutine = StartCoroutine(ZoomRoutine());
        }
    }

    private void EndZoom(Vector2 lastPosition)
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



    /*
        Camera Zone Action
     */
    public void ChangeDownLeftCamActionZone(Vector2 newPosition)
    {
        _cameraActionZonePointDL = newPosition;
    }
    public void ChangeUpRightCamActionZone(Vector2 newPosition)
    {
        _cameraActionZonePointUR = newPosition;
    }
    public void ResetDownLeftCamActionZone()
    {
        _cameraActionZonePointDL = new Vector2(_defaultCameraActionZone.x, _defaultCameraActionZone.y);

    }
    public void ResetUpRightCamActionZone()
    {
        _cameraActionZonePointUR = new Vector2(_defaultCameraActionZone.z, _defaultCameraActionZone.w);
    }
    public void ResetCameraActionZone()
    {
        ResetDownLeftCamActionZone();
        ResetUpRightCamActionZone();
    }
}
