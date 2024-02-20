using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class GameplayScript : MonoBehaviour
{
    [Header("Camera Swipe Reference")]
    [SerializeField] Camera _mainCamera;
    [SerializeField] InputActionReference _inputTouch;
    [SerializeField] InputActionReference _inputPosition;
    [Header("Camera Swipe Variables")]
    [SerializeField] float _swipeDeadZone;
    [SerializeField] float _swipeSpeed;
    [SerializeField] float _swipeMaxSpeed;
    [SerializeField] float _swipeDeceleration;
    //Coroutine
    Coroutine _swipeCoroutine;
    Coroutine _swipeDecelerationCoroutine;

    Vector2 _initialPosition;
    Vector3 _swipeDirection;
    float _swipeForce;

    private void OnEnable()
    {
        _inputTouch.action.started += StartSwipe;
        _inputTouch.action.canceled += EndSwipe;
    }


    private void OnDisable()
    {
        _inputTouch.action.started -= StartSwipe;
        _inputTouch.action.canceled -= EndSwipe;
    }

    private void StartSwipe(InputAction.CallbackContext context)
    {
        _initialPosition = _inputPosition.action.ReadValue<Vector2>();
        _swipeCoroutine = StartCoroutine(PerformSwipeRoutine());
    }

    private void EndSwipe(InputAction.CallbackContext context)
    {
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
            Vector2 currentPosition = _inputPosition.action.ReadValue<Vector2>();
            Vector2 delta = currentPosition - _initialPosition;
            _swipeForce = delta.magnitude;
            
            if (_swipeForce > _swipeDeadZone)
            {

                _swipeDirection = -delta.normalized;
                _swipeDirection.z = 0;

                //Clamp magnitude of velocity
                Vector3 velocity = _swipeForce * _swipeSpeed * _swipeDirection * Time.fixedDeltaTime;
                float velocityMagnitude = Mathf.Clamp(velocity.magnitude, -_swipeMaxSpeed, _swipeMaxSpeed);
                Vector3 velocityDirection = velocity.normalized;
                velocity = velocityDirection * velocityMagnitude;
             
                //Apply velocity to position
                print(velocity);
                _mainCamera.transform.position += velocity;

                _initialPosition = currentPosition;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator DecelerationSwipeRoutine()
    {
        while (_swipeForce >= 0)
        {
            print(_swipeForce * _swipeSpeed * _swipeDirection * Time.fixedDeltaTime);

            _mainCamera.transform.position += _swipeForce * _swipeDirection * Time.fixedDeltaTime;
            _swipeForce -= _swipeDeceleration * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

}
