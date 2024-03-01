using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DisplayFurnitureScrollbar : MonoBehaviour
{
    [SerializeField] Scrollbar _scrollbar;
    [SerializeField] Transform _parentObject;
    [SerializeField] Transform _upperEdgeOfScroll;
    [SerializeField] int _numberItemBeforeScroll;
    [SerializeField] float _spacePerItem;
    [SerializeField] float _scrollSensitivity;
    float _currentNumberItems;
    float _parentXStartingPosition;

    public static bool IsScrolling { get ; private set; }

    private void Awake()
    {
        _parentXStartingPosition = _parentObject.position.x;
    }

    private void OnEnable()
    {
        GameplayScript.onSwipe += swipeScroll;
        GameplayScript.onEndPrimaryTouch += stopScrolling;
    }

    private void OnDisable()
    {
        GameplayScript.onSwipe -= swipeScroll;
        GameplayScript.onEndPrimaryTouch -= stopScrolling;
    }

    private void OnValidate()
    {
        if(_scrollSensitivity <= 0)
        {
            Debug.LogWarning("_scrollSensitivity ne peut pas être négatif ou nulle");
            _scrollSensitivity = 1;
        }
    }

    public void UpdateSize(int totalItems)
    {
        int numberOfExtraItem = totalItems - _numberItemBeforeScroll;

        if (numberOfExtraItem > 0)
        {
            _scrollbar.size = 1f / numberOfExtraItem;
            _currentNumberItems = totalItems;
        }
        else
        {
            _scrollbar.size = 1;
            _currentNumberItems = 0;
        }
        _scrollbar.value = 0;
    }

    public void PerformScroll(float value)
    {
        if (isInScrollZone())
        {
            //print(_parentXStartingPosition + "+" + value + "*" + _currentNumberItems + "*" + _spacePerItem);
            Vector2 newParentPosition = new Vector2(_parentXStartingPosition - value * _currentNumberItems * _spacePerItem,
            _parentObject.position.y);
            _parentObject.position = newParentPosition;
        }
    }

    private void swipeScroll(Vector2 vector)
    {
        if (GameplayScript.Instance.IsSafeSwipe)
        {
            IsScrolling = true;
            float newScrollBarValue = Mathf.Clamp01(_scrollbar.value + ((vector.x * _scrollSensitivity) / _currentNumberItems));
            _scrollbar.value = newScrollBarValue;
        }
    }

    private void stopScrolling(Vector2 vector)
    {
        if (IsScrolling)
        {
            IsScrolling = false;
        }
    }

    public void ResetScroll()
    {
        _scrollbar.value = 0;
    }

    bool isInScrollZone()
    {
        return GameplayScript.Instance.PrimaryPosition.y < _upperEdgeOfScroll.position.y;
    }
}
