using UnityEngine;
using GameManagerSpace;

public class ItemInput : MonoBehaviour
{
    [SerializeField] ItemBehaviour _itemBehave;
    [SerializeField] ItemUI _itemUI;
    GameplayScript _gameplay;

    bool _primWasPressed = false;
    bool _holdWasPressed = false;

    private void Start()
    {
        _gameplay = GameplayScript.Instance;
    }

    private void OnMouseOver()
    {
        // Etape 3 -> 2
        if (CheckIsHolding() && _itemBehave.CurrentState == GMStatic.State.Waiting)
        {
            _itemBehave.CurrentState = GMStatic.State.Moving;
            _itemUI.ActivateUI(false);
        }
        // Etape 3 -> Rotation
        if (CheckIsTouching() && _itemBehave.CurrentState == GMStatic.State.Waiting )
        {
            _itemBehave.Rotation();
        }
        // Etape 4 -> 5
        if (CheckIsTouching() && _itemBehave.CurrentState == GMStatic.State.Placed)
        {
            _itemUI.ActivateUI(true);
        }
    }

    private void Update()
    {
        // Etape 2 -> 3
        if (_itemBehave.CurrentState == GMStatic.State.Moving && !_gameplay.IsHold)
        {
            _itemBehave.CurrentState = GMStatic.State.Waiting;
            _itemUI.ActivateUI(true);
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
        return _primWasPressed && !_holdWasPressed && !_gameplay.IsSecondaryPressed;
    }
}
