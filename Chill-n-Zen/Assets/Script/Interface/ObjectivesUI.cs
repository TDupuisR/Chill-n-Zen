using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesUI : MonoBehaviour
{    
    [SerializeField] List<Image> _primaryObjectivesImg;
    [SerializeField] List<TMP_Text> _primaryObjectivesText;

    [SerializeField] List<Image> _secondaryObjectivesImg;
    [SerializeField] List<TMP_Text> _secondaryObjectivesText;

    [SerializeField] Color _notCompletedColor;
    [SerializeField] Color _completedColor;

    [SerializeField] Sprite _uncheckedSprite;
    [SerializeField] Sprite _checkedSprite;

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
}
