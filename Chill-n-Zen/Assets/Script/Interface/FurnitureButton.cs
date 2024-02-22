using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureButton : MonoBehaviour
{
    Item _furniture;
    [SerializeField] Image _img;
    [SerializeField] TMP_Text _priceText;
    public Item furniture
    {
        get => _furniture;
        set 
        {
            _furniture = value;
            ReadLinkedFurniture();
        }
    }

    public void ReadLinkedFurniture()
    {
        _img.sprite = _furniture.asset2D;
        _priceText.text = _furniture.price.ToString() + " Cr";
    }

    public void SelectFurniture()
    {

    }
}
