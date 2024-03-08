using NaughtyAttributes.Test;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeContainChildren : MonoBehaviour
{
    [SerializeField] RectTransform _rect;
    [SerializeField] Transform _objectToCheck;
    [Space(3)]
    [SerializeField] float _sizeOfOneChildren;
    [SerializeField] int _numberChildrenBeforeExpansion;

    Vector3 _defaultPosition;
    Vector3 _defaultSize;
    private void Awake()
    {
        _defaultPosition = _rect.localPosition;
        _defaultSize = _rect.sizeDelta;
        print(_defaultSize);
    }


    private void OnEnable()
    {
        ObjectivesUI.OnFinishInitialisation += ChangeSize;
    }
    private void OnDisable()
    {
        ObjectivesUI.OnFinishInitialisation -= ChangeSize;
    }

    public void ChangeSize()
    {
        int numberOfChildren = _objectToCheck.childCount;

        if (numberOfChildren < 0)
        {
            Debug.LogError("Number of objectives in " + gameObject + " is 0 or negative !!!", gameObject);
            return;
        }

        if (numberOfChildren > _numberChildrenBeforeExpansion)
        {
            float newFactor = _sizeOfOneChildren * numberOfChildren;
            _rect.sizeDelta = new Vector2(_defaultSize.x, _defaultSize.y + newFactor);
            _rect.localPosition = new Vector2(_defaultPosition.x, -.5f*_rect.sizeDelta.y - 65);
        }

    }
}
