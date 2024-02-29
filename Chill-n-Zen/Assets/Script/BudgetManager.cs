using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BudgetManager : MonoBehaviour
{
    [SerializeField] int _currentBudget;
    [SerializeField] int _maxBudget;

    public int CurrentBudget
    {
        get => _currentBudget;
        set
        {
            _currentBudget = value;
            _onBudgetChanged?.Invoke(_currentBudget, _maxBudget);
        }
    }

    public int MaxBudget
    {
        get => _maxBudget;
        set
        {
            _maxBudget = value;
            _onBudgetChanged?.Invoke(_currentBudget, _maxBudget);
        }
    }

    public Action<int, int> _onBudgetChanged;

    private void Awake()
    {
        _onBudgetChanged?.Invoke(_currentBudget, _maxBudget);
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

}
