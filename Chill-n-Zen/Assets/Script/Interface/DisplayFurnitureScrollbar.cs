using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DisplayFurnitureScrollbar : MonoBehaviour
{
    [SerializeField] InputActionReference _inputPrimaryTouch;
    [Space(5)]
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

    private void OnEnable()
    {
        _inputPrimaryTouch.action.started += StartTouch;
        _inputPrimaryTouch.action.canceled += EndTouch;
    }

    private void OnDisable()
    {
        _inputPrimaryTouch.action.started -= StartTouch;
        _inputPrimaryTouch.action.canceled -= EndTouch;
    }

}
