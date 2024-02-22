using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

public class GameplayScript : MonoBehaviour
{
    public static GameplayScript Instance;

    [Foldout("Inputs")][SerializeField] InputActionReference _inputPrimaryTouch;
    [Foldout("Inputs")][SerializeField] InputActionReference _inputPrimaryPosition;
    [Foldout("Inputs")][SerializeField] InputActionReference _inputPrimaryDelta;
    [Space(4)]
    [Foldout("Inputs")][SerializeField] InputActionReference _inputSecondaryTouch;
    [Foldout("Inputs")][SerializeField] InputActionReference _inputSecondaryPosition;

    [Header("Swipe Fields")]
    [SerializeField] bool _invertDirection;
    [SerializeField] float _swipeDeadZone;
    [SerializeField] float _swipeSpeed;
    [SerializeField] float _swipeMaxSpeed;
    [SerializeField] float _swipeDeceleration;
    Vector3 _swipeCurrentVelocity;
    Coroutine _swipeCoroutine;

    public float swipeDeceleration
    {
        get => _swipeDeceleration;
    }

    public Vector2 PrimaryPosition
    {
        get => _inputPrimaryPosition.action.ReadValue<Vector2>();
    }

    public Vector2 SecondaryPosition
    {
        get => _inputSecondaryPosition.action.ReadValue<Vector2>();
    }

    public static Action<Vector2> _onStartPrimaryTouch;
    public static Action<Vector2> _onEndSecondaryTouch;
    public static Action<Vector2> _onStartSecondaryTouch;
    public static Action<Vector2> _onEndPrimaryTouch;
    public static Action<Vector2> _onSwipe;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    private void OnValidate()
    {

        if (_swipeDeadZone <= 0)
        {
            Debug.LogWarning("swipeDeadZone ne peut pas �tre n�gative !");
            _swipeDeadZone = 1;
        }

        if (_swipeSpeed <= 0)
        {
            Debug.LogWarning("La vitesse du swipe ne peut pas �tre n�gative !");
            _swipeSpeed = 1;
        }

        if (_swipeMaxSpeed <= 0)
        {
            Debug.LogWarning("swipeMaxSpeed ne peut pas �tre n�gative !");
            _swipeMaxSpeed = 1;
        }

        if (_swipeDeceleration <= 0)
        {
            Debug.LogWarning("swipeDeceleration ne peut pas �tre n�gative !");
            _swipeDeceleration = 1;
        }
    }

    private void OnEnable()
    {
        _inputPrimaryTouch.action.started += StartPrimaryTouch;
        _inputPrimaryTouch.action.canceled += EndPrimaryTouch;
        _inputSecondaryTouch.action.started += StartSecondaryTouch;
        _inputSecondaryTouch.action.canceled += EndSecondaryTouch;
    }

    private void OnDisable()
    {
        _inputPrimaryTouch.action.started -= StartPrimaryTouch;
        _inputPrimaryTouch.action.canceled -= EndPrimaryTouch;
        _inputSecondaryTouch.action.started -= StartSecondaryTouch;
        _inputSecondaryTouch.action.canceled -= EndSecondaryTouch;

    }

    /*
        Swipe movement
    */

    private void StartPrimaryTouch(InputAction.CallbackContext context)
    {
        _onStartPrimaryTouch?.Invoke(_inputPrimaryPosition.action.ReadValue<Vector2>());
        
        //Start Swipe
        _swipeCoroutine = StartCoroutine(PerformSwipeRoutine());
    }

    private void EndPrimaryTouch(InputAction.CallbackContext context)
    {
        _onEndPrimaryTouch?.Invoke(_inputPrimaryPosition.action.ReadValue<Vector2>());

        //End Swipe
        if ( _swipeCoroutine != null)
        {
            StopCoroutine(_swipeCoroutine);
        }
    }

    private void StartSecondaryTouch(InputAction.CallbackContext context)
    {
        _onStartSecondaryTouch?.Invoke(_inputSecondaryPosition.action.ReadValue<Vector2>());
    }

    private void EndSecondaryTouch(InputAction.CallbackContext context)
    {
        _onEndSecondaryTouch?.Invoke(_inputSecondaryPosition.action.ReadValue<Vector2>());
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
                _onSwipe?.Invoke(_swipeCurrentVelocity);
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
