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
    [SerializeField] private float speedSlider = 1;
    private List<RectTransform> _listPicture = new List<RectTransform>();
    private RectTransform _tempRectTransform = null;
    private float _distance = Mathf.Infinity;
    private float _newXPosition;
    private float _initialOffSet;
    private bool _isDragging;
    private float posMax;
    private float posMin;

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
        GameplayScript._onSwipe += StartSwipe;
        GameplayScript._onEndPrimaryTouch += EndSwipe;
    }

    private void OnDisable()
    {
        GameplayScript._onSwipe -= StartSwipe;
        GameplayScript._onSwipe -= EndSwipe;
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


    IEnumerator CoroutineRectTransform(Vector2 velocity)
    {
        while(_isDragging)
        {
            _newXPosition += velocity.x*speedSlider;
            _newXPosition = Mathf.Clamp(_newXPosition, _listPicture[0].localPosition.x, _listPicture[4].localPosition.x);
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
        _rectTransform.localPosition = new Vector2(_tempRectTransform.localPosition.x,0);
        _newXPosition = _rectTransform.localPosition.x;
    }
}
