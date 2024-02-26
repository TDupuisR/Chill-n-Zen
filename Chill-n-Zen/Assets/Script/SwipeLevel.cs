using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class SwipeLevel : MonoBehaviour
{

    #region Variable 
    [Foldout("Inputs")][SerializeField] InputActionReference _inputPrimaryTouch;
    [Foldout("Inputs")][SerializeField] InputActionReference _inputPrimaryPosition;
    [SerializeField] private RectTransform _rectTransform;
    private List<RectTransform> _listPicture = new List<RectTransform>();
    private RectTransform _tempRectTransform = null;
    private float _distance = Mathf.Infinity;
    private float _newXPosition;
    private float _initialOffSet;
    private bool _isDragging;
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
        _inputPrimaryTouch.action.started += StartSwipe;
        _inputPrimaryTouch.action.canceled += EndSwipe;
    }

    private void OnDisable()
    {
        _inputPrimaryTouch.action.started -= StartSwipe;
        _inputPrimaryTouch.action.canceled -= EndSwipe;
    }

    private void StartSwipe(InputAction.CallbackContext context)
    {
        _isDragging = true;
        _initialOffSet = Input.mousePosition.x - _rectTransform.position.x;
        StartCoroutine(CoroutineRectTransform());
    }

    private void EndSwipe(InputAction.CallbackContext context)
    {
        _isDragging = false;
        StopCoroutine(CoroutineRectTransform());
        FindClosestImage();
    }


    IEnumerator CoroutineRectTransform()
    {
        while(_isDragging)
        {
            _newXPosition = Input.mousePosition.x - _initialOffSet;
            _newXPosition = Mathf.Clamp(_newXPosition, _listPicture[0].position.x, _listPicture[4].position.x);
            _rectTransform.position = new Vector3(_newXPosition, _rectTransform.position.y, _rectTransform.position.z);
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
        _rectTransform.localPosition = new Vector2(_tempRectTransform.localPosition.x,0);
    }
}
