using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContemplativeButton : MonoBehaviour
{
    [SerializeField] GameObject _gameplayObjects;
    bool _active = false;

    public void Activate()
    {
        _gameplayObjects.SetActive(_active);
        _active = !_active;
    }
}
