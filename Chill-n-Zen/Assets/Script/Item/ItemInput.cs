using UnityEngine;
using GameManagerSpace;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ItemInput : MonoBehaviour
{
    [SerializeField] ItemBehaviour _itemBehave;
    [SerializeField] ItemUI _itemUI;
    GameplayScript _gameplay;

    bool _primWasPressed = false;
    bool _holdWasPressed = false;
    bool _isOnItem = false;
    bool _isOnUI = false;
    int _layerUI;
    bool _showedUI;
    
    public static Action<ItemBehaviour> OnCallDescription;
    public static Action OnCallHideDescription;

    private void Start()
    {
        _layerUI = LayerMask.NameToLayer("UI");
        _gameplay = GameManager.gameplayScript;

        OnCallDescription?.Invoke(_itemBehave);
    }

    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysast)
    {
        bool res = false;

        for (int index = 0; index < eventSystemRaysast.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysast[index];
            if (curRaysastResult.gameObject.layer == _layerUI)
                res = true;
        }

        return res;
    }
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = GameManager.gameplayScript.PrimaryPosition;

        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);

        return raysastResults;
    }

    private void OnMouseOver()
    {
        if (!_isOnUI)
        {
            _isOnItem = true;

            // Etape 3 -> 2
            if (CheckIsHolding() && _itemBehave.CurrentState == GMStatic.State.Waiting)
            {
                _itemBehave.CurrentState = GMStatic.State.Moving;
                _itemUI.ActivateUI(false);

                CameraControls.Instance.CanMoveCamera = false;
            }
            // Etape 3 -> Rotation
            if (CheckIsTouching() && _itemBehave.CurrentState == GMStatic.State.Waiting )
            {
                _itemBehave.Rotation();
            }
            // Etape 4 -> 5
            if (CheckIsTouching() && _itemBehave.CurrentState == GMStatic.State.Placed)
            {
                if (TileSystem.Instance.IsSceneVacant)
                {
                    _itemUI.ActivateUI(true);
                    OnCallDescription?.Invoke(_itemBehave);
                    _showedUI = true;
                }
            }
        }
    }
    private void OnMouseExit() { _isOnItem = false; }

    private void Update()
    {
        _isOnUI = IsPointerOverUIElement(GetEventSystemRaycastResults());

        // Etape 2 -> 3
        if (_itemBehave.CurrentState == GMStatic.State.Moving && !_gameplay.IsHold)
        {
            _itemBehave.CurrentState = GMStatic.State.Waiting;
            _itemUI.ActivateUI(true);

            CameraControls.Instance.CanMoveCamera = true;
        }
        // Etape 5 -> 4
        if (_itemBehave.CurrentState == GMStatic.State.Placed && CheckIsTouching() && _isOnItem == false)
        {
            _itemUI.ActivateUI(false);
            if(_showedUI)
            {
                OnCallHideDescription?.Invoke();
                _showedUI = false;
            }
        }

        _primWasPressed = _gameplay.IsPrimaryPressed;
        _holdWasPressed = _gameplay.IsHold;
    }

    private bool CheckIsHolding()
    {
        return _gameplay.IsPrimaryPressed && !_gameplay.IsSecondaryPressed && _gameplay.IsHold;
    }
    private bool CheckIsTouching()
    {
        return !_gameplay.IsPrimaryPressed && _primWasPressed && !_holdWasPressed && !_gameplay.IsSecondaryPressed;
    }

    public void SendErase()
    {
        OnCallHideDescription?.Invoke();
    }
}
