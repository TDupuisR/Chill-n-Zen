using GameManagerSpace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BudgetSlider : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] TMP_Text _text;

    private void OnEnable()
    {
        GameManager.budgetManager._onBudgetChanged += UpdateInterface;
    }
    private void OnDisable()
    {
        GameManager.budgetManager._onBudgetChanged -= UpdateInterface;
    }


    void UpdateInterface(int currentBudget, int maxBudget)
    {
        _slider.maxValue = maxBudget;
        _slider.value = currentBudget;
        _text.text = _slider.value.ToString() + " Cr";
    }
}
