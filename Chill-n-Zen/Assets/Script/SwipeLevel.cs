using GameManagerSpace;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class SwipeLevel : MonoBehaviour
{

    #region Variable 
    [Foldout("Inputs")][SerializeField] InputActionReference _inputPrimaryTouch;
    [Foldout("Inputs")][SerializeField] InputActionReference _inputPrimaryPosition;
    [Foldout("Inputs")][SerializeField] InputActionReference _inputBackButton;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private float _speedSlider = 1;
    [SerializeField] private float _endSlideSpeed = 1;
    private List<RectTransform> _listPicture = new List<RectTransform>();
    private RectTransform _tempRectTransform = null;
    private float _distance = Mathf.Infinity;
    private float _newXPosition = 5760;
    private bool _isDragging;
    private bool _isLoading;

    #endregion

    void Start()
    {
        for (int i = 0; i < _rectTransform.childCount; i++)
        {
            _listPicture.Add(_rectTransform.GetChild(i).GetComponent<RectTransform>());
        }
    }

    private void OnEnable()
    {
        GameplayScript.onSwipe += StartSwipe;
        GameplayScript.onEndPrimaryTouch += EndSwipe;
        _inputBackButton.action.started += ReturnMainMenu;
    }


    private void OnDisable()
    {
        GameplayScript.onSwipe -= StartSwipe;
        GameplayScript.onEndPrimaryTouch -= EndSwipe;
        _inputBackButton.action.started -= ReturnMainMenu;

    }

    private void StartSwipe(Vector2 velocity)
    {
        _isDragging = true;
        StartCoroutine(CoroutineRectTransform(velocity));
    }

    private void EndSwipe(Vector2 endPosition)
    {
        _isDragging = false;
        StopCoroutine(CoroutineRectTransform(endPosition));
        FindClosestImage();
    }

    private void ReturnMainMenu(InputAction.CallbackContext obj)
    {
        if (!_isLoading)
        {
            GameManager.Instance.ChangeScene(1);
            _isLoading = true;
        }
    }


    IEnumerator CoroutineRectTransform(Vector2 velocity)
    {
        while(_isDragging)
        {
            _newXPosition += velocity.x*_speedSlider;
            //_newXPosition = Mathf.Clamp(_newXPosition, _listPicture[0].localPosition.x, _listPicture[4].localPosition.x);
            _newXPosition = Mathf.Clamp(_newXPosition, -5760, 5760);
            _rectTransform.localPosition = new Vector3(_newXPosition, 0, 0);
            yield return new WaitForFixedUpdate();
        }
    }

    void FindClosestImage()
    {
        _distance = Mathf.Infinity;
        for (int i = 0; i < _rectTransform.childCount; i++)
        {
            if (Vector2.Distance(_rectTransform.localPosition, _listPicture[i].localPosition) < _distance)
            {
                _distance = Vector2.Distance(_rectTransform.localPosition, _listPicture[i].localPosition);
                _tempRectTransform = _listPicture[i];
            }
        }
        _newXPosition = _rectTransform.localPosition.x;
        StartCoroutine(MoveTowardsTarget());
    }

    IEnumerator MoveTowardsTarget()
    {
        while (Mathf.Abs(_rectTransform.localPosition.x - _tempRectTransform.localPosition.x) > 0.1f)
        {
            _newXPosition = _rectTransform.localPosition.x;
            _rectTransform.localPosition = Vector3.Lerp(_rectTransform.localPosition, new Vector3(_tempRectTransform.localPosition.x, 0, 0), Time.fixedDeltaTime * _endSlideSpeed);
            yield return new WaitForFixedUpdate();

        }
        _rectTransform.localPosition = new Vector3(_tempRectTransform.localPosition.x, 0, 0);
    }
}


