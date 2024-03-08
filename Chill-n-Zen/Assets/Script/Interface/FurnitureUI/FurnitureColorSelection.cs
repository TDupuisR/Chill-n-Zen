using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureColorSelection : MonoBehaviour
{
    [SerializeField] List<Image> _imageList;
    [SerializeField] List<TMP_Text> _textList;


    public ItemBehaviour linkedItem { get; set; }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public void InitializeButtons()
    {
        throw new System.NotImplementedException();
    }

    void ChangeLinkedItem(ItemBehaviour newItem)
    {
        linkedItem = newItem;
    }

    public void ChangeColor(int index)
    {
        Color color = Color.white;
        linkedItem.ChangeSpriteColor(color);
    }
}
