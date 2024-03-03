using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesUI : MonoBehaviour
{
    [Header("Objectives")]
    [SerializeField] List<Image> _primaryObjectivesImg;
    [SerializeField] List<TMP_Text> _primaryObjectivesText;
    [SerializeField] List<Image> _secondaryObjectivesImg;
    [SerializeField] List<TMP_Text> _secondaryObjectivesText;
    [SerializeField] Color _notCompletedColor;
    [SerializeField] Color _completedColor;
    [SerializeField] Sprite _uncheckedSprite;
    [SerializeField] Sprite _checkedSprite;

    int _numberOfPrimaryObjectves;
    int _numberOfSecondaryObjectves;

    [Header("Finish Level")]
    [SerializeField] Button _completeLevelButton;

    public int NumberOfPrimaryObjectives
    {
        get => _numberOfPrimaryObjectves;
        set
        {
            _numberOfPrimaryObjectves = value;
            SetNumberOfObjectives(_numberOfPrimaryObjectves, _primaryObjectivesText, UpdatePrimaryObjectives);
        }
    }

    public int NumberOfSecondaryObjectives
    {
        get => _numberOfSecondaryObjectves;
        set
        {
            _numberOfSecondaryObjectves = value;
            SetNumberOfObjectives(_numberOfSecondaryObjectves, _secondaryObjectivesText, UpdateSecondaryObjectives);
        }
    }

    void SetNumberOfObjectives(int count, List<TMP_Text> textList, Action<int, bool> UpdateObjective)
    {
        for(int i=0; i< textList.Count; i++)
        {
            bool isActive = i > count;
            textList[i].gameObject.SetActive(isActive);
            UpdateObjective(i, false);
        }
    }

    public void SetPrimaryObjectiveText(int index, string text)
    {
        if (text == "" || text == null)
        {
            Debug.LogWarning("text is empty");
            _primaryObjectivesText[index].text = "";
            return;
        }
        _primaryObjectivesText[index].text = text;
    }

    public void SetSecondaryObjectiveText(int index, string text)
    {
        if(text == "" || text == null)
        {
            Debug.LogWarning("text is empty");
            _secondaryObjectivesText[index].text = "";
            return;
        }
        _secondaryObjectivesText[index].text = text;
    }

    public void UpdatePrimaryObjectives(int index, bool isValid)
    {
        _primaryObjectivesImg[index].sprite = isValid ? _checkedSprite : _uncheckedSprite;
        _primaryObjectivesText[index].color = isValid ? _completedColor : _notCompletedColor;
    }

    public void UpdateSecondaryObjectives(int index, bool isValid)
    {
        _secondaryObjectivesImg[index].sprite = isValid ? _checkedSprite : _uncheckedSprite;
        _secondaryObjectivesText[index].color = isValid ? _completedColor : _notCompletedColor;
    }


    /*
     Complete Level Functions
     */
    public void UnlockButton()
    {
        _completeLevelButton.interactable = true;
    }

    public void CompleteLevel()
    {
        throw new System.NotImplementedException();
    }
}
