using GameManagerSpace;
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
        if (GameManager.budgetManager != null)
        {
            GameManager.budgetManager._onBudgetChanged += UpdateInterface;
        } //Needed because of timing issues (budgetManager is null on first OnEnable)

    }
    private void OnDisable()
    {
        GameManager.budgetManager._onBudgetChanged -= UpdateInterface;
    }

    private void Start()
    {
        //initialize Budget
        _defaultBudget = GameManager.budgetManager.CurrentBudget;
        GameManager.budgetManager._onBudgetChanged += UpdateInterface; //Needed because of timing issues (budgetManager is null on first OnEnable)
    }

    void UpdateInterface(int currentBudget)
    {
        _slider.maxValue = _defaultBudget;
        _slider.value = currentBudget;
        _text.text = currentBudget.ToString() + " Cr";
    }
}
