using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayFurniture : MonoBehaviour
{
    [Header("References")]
    [SerializeField] List<Item> _furnitureList;
    [SerializeField] GameObject _furniturePrefab;
    [SerializeField] GameObject _parentObject;
    [SerializeField] List<GameObject> _itemsCreated;
    [Header("Display format")]
    [SerializeField] Transform _startingPoint;
    [SerializeField] int _furniturePerRow;
    [SerializeField] float _spaceBTWFurniture;
    [SerializeField] float _spaceBTWRows;
    

    public void DisplayCollection(List<Item> collection)
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
                currentPosition = new Vector2(_startingPoint.localPosition.x, currentPosition.y - _spaceBTWRows);
            }
            else
            {
                currentPosition = new Vector2(currentPosition.x + _spaceBTWFurniture, currentPosition.y);
            }
        }
    }

    public void EraseCollection()
    {
        for(int i = 0; i < _itemsCreated.Count; i++)
        {
            Destroy(_itemsCreated[i].gameObject);
        }

        _itemsCreated.Clear();
    }

    [Button] public void DisplayAllFurnitures() => DisplayCollection(_furnitureList);
    [Button] public void EraseAllFurnitures() => EraseCollection();

    /*  Furniture Prefab (furniture button) :
     *      Tag
     *      
     *  Display Furniture -> take List<Item>
     *      Handle no tag / first display
     *  Apply Filter -> Sort
     *  
     *  Selection Furniture
     *      -> drag then create object
     */
}
