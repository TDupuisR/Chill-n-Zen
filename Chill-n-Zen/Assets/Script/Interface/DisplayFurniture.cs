using GameManagerSpace;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayFurniture : MonoBehaviour
{
    [Header("References")]
    [SerializeField] DisplayFurnitureScrollbar _displayScrollbar;
    [SerializeField] GameObject _furniturePrefab;
    [SerializeField] GameObject _parentObject;
    [Header("Display format")]
    [SerializeField] Transform _startingPoint;
    [SerializeField] int _furniturePerRow;
    [SerializeField] float _spaceBTWFurniture;
    [SerializeField] float _spaceBTWRows;
    List<GameObject> _itemsCreated = new List<GameObject>();
    int _numberOfRows;

    public int numberOfRows
    {
        get => _numberOfRows;
        set
        {
            _numberOfRows = value;
            _displayScrollbar.UpdateSize(value);
        }
    }

    private void OnValidate()
    {
        if(_furniturePerRow <= 0)
        {
            Debug.LogWarning("_furniturePerRow ne peut pas être négative ou nulle");
            _furniturePerRow = 1;
        }

        if(_spaceBTWFurniture <= 0)
        {
            Debug.LogWarning("_spaceBTWFurniture ne peut pas être négative ou nulle");
            _spaceBTWFurniture = 1;
        }

        if (_spaceBTWRows <= 0)
        {
            Debug.LogWarning("_spaceBTWRows ne peut pas être négative ou nulle");
            _spaceBTWRows = 1;
        }
    }

    private void Awake()
    {
        DisplayCollection(GameManager.libraryItems.listItems);
    }

    public void ResetAndDisplay(List<Item> collection)
    {
        if(_itemsCreated.Count > 0)
        {
            EraseCollection();
        }

        DisplayCollection(collection);

    }


    void DisplayCollection(List<Item> collection)
    {
        Vector2 currentPosition = _startingPoint.localPosition;
        int numberOfItems = 0;
        foreach (Item item in collection)
        {
            GameObject newItem = Instantiate(_furniturePrefab, _parentObject.transform);
            newItem.transform.localPosition = currentPosition;
            newItem.GetComponent<FurnitureButton>().furniture = item;
            _itemsCreated.Add(newItem);

            numberOfItems++;
            if(numberOfItems == _furniturePerRow)
            {
                numberOfItems = 0;
                numberOfRows++;
                currentPosition = new Vector2(_startingPoint.localPosition.x, currentPosition.y - _spaceBTWRows);
            }
            else
            {
                currentPosition = new Vector2(currentPosition.x + _spaceBTWFurniture, currentPosition.y);
            }
        }
    }

    void EraseCollection()
    {
        for(int i = 0; i < _itemsCreated.Count; i++)
        {
            Destroy(_itemsCreated[i].gameObject);
        }
        numberOfRows = 0;
        _itemsCreated.Clear();
    }

    [Button] public void DisplayAllFurnitures() => DisplayCollection(GameManager.libraryItems.listItems);
    [Button] public void EraseAllFurnitures() => EraseCollection();
}
