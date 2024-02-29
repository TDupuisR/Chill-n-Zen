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
    [Space(4)]
    [Foldout("Inputs")][SerializeField] InputActionReference _inputSecondaryTouch;
    [Foldout("Inputs")][SerializeField] InputActionReference _inputSecondaryPosition;


    [Header("Swipe Fields")]
    [SerializeField] bool _invertDirection;
    [SerializeField] float _swipeSpeed;
    [SerializeField] float _swipeMaxSpeed;
    [SerializeField] float _swipeDeceleration;
    Vector2 _swipeLastPosition;
    Vector3 _swipeCurrentVelocity;
    Coroutine _swipeCoroutine;

    [Header("Hold Fields")]
    [SerializeField] float _durationToLongPress;
    [SerializeField] float _durationToHold;
    bool _islongPress = false;
    bool _ishold = false;
    bool _isPrimaryPressed = false;
    bool _isSecondaryPressed = false;
    Coroutine _longPressCoroutine;
    Coroutine _holdCoroutine;

    public bool IsPrimaryPressed { get => _isPrimaryPressed; }
    public bool IsSecondaryPressed { get => _isSecondaryPressed; }
    public bool IsHold { get => _ishold; }
    public float SwipeDeceleration { get => _swipeDeceleration; }
    public bool IsLongPress { get => _islongPress; }

    public Vector2 PrimaryPosition { get => _inputPrimaryPosition.action.ReadValue<Vector2>(); }
    public Vector2 SecondaryPosition { get => _inputSecondaryPosition.action.ReadValue<Vector2>(); }
    public Vector2 MouseWorldPosition { get => Camera.main.ScreenToWorldPoint(PrimaryPosition); }

    public static Action<Vector2> onStartPrimaryTouch;
    public static Action<Vector2> onEndSecondaryTouch;
    public static Action<Vector2> onStartSecondaryTouch;
    public static Action<Vector2> onEndPrimaryTouch;
    public static Action<Vector2> onSwipe;

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

        if (_swipeSpeed <= 0)
        {
            Debug.LogWarning("La vitesse du swipe ne peut pas �tre n�gative ou nulle !");
            _swipeSpeed = 1;
        }

        if (_swipeMaxSpeed <= 0)
        {
            Debug.LogWarning("swipeMaxSpeed ne peut pas �tre n�gative ou nulle !");
            _swipeMaxSpeed = 1;
        }

        if (_swipeDeceleration <= 0)
        {
            Debug.LogWarning("swipeDeceleration ne peut pas �tre n�gative ou nulle !");
            _swipeDeceleration = 1;
        }

        if(_durationToLongPress > _durationToHold)
        {
            Debug.LogWarning("duration to long Press can't be more than the duration to Hold !");
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
        onStartPrimaryTouch?.Invoke(_inputPrimaryPosition.action.ReadValue<Vector2>());
        //Start LongPress & hold coroutine
        _longPressCoroutine = StartCoroutine(LongPressRoutine(_durationToLongPress));
        _holdCoroutine = StartCoroutine(HoldRoutine(_durationToHold));
        _isPrimaryPressed = true;
    }

    private void EndPrimaryTouch(InputAction.CallbackContext context)
    {
        onEndPrimaryTouch?.Invoke(_inputPrimaryPosition.action.ReadValue<Vector2>());

        //End Swipe
        if ( _swipeCoroutine != null)
        {
            StopCoroutine(_swipeCoroutine);
        }

        //End hold
        StopLongPressCheck();
        StopHoldCheck();

        _isPrimaryPressed = false;
    }

    private void StartSecondaryTouch(InputAction.CallbackContext context)
    {
        onStartSecondaryTouch?.Invoke(_inputSecondaryPosition.action.ReadValue<Vector2>());
        
        //End single touch hold
        StopHoldCheck();

        _isSecondaryPressed = true;
    }

    private void EndSecondaryTouch(InputAction.CallbackContext context)
    {
        onEndSecondaryTouch?.Invoke(_inputSecondaryPosition.action.ReadValue<Vector2>());
        _isSecondaryPressed = false;
    }

    IEnumerator PerformSwipeRoutine()
    {
        _swipeLastPosition = _inputPrimaryPosition.action.ReadValue<Vector2>();
            
        while (true)
        {
            Vector2 currentPosition = _inputPrimaryPosition.action.ReadValue<Vector2>();
            Vector2 delta = currentPosition - _swipeLastPosition;
            float force = delta.magnitude;

            if (force > 0)
            {
                Vector3 direction = (_invertDirection ? -1 : 1) * delta.normalized;
                direction.z = 0;

                //Clamp magnitude of velocity
                _swipeCurrentVelocity = force * _swipeSpeed * direction * Time.fixedDeltaTime;
                _swipeCurrentVelocity = _swipeCurrentVelocity.normalized * Mathf.Clamp(_swipeCurrentVelocity.magnitude, -_swipeMaxSpeed, _swipeMaxSpeed);

                //Apply velocity to position
                onSwipe?.Invoke(_swipeCurrentVelocity);

                _swipeLastPosition = currentPosition;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    /*
     Hold & Long Press
     */

    void StopLongPressCheck()
    {
        _islongPress = false;
        if (_longPressCoroutine != null)
        {
            StopCoroutine(_longPressCoroutine);
        }
    }
    IEnumerator LongPressRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        print("longpress");
        _islongPress = true;
        //Start Swipe
        _swipeCoroutine = StartCoroutine(PerformSwipeRoutine());
    }
    void StopHoldCheck()
    {
        _ishold = false;
        if (_holdCoroutine != null)
        {
            StopCoroutine(_holdCoroutine);
        }
    }
    IEnumerator HoldRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        print("hold");
        _ishold = true;
        _islongPress = false;
    }
}
