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


    public ItemBehaviour linkedItem { get; set; }

    private void OnEnable()
    {
        ItemInput.OnCallDescription += ChangeLinkedItem;
    }

    private void OnDisable()
    {
        ItemInput.OnCallDescription -= ChangeLinkedItem;
    }

    public void InitializeButtons()
    {
        _imageList[0].color = GameManagerSpace.GameManager.colorData.Color1;
        _imageList[1].color = GameManagerSpace.GameManager.colorData.Color2;
        _imageList[2].color = GameManagerSpace.GameManager.colorData.Color3;
        _imageList[3].color = GameManagerSpace.GameManager.colorData.Color4;
        _imageList[4].color = GameManagerSpace.GameManager.colorData.Color5;
    }

    void ChangeLinkedItem(ItemBehaviour newItem)
    {
        linkedItem = newItem;
        ActualizeBorders(FindColor(linkedItem.ItemColor));
    }

    public void ChangeColor(int index)
    {
        ActualizeBorders(index);
        if(linkedItem != null)
            linkedItem.ChangeSpriteColor(_imageList[index].color);
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
        if (colorToFind == GameManagerSpace.GameManager.colorData.Color1)
            return 0;
        if (colorToFind == GameManagerSpace.GameManager.colorData.Color2)
            return 1;
        if (colorToFind == GameManagerSpace.GameManager.colorData.Color3)
            return 2;
        if (colorToFind == GameManagerSpace.GameManager.colorData.Color4)
            return 3;
        if (colorToFind == GameManagerSpace.GameManager.colorData.Color5)
            return 4;

        return -1;
    }
}
