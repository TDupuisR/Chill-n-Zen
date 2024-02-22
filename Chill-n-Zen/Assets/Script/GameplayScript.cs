using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

public class GameplayScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Camera _mainCamera;
    [Foldout("Inputs")][SerializeField] InputActionReference _inputPrimaryTouch;
    [Foldout("Inputs")][SerializeField] InputActionReference _inputPrimaryPosition;
    [Foldout("Inputs")][SerializeField] InputActionReference _inputPrimaryDelta;
    [Space(4)]
    [Foldout("Inputs")][SerializeField] InputActionReference _inputSecondaryTouch;
    [Foldout("Inputs")][SerializeField] InputActionReference _inputSecondaryPosition;
    [Header("Camera Swipe Fields")]
    [SerializeField] bool _invertDirection;
    [SerializeField] float _swipeDeadZone;
    [SerializeField] float _swipeSpeed;
    [SerializeField] float _swipeMaxSpeed;
    [SerializeField] float _swipeDeceleration;
    Vector3 _swipeCurrentVelocity;
    Coroutine _swipeCoroutine;
    Coroutine _swipeDecelerationCoroutine;

    [Header("Camera Zoom Fields")]
    [SerializeField][MinMaxSlider(1f,100f)] Vector2 _minMaxZoom;
    [SerializeField] float _zoomSensitivity;
    Coroutine _zoomCoroutine;

    private void OnValidate()
    {
        if(_zoomSensitivity <= 0)
        {
            Debug.LogWarning("La sensibilité du Zoom ne peut pas être négative !");
            _zoomSensitivity = 1;
        }

        if (_swipeDeadZone <= 0)
        {
            Debug.LogWarning("swipeDeadZone ne peut pas être négative !");
            _zoomSensitivity = 1;
        }

        if (_swipeSpeed <= 0)
        {
            Debug.LogWarning("La vitesse du swipe ne peut pas être négative !");
            _zoomSensitivity = 1;
        }

        if (_swipeMaxSpeed <= 0)
        {
            Debug.LogWarning("swipeMaxSpeed ne peut pas être négative !");
            _zoomSensitivity = 1;
        }

        if (_swipeDeceleration <= 0)
        {
            Debug.LogWarning("swipeDeceleration ne peut pas être négative !");
            _zoomSensitivity = 1;
        }
    }

    private void OnEnable()
    {
        _inputPrimaryTouch.action.started += StartTouch;
        _inputPrimaryTouch.action.canceled += EndTouch;
        _inputSecondaryTouch.action.started += StartZoom;
        _inputSecondaryTouch.action.canceled += EndZoom;
    }

    private void OnDisable()
    {
        _inputPrimaryTouch.action.started -= StartTouch;
        _inputPrimaryTouch.action.canceled -= EndTouch;
        _inputSecondaryTouch.action.started -= StartZoom;
        _inputSecondaryTouch.action.canceled -= EndZoom;
    }

    /*
        Camera Swipe movement
    */

    private void StartTouch(InputAction.CallbackContext context)
    {

        //Start Swipe
        _swipeCoroutine = StartCoroutine(PerformSwipeRoutine());
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        //End Swipe
        if( _swipeCoroutine != null )
        {
            StopCoroutine(_swipeCoroutine);
            _swipeDecelerationCoroutine = StartCoroutine(DecelerationSwipeRoutine());
        }
    }

    IEnumerator PerformSwipeRoutine()
    {
        while (true)
        {
            Vector2 delta = _inputPrimaryDelta.action.ReadValue<Vector2>();
            float force = delta.magnitude;
            
            if (force > _swipeDeadZone)
            {

                Vector3 direction = (_invertDirection ? -1 : 1) * delta.normalized;
                direction.z = 0;

                //Clamp magnitude of velocity
                _swipeCurrentVelocity = force * _swipeSpeed * direction * Time.fixedDeltaTime;
                _swipeCurrentVelocity = _swipeCurrentVelocity.normalized * Mathf.Clamp(_swipeCurrentVelocity.magnitude, -_swipeMaxSpeed, _swipeMaxSpeed);
             
                //Apply velocity to position
                _mainCamera.transform.position += _swipeCurrentVelocity;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator DecelerationSwipeRoutine()
    {
        Vector3 _decelerationDirection = _swipeCurrentVelocity.normalized;
        float _decelerationMagnitude = _swipeCurrentVelocity.magnitude;
        while (_decelerationMagnitude >= 0)
        {
            _mainCamera.transform.position += _decelerationDirection * _decelerationMagnitude * Time.fixedDeltaTime;
            _decelerationMagnitude -= _swipeDeceleration * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    /*
        Camera zoom pitch
     */

    private void StartZoom(InputAction.CallbackContext context)
    {
        _zoomCoroutine = StartCoroutine(ZoomRoutine());
    }

    private void EndZoom(InputAction.CallbackContext context)
    {
        if(_zoomCoroutine != null )
        {
            StopCoroutine(_zoomCoroutine);
        }
    }

    IEnumerator ZoomRoutine()
    {
        float currentDistance = Vector2.Distance(_inputPrimaryPosition.action.ReadValue<Vector2>(), _inputSecondaryPosition.action.ReadValue<Vector2>());
        float oldDistance = currentDistance;

        while (true)
        {
            currentDistance = Vector2.Distance(_inputPrimaryPosition.action.ReadValue<Vector2>(), _inputSecondaryPosition.action.ReadValue<Vector2>());
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
