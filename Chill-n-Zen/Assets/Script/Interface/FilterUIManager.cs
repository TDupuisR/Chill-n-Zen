using GameManagerSpace;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilterUIManager : MonoBehaviour
{
    [SerializeField] TMP_Dropdown _roomFilter, _typeFilter, _styleFilter;
    enum _typeOfTags { tagRoom, tagType, tagStyle }

    private void Awake()
    {
        //Initialize Dropdowns
        InitDropDown(_roomFilter, _typeOfTags.tagRoom);
        InitDropDown(_typeFilter, _typeOfTags.tagType);
        InitDropDown(_styleFilter, _typeOfTags.tagStyle);
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
                    filter.options.Add(new TMP_Dropdown.OptionData() { text = tagName });
                }
                break;

            case _typeOfTags.tagType:
                foreach (GMStatic.tagStyle tagElement in Enum.GetValues(typeof(GMStatic.tagStyle)))
                {
                    string tagName = tagElement.ToString();
                    filter.options.Add(new TMP_Dropdown.OptionData() { text = tagName });
                }
                break;

            case _typeOfTags.tagStyle:
                foreach (GMStatic.tagType tagElement in Enum.GetValues(typeof(GMStatic.tagType)))
                {
                    string tagName = tagElement.ToString();
                    filter.options.Add(new TMP_Dropdown.OptionData() { text = tagName });
                }
                break;

            default:
                Debug.LogError("Can't find style in switch/case");
                break;
        }
    }


    public void ApplyFilter()
    {
        
    }
}
