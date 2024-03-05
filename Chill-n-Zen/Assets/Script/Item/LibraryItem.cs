using GameManagerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class LibraryItem : MonoBehaviour
{
    [SerializeField] List<Item> _listItems;
    
    public List<Item> listItems { get => _listItems; }

    public List<Item> Sort(GMStatic.tagRoom room, GMStatic.tagType type, GMStatic.tagMaterial style)
    {
        List<Item> list = new List<Item>();
        
        if(room == GMStatic.tagRoom.Null && type == GMStatic.tagType.Null && style == GMStatic.tagMaterial.Null)
        {
            Debug.Log("Warning : No Filter");
            return GameManager.libraryItems._listItems;
        }

        if (room != GMStatic.tagRoom.Null && type != GMStatic.tagType.Null && style != GMStatic.tagMaterial.Null)
        {
            foreach (Item item in GameManager.libraryItems._listItems)
            {
                if (item.room == room && item.type == type && item.style == style)
                {
                    list.Add(item);
                }
            }
        }
        else if (type != GMStatic.tagType.Null && room != GMStatic.tagRoom.Null)
        {
            foreach (Item item in GameManager.libraryItems._listItems)
            {
                if (item.type == type && item.room == room) 
                { 
                    list.Add(item);
                }
            }
        }
        else if (type != GMStatic.tagType.Null && style != GMStatic.tagMaterial.Null)
        {
            foreach (Item item in GameManager.libraryItems._listItems)
            {
                if (item.type == type && item.style == style)
                {
                    list.Add(item);
                }
            }
        }
        else if (room != GMStatic.tagRoom.Null && style != GMStatic.tagMaterial.Null)
        {
            foreach (Item item in GameManager.libraryItems._listItems)
            {
                if (item.room == room && item.style == style)
                {
                    list.Add(item);
                }
            }
        }
        else if(style != GMStatic.tagMaterial.Null)
        {
            foreach (Item item in GameManager.libraryItems._listItems)
            {
                if (item.style == style)
                {
                    list.Add(item);
                }
            }
        }
        else if (room != GMStatic.tagRoom.Null)
        {
            foreach (Item item in GameManager.libraryItems._listItems)
            {
                if (item.room == room)
                {
                    list.Add(item);
                }
            }
        }
        else if (type != GMStatic.tagType.Null)
        {
            foreach (Item item in GameManager.libraryItems._listItems)
            {
                if (item.type == type)
                {
                    list.Add(item);
                }
            }
        }
        return list;
    }
}


