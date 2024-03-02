using GameManagerSpace;
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
    [SerializeField] float _spacingFactor;

    [Header("Warning Text")]
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] string _obstructionText;
    [SerializeField] string _noaccessText;

    private void Update()
    {
        //move UI with object
        Vector3 objectScreenPosition = Camera.main.WorldToScreenPoint(_item.transform.position);
        _parentObject.transform.position = objectScreenPosition + _item.OffsetPos;
        _parentObject.sizeDelta = _item.SpriteRenderer.bounds.size * _spacingFactor;
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

    public void TextIssues(bool osbtruction, bool noaccess)
    {
        if (osbtruction && noaccess)
            _text.text = _obstructionText + "\n" + _noaccessText;
        else if (osbtruction)
            _text.text = _obstructionText;
        else if (noaccess)
            _text.text = _noaccessText;
        else
        {
            _text.text = "";
            // Desactivé la boite de texte //
        }
    }
}
