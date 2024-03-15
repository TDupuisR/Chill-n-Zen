using GameManagerSpace;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayFurniture : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SwipeScrollbar _displayScrollbar;
    [SerializeField] GameObject _furniturePrefab;
    [SerializeField] GameObject _parentObject;
    [SerializeField] GameObject _selectedUnderlay;
    [Header("Display format")]
    [SerializeField] Transform _startingPoint;
    [SerializeField] float _spaceBTWFurniture;

    [Header("Outside References")]
    [SerializeField] Transform _objectParent;
    [SerializeField] FurnitureReadData _detailWindow;

    //[SerializeField] int _furniturePerRow;
    //[SerializeField] float _spaceBTWRows;
    List<GameObject> _itemsCreated = new List<GameObject>();
    int _totalNumberOfItems;

    public int TotalNumberOfItems
    {
        get => _totalNumberOfItems;
        set
        {
            _totalNumberOfItems = value;
            _displayScrollbar.UpdateSize(value);
        }
    }

    private void OnValidate()
    {
        //if(_furniturePerRow <= 0) bite
        //{
        //    Debug.LogWarning("_furniturePerRow ne peut pas être négative ou nulle");
        //    _furniturePerRow = 1;
        //}

        if(_spaceBTWFurniture <= 0)
        {
            Debug.LogWarning("_spaceBTWFurniture ne peut pas être négative ou nulle");
            _spaceBTWFurniture = 1;
        }

        //if (_spaceBTWRows <= 0)
        //{
        //    Debug.LogWarning("_spaceBTWRows ne peut pas être négative ou nulle");
        //    _spaceBTWRows = 1;
        //}
    }

    private void OnEnable()
    {
        ItemSpawner.onItemTouched += ActivateUnderlay;
        ItemSpawner.onItemSelected += DisableUnderlay;

    }

    private void OnDisable()
    {
        ItemSpawner.onItemTouched -= ActivateUnderlay;
        ItemSpawner.onItemSelected -= DisableUnderlay;
    }

    private void Start()
    {
        if (_objectParent == null)
        {
            Debug.LogError("No _objectParent in DisplayFurniture");
        }

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
            newItem.GetComponent<FurnitureReadData>().Furniture = item;
            newItem.GetComponent<ItemSpawner>().ObjectParent = _objectParent;
            newItem.GetComponent<ItemSpawner>().DetailWindow = _detailWindow;
            newItem.GetComponent<ItemSpawner>().Scrollbar = _displayScrollbar;
            _itemsCreated.Add(newItem);
            numberOfItems++;

            currentPosition = new Vector2(currentPosition.x + _spaceBTWFurniture, currentPosition.y);


            //if(numberOfItems == _furniturePerRow)
            //{
            //    numberOfItems = 0;
            //    totalNumberOfItems++;
            //    currentPosition = new Vector2(_startingPoint.localPosition.x, currentPosition.y - _spaceBTWRows);
            //}
            //else
            //{
            //    currentPosition = new Vector2(currentPosition.x + _spaceBTWFurniture, currentPosition.y);
            //}
        }
        TotalNumberOfItems = numberOfItems;
    }

    void EraseCollection()
    {
        for(int i = 0; i < _itemsCreated.Count; i++)
        {
            Destroy(_itemsCreated[i].gameObject);
        }
        TotalNumberOfItems = 0;
        _itemsCreated.Clear();
    }

    #region UnderLay function
    public void ActivateUnderlay(Vector2 pos)
    {
        _selectedUnderlay.SetActive(true);
        _selectedUnderlay.transform.position = pos;
        GameManager.audioManager.PlaySound("clic sur bouton");
    }
    public void DisableUnderlay() => _selectedUnderlay.SetActive(false);
    #endregion

    [Button] public void DisplayAllFurnitures() => DisplayCollection(GameManager.libraryItems.listItems);
    [Button] public void EraseAllFurnitures() => EraseCollection();
}
