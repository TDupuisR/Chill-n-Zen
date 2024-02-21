using GameManagerSpace;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
 

public class LibraryItem : MonoBehaviour
{
    public static LibraryItem Instance;
    [SerializeField] List<Item> listItems;
    
    private void Awake()
    {
        if (Instance == null)
        {
            if (Instance != null) Destroy(gameObject);
            Instance = this;
        }
        else
        {
            Debug.LogError(" (error : 1x0) Too many LibraryItem instance ", gameObject);
        }
    }

    private void Start()
    {
        List<Item> listitem1 = Sort(GMStatic.tagRoom.Bedroom, GMStatic.tagType.Furniture, GMStatic.tagStyle.Vintage);
        Debug.Log(listitem1.Count);
    }

    public List<Item> Sort(GMStatic.tagRoom room, GMStatic.tagType type, GMStatic.tagStyle style)
    {
        List<Item> list = new List<Item>();
        
        if(room == GMStatic.tagRoom.Null && type == GMStatic.tagType.Null && style == GMStatic.tagStyle.Null)
        {
            Debug.Log("Warning : No Filter");
            return GameManager.libraryItems.listItems;
        }

        if (room != GMStatic.tagRoom.Null && type != GMStatic.tagType.Null && style != GMStatic.tagStyle.Null)
        {
            foreach (Item item in GameManager.libraryItems.listItems)
            {
                if (item.room == room && item.type == type && item.style == style)
                {
                    list.Add(item);
                }
            }
        }
        else if (type != GMStatic.tagType.Null && room != GMStatic.tagRoom.Null)
        {
            foreach (Item item in GameManager.libraryItems.listItems)
            {
                if (item.type == type && item.room == room) 
                { 
                    list.Add(item);
                }
            }
        }
        else if (type != GMStatic.tagType.Null && style != GMStatic.tagStyle.Null)
        {
            foreach (Item item in GameManager.libraryItems.listItems)
            {
                if (item.type == type && item.style == style)
                {
                    list.Add(item);
                }
            }
        }
        else if (room != GMStatic.tagRoom.Null && style != GMStatic.tagStyle.Null)
        {
            foreach (Item item in GameManager.libraryItems.listItems)
            {
                if (item.room == room && item.style == style)
                {
                    list.Add(item);
                }
            }
        }
        else if(style != GMStatic.tagStyle.Null)
        {
            foreach (Item item in GameManager.libraryItems.listItems)
            {
                if (item.style == style)
                {
                    list.Add(item);
                }
            }
        }
        else if (room != GMStatic.tagRoom.Null)
        {
            foreach (Item item in GameManager.libraryItems.listItems)
            {
                if (item.room == room)
                {
                    list.Add(item);
                }
            }
        }
        else if (type != GMStatic.tagType.Null)
        {
            foreach (Item item in GameManager.libraryItems.listItems)
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


