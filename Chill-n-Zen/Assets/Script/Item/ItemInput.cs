using UnityEngine;
using GameManagerSpace;
using System;

public class ItemInput : MonoBehaviour
{
    [SerializeField] ItemBehaviour _itemBehave;
    [SerializeField] ItemUI _itemUI;
    GameplayScript _gameplay;

    bool _primWasPressed = false;
    bool _holdWasPressed = false;
    bool _isOnItem = false;
    
    public static Action<ItemBehaviour> OnCallDescription;

    private void Start()
    {
        _gameplay = GameplayScript.Instance;

        OnCallDescription?.Invoke(_itemBehave);
    }
    
    private void OnMouseOver()
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
            }
        }
    }
    private void OnMouseExit() { _isOnItem = false; }

    private void Update()
    {
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
}
