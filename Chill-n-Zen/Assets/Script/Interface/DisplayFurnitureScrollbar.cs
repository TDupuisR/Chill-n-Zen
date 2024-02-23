using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class DisplayFurnitureScrollbar : MonoBehaviour
{
    [SerializeField] Scrollbar _scrollbar;
    [SerializeField] Transform _parentObject;
    [SerializeField] int _numberRowBeforeScroll;
    [SerializeField] float _spacePerRow;
    float _currentNumberRows;
    float _parentYStartingPosition;

    private void Awake()
    {
        _parentYStartingPosition = _parentObject.position.y;
    }

    private void OnEnable()
    {
        GameplayScript._onSwipe += swipeScroll;
    }

    private void OnDisable()
    {
        GameplayScript._onSwipe -= swipeScroll;
    }

    public void UpdateSize(int numberRows)
    {
        int numberOfExtraRows = numberRows - _numberRowBeforeScroll;

        if (numberOfExtraRows > 0)
        {
            _scrollbar.size = 1f / numberOfExtraRows;
            _currentNumberRows = numberRows;
        }
        else
        {
            _scrollbar.size = 1;
            _currentNumberRows = 0;
        }
        _scrollbar.value = 0;
    }

    public void PerformScroll(float value)
    {
        Vector2 newParentPosition = new Vector2(_parentObject.position.x,
        _parentYStartingPosition + value * _currentNumberRows * _spacePerRow);
        _parentObject.position = newParentPosition;
    }

    private void swipeScroll(Vector2 vector)
    {
        float newScrollBarValue = Mathf.Clamp01(_scrollbar.value - vector.y);
        _scrollbar.value = newScrollBarValue;
    }

}
