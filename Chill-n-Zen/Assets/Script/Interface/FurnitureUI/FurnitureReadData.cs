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
    [SerializeField] Image _imgColored;
    [SerializeField] TMP_Text _priceText;
    [SerializeField] TMP_Text _nameText;
    [SerializeField] TMP_Text _pointsText;
    [SerializeField] TMP_Text _descriptionText;
    [Header("Badge Images")]
    [SerializeField] Image _roomImg;
    [SerializeField] Image _styleImg;
    [SerializeField] Image _typeImg;
    [Space(2)]
    [SerializeField] TMP_Text _roomText;
    [SerializeField] TMP_Text _styleText;
    [SerializeField] TMP_Text _typeText;

    public Item Furniture
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
        _img.sprite = furniture.spriteOneFixed;
        _imgColored.sprite = furniture.spriteOneColored;

        _imgColored.rectTransform.sizeDelta = new Vector2 (_imgColored.sprite.rect.width * 100.0f / _imgColored.sprite.rect.height, 100.0f);

        _priceText.text = furniture.price.ToString() + " Cr";
        //if(_nameText != null) bite
        //    _nameText.text = furniture.name.ToString();
        if(_pointsText != null)
            _pointsText.text = furniture.score.ToString() + " Pts";
        if(_descriptionText != null)
            _descriptionText.text = furniture.description;

        if(IsAllTagValid(furniture))
        {
            _roomImg.color = _tagUIList.associatedSprite[_tagUIList.tagNames.IndexOf(furniture.room.ToString())];
            if(_roomText != null)
            {
                _roomText.text = furniture.room.ToString();
            }

            _styleImg.color = _tagUIList.associatedSprite[_tagUIList.tagNames.IndexOf(furniture.material.ToString())];
            if (_styleText != null)
            {
                _styleText.text = furniture.material.ToString();
            }

            _typeImg.color = _tagUIList.associatedSprite[_tagUIList.tagNames.IndexOf(furniture.type.ToString())];
            if (_typeText != null)
            {
                _typeText.text = furniture.type.ToString();
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
        if (!_tagUIList.tagNames.Contains(furniture.material.ToString()))
        {
            Debug.LogError("style tag " + furniture.material.ToString() + " n'est pas dans la liste des tags ! (TagUI)");
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
