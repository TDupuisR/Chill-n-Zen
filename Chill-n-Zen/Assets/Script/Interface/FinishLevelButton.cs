using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishLevelButton : MonoBehaviour
{
    [SerializeField] Button _button;

    public void UnlockButton()
    {
        _button.interactable = true;
    }

    public void CompleteLevel()
    {
        throw new System.NotImplementedException();
    }
}
