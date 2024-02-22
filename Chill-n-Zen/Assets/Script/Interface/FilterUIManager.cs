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
    enum _typeOfTags { tagRoom, tagType, tagStyle }

    public int roomFilter
    {
        get; set;
    }

    public int typeFilter
    {
        get; set;
    }

    public int styleFilter
    {
        get; set;
    }

    private void Awake()
    {
        //Initialize Dropdowns
        InitDropDown(_roomFilterDropdown, _typeOfTags.tagRoom);
        InitDropDown(_typeFilterDropdown, _typeOfTags.tagType);
        InitDropDown(_styleFilterDropdown, _typeOfTags.tagStyle);
    }

    void InitDropDown(TMP_Dropdown filter, _typeOfTags tag)
    {
        filter.options.Add(new TMP_Dropdown.OptionData() { text = "None" });
        switch (tag)
        {
            case _typeOfTags.tagRoom:
                foreach (GMStatic.tagRoom tagElement in Enum.GetValues(typeof(GMStatic.tagRoom)))
                {
                    string tagName = tagElement.ToString();

                    if (tagName != "Null")
                    {
                        filter.options.Add(new TMP_Dropdown.OptionData() { text = tagName });
                    }
                }
                break;

            case _typeOfTags.tagType:
                foreach (GMStatic.tagStyle tagElement in Enum.GetValues(typeof(GMStatic.tagStyle)))
                {
                    string tagName = tagElement.ToString();
                    if (tagName != "Null")
                    {
                        filter.options.Add(new TMP_Dropdown.OptionData() { text = tagName });
                    }
                }
                break;

            case _typeOfTags.tagStyle:
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
        List<Item> newItems = GameManager.libraryItems.Sort((GMStatic.tagRoom)roomFilter, (GMStatic.tagType)typeFilter, (GMStatic.tagStyle)styleFilter);
        _displayFurniture.ResetAndDisplay(newItems);
    }
}
