using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureFilterButton : MonoBehaviour
{
    [SerializeField] GameObject _filterParent;
    bool _isDeployed = false;

    public void Deploy()
    {
        _isDeployed = !_isDeployed;
        _filterParent.SetActive(_isDeployed);
    }

    public void Hide()
    {
        _isDeployed = false;
        _filterParent.SetActive(_isDeployed);
    }
}
