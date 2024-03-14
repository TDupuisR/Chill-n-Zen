using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeScrollbar : MonoBehaviour
{
    [SerializeField] Scrollbar _scrollbar;
    [SerializeField] Transform _parentObject;
    [SerializeField] Transform _EdgeOfScroll;
    [SerializeField] int _numberItemBeforeScroll;
    [SerializeField] float _spacePerItem;
    [SerializeField] float _scrollSensitivity;
    [SerializeField] bool _isHorizontal;
    [SerializeField] bool _isInUpperSideOfScreen;

    float _currentNumberItems;
    Vector2 _parentStartingPosition;

    public bool IsScrolling { get ; private set; }

    private void Awake()
    {
        _parentStartingPosition = _parentObject.position;
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
            Vector2 newParentPosition = _parentObject.position;
            if (_isHorizontal)
            {
                newParentPosition = new Vector2(_parentStartingPosition.x - value * _currentNumberItems * _spacePerItem,
                _parentObject.position.y);
            }
            else
            {
                value = 1 - value;
                newParentPosition = new Vector2(_parentObject.position.x,
                _parentStartingPosition.y + value * _currentNumberItems * _spacePerItem);
            }

            _parentObject.position = newParentPosition;
        }
    }

    private void swipeScroll(Vector2 vector)
    {
        float inputVector = vector.y;
        if (_isHorizontal)
            inputVector = vector.x;

        if (GameplayScript.Instance.IsSafeSwipe)
        {
            IsScrolling = true;
            float newScrollBarValue = Mathf.Clamp01(_scrollbar.value + ((inputVector * _scrollSensitivity) / _currentNumberItems));
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
        if (_isHorizontal)
        {
            return GameplayScript.Instance.PrimaryPosition.y < _EdgeOfScroll.position.y;
        }
        else
        {
            if(!(GameplayScript.Instance.PrimaryPosition.x < _EdgeOfScroll.position.x))
                return false;
            
            if (_isInUpperSideOfScreen)
                return GameplayScript.Instance.PrimaryPosition.y > (Screen.height / 2.0f);
            else
                return GameplayScript.Instance.PrimaryPosition.y < (Screen.height / 2.0f);
        }
    }

    void RemoveChildren(GameObject _object)
    {
        throw new System.NotImplementedException();
    }
}
