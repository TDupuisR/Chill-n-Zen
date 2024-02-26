using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BudgetSlider : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] TMP_Text _text;
    [SerializeField] int _maxBudget;
    [SerializeField] int _currentBudget;

    public int CurrentBudget
    {
        get => _currentBudget;
        set
        {
            _currentBudget = value;
            UpdateInterface();
        }
    }

    void UpdateInterface()
    {
        _slider.maxValue = _maxBudget;
        _slider.value = _currentBudget;
        _text.text = _slider.value.ToString() + " Cr";
    }
}
