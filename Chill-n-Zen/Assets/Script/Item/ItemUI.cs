using GameManagerSpace;
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
        _parentObject.transform.position = objectScreenPosition + _item.OffsetPos;
    }

    public void ActivateUI(bool isActive)
    {
        _parentObject.gameObject.SetActive(isActive);
        SetupLeftButton();
    }

    public void SetupLeftButton()
    {
        _validButton.gameObject.SetActive(_item.CurrentState == GMStatic.State.Waiting);
        _validButton.interactable = _item.CanPlace;

        _moveButton.gameObject.SetActive(_item.CurrentState != GMStatic.State.Waiting);
    }
}
