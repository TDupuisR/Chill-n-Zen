using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] ItemBehaviour _item;
    [SerializeField] RectTransform _parentObject;
    [SerializeField] Button _validButton;
    [SerializeField] Button _moveButton;
    [SerializeField] Button _deleteButton;

    private void Update()
    {
        //move UI with object
        Vector3 objectScreenPosition = Camera.main.WorldToScreenPoint(_item.transform.position);
        _parentObject.transform.position = objectScreenPosition;
    }
}
