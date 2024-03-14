using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureColorSelection : MonoBehaviour
{
    [SerializeField] List<Image> _imageList;
    [SerializeField] List<Image> _borderImgList;
    [SerializeField] List<TMP_Text> _textList;
    Vector2 currentButtonPosition;
    int _cachedColor;

    public ItemBehaviour linkedItem { get; set; }

    private void OnEnable()
    {
        ItemInput.OnCallDescription += ChangeLinkedItem;
        ItemSpawner.onItemTouched += SelectNewItem;
    }


    private void OnDisable()
    {
        ItemInput.OnCallDescription -= ChangeLinkedItem;
        ItemSpawner.onItemTouched -= SelectNewItem;
    }

    public void InitializeButtons()
    {
        _imageList[0].color = GameManagerSpace.GameManager.colorData.Color1;
        _imageList[1].color = GameManagerSpace.GameManager.colorData.Color2;
        _imageList[2].color = GameManagerSpace.GameManager.colorData.Color3;
        _imageList[3].color = GameManagerSpace.GameManager.colorData.Color4;
        _imageList[4].color = GameManagerSpace.GameManager.colorData.Color5;
        _imageList[5].color = GameManagerSpace.GameManager.colorData.Color6;
    }


    void SelectNewItem(Vector2 vector)
    {
        if(currentButtonPosition != vector)
        {
            linkedItem = null;
            ChangeColor(0);
            currentButtonPosition = vector;
        }

    }

    void ChangeLinkedItem(ItemBehaviour newItem)
    {
        linkedItem = newItem;
        int itemColor = FindColor(linkedItem.ItemColor);

        if(itemColor != -1)
        {
            ActualizeBorders(itemColor);
            return;
        }
        else if (_cachedColor != -1)
        {
            ChangeColor(_cachedColor);
            _cachedColor = -1;
        }
        else
            ChangeColor(0);
    }

    public void ChangeColor(int index)
    {
        ActualizeBorders(index);
        if (linkedItem != null)
            linkedItem.ChangeSpriteColor(_imageList[index].color);
        else
            _cachedColor = index;
    }

    void ActualizeBorders(int newSelected)
    {
        foreach (Image border in _borderImgList)
        {
            border.color = Color.black;
        }
        if(newSelected != -1)
            _borderImgList[newSelected].color = Color.white;
    }

    int FindColor(Color colorToFind)
    {
        for(int i=0; i < _imageList.Count; i++)
        {
            if (_imageList[i].color == colorToFind)
                return i;
        }
        return -1;
    }
}
