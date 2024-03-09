using GameManagerSpace;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BudgetSlider : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] TMP_Text _text;
    int _defaultBudget;

    private void OnEnable()
    {
        GameManager.budgetManager.OnSetDefaultBudget += InitializeBudget;
        GameManager.budgetManager._onBudgetChanged += UpdateInterface;
    }

    private void OnDisable()
    {
        GameManager.budgetManager._onBudgetChanged -= UpdateInterface;
    }

    private void InitializeBudget()
    {
        _defaultBudget = GameManager.budgetManager.CurrentBudget;
        UpdateInterface(_defaultBudget);
    }

    void UpdateInterface(int currentBudget)
    {
        _slider.maxValue = _defaultBudget;
        _slider.value = currentBudget;
        _text.text = currentBudget.ToString() + " Cr";
    }
}
