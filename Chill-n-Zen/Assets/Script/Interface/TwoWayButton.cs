using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TwoWayButton : MonoBehaviour
{
    [SerializeField] bool _isActive;

    [SerializeField] UnityEvent OnActivate;
    [SerializeField] UnityEvent OnDeActivate;

    public void Activate()
    {
        if(_isActive)
            OnActivate?.Invoke();
        else
            OnDeActivate?.Invoke();

        _isActive = !_isActive;
    }
}
