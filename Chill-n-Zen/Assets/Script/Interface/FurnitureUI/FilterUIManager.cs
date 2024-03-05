using GameManagerSpace;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilterUIManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] DisplayFurniture _displayFurniture;
    [SerializeField] TMP_Dropdown _roomFilterDropdown, _typeFilterDropdown, _styleFilterDropdown;

    [Header("Button")]
    [SerializeField] GameObject _filterParent;
    bool _isDeployed = false;

    enum TypeOfTags { tagRoom, tagType, tagStyle }

    public int RoomFilter { get; set; }

    public int TypeFilter { get; set; }

    public int StyleFilter { get; set; }

    private void Awake()
    {
        //Initialize Dropdowns
        InitDropDown(_roomFilterDropdown, TypeOfTags.tagRoom);
        InitDropDown(_typeFilterDropdown, TypeOfTags.tagType);
        InitDropDown(_styleFilterDropdown, TypeOfTags.tagStyle);
    }

    void InitDropDown(TMP_Dropdown filter, TypeOfTags tag)
    {
        filter.options.Add(new TMP_Dropdown.OptionData() { text = "None" });
        switch (tag)
        {
            case TypeOfTags.tagRoom:
                foreach (GMStatic.tagRoom tagElement in Enum.GetValues(typeof(GMStatic.tagRoom)))
                {
                    string tagName = tagElement.ToString();

                    if (tagName != "Null")
                    {
                        filter.options.Add(new TMP_Dropdown.OptionData() { text = tagName });
                    }
                }
                break;

            case TypeOfTags.tagType:
                foreach (GMStatic.tagStyle tagElement in Enum.GetValues(typeof(GMStatic.tagStyle)))
                {
                    string tagName = tagElement.ToString();
                    if (tagName != "Null")
                    {
                        filter.options.Add(new TMP_Dropdown.OptionData() { text = tagName });
                    }
                }
                break;

            case TypeOfTags.tagStyle:
                foreach (GMStatic.tagType tagElement in Enum.GetValues(typeof(GMStatic.tagType)))
                {
                    string tagName = tagElement.ToString();
                    if (tagName != "Null")
                    {
                        filter.options.Add(new TMP_Dropdown.OptionData() { text = tagName });
                    }
                }
                break;

            default:
                Debug.LogError("Can't find style in switch/case");
                break;
        }
    }


    public void ApplyFilter()
    {
        List<Item> newItems = GameManager.libraryItems.Sort((GMStatic.tagRoom)RoomFilter, (GMStatic.tagType)TypeFilter, (GMStatic.tagStyle)StyleFilter);
        _displayFurniture.ResetAndDisplay(newItems);
    }

    /*
        Button Functions
     */
    public void Deploy()
    {
        _isDeployed = !_isDeployed;
        _filterParent.SetActive(_isDeployed);
    }

    public void Hide()
    {
        _isDeployed = false;
        _filterParent.SetActive(_isDeployed);
    }
}
