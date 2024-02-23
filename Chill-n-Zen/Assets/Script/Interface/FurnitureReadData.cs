using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureReadData : MonoBehaviour
{
    Item _furniture;
    [SerializeField] TagUIScriptable _tagUIList;
    [SerializeField] Image _img;
    [SerializeField] TMP_Text _priceText;
    [Header("Badge Images")]
    [SerializeField] Image _roomImg;
    [SerializeField] Image _styleImg;
    [SerializeField] Image _typeImg;
    [Space(2)]
    [SerializeField] TMP_Text _roomText;
    [SerializeField] TMP_Text _styleText;
    [SerializeField] TMP_Text _typeText;

    public Item furniture
    {
        get => _furniture;
        set 
        {
            _furniture = value;
            ReadFurnitureData(_furniture);
        }
    }

    public void ReadFurnitureData(Item furniture)
    {
        _img.sprite = furniture.asset2D;
        _priceText.text = furniture.price.ToString() + " Cr";

        if(IsAllTagValid(furniture))
        {
            _roomImg.sprite = _tagUIList.associatedSprite[_tagUIList.tagNames.IndexOf(furniture.room.ToString())];
            if(_roomText != null)
            {
                _roomText.text = furniture.room.ToString();
            }

            _styleImg.sprite = _tagUIList.associatedSprite[_tagUIList.tagNames.IndexOf(furniture.style.ToString())];
            if (_styleText != null)
            {
                _styleText.text = furniture.room.ToString();
            }

            _typeImg.sprite = _tagUIList.associatedSprite[_tagUIList.tagNames.IndexOf(furniture.type.ToString())];
            if (_typeText != null)
            {
                _typeText.text = furniture.room.ToString();
            }
        }
    }

    bool IsAllTagValid(Item furniture)
    {
        if (!_tagUIList.tagNames.Contains(furniture.room.ToString()))
        {
            Debug.LogError("room tag " + furniture.room.ToString() + " n'est pas dans la liste des tags ! (TagUI)");
            return false;
        }
        if (!_tagUIList.tagNames.Contains(furniture.style.ToString()))
        {
            Debug.LogError("style tag " + furniture.style.ToString() + " n'est pas dans la liste des tags ! (TagUI)");
            return false;
        }
        if (!_tagUIList.tagNames.Contains(furniture.type.ToString()))
        {
            Debug.LogError("room tag " + furniture.type.ToString() + " n'est pas dans la liste des tags ! (TagUI)");
            return false;
        }
        return true;
    }
}
