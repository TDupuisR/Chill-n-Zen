using GameManagerSpace;
using System.Collections;
using System.Collections.Generic;
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

    public List<Item> SortBy(GameManagerSpace.GMStatic.tagRoom room, GameManagerSpace.GMStatic.tagType type, GameManagerSpace.GMStatic.tagStyle style)
    {
        List<Item> list = new List<Item>();
        
        if (room != GameManagerSpace.GMStatic.tagRoom.Null)
        {
            foreach (Item item in GameManager.libraryItems.listItems)
            {
                if (item.room == room)
                {
                    list.Add(item);
                }
            }
        }
        if (type != GameManagerSpace.GMStatic.tagType.Null)
        {
            foreach (Item item in GameManager.libraryItems.listItems)
            {
                if (item.type == type) 
                { 
                    list.Add(item);
                }
            }
        }


        if(style != GameManagerSpace.GMStatic.tagStyle.Null)
        {
            foreach (Item item in GameManager.libraryItems.listItems)
            {
                if (item.style == style)
                {
                    list.Add(item);
                }
            }
        }
        return list;
    }
}

