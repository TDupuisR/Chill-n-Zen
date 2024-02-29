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
            if(_currentBudget < 0)
            {
                Debug.LogError("Current budget can't be negative !");
                _currentBudget = 0;
            }

            _onBudgetChanged?.Invoke(_currentBudget, _maxBudget);
        }
    }

    public int MaxBudget
    {
        get => _maxBudget;
        set
        {
            _maxBudget = value;
            if (_maxBudget < 0)
            {
                Debug.LogError("maximum budget can't be negative !");
                _maxBudget = 0;
            }

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
        TileSystem.OnItemAdded += AddToBudget;
        TileSystem.OnItemRemoved += RemoveToBudget;
    }
    private void OnDisable()
    {
        TileSystem.OnItemAdded -= AddToBudget;
        TileSystem.OnItemRemoved -= RemoveToBudget;

    }

    private void AddToBudget(Item item)
    {
        CurrentBudget += item.price;
    }
    private void RemoveToBudget(Item item)
    {
        CurrentBudget -= item.price;
    }
    public bool ChkIfHasBudget(int price)
    {
        return (MaxBudget - CurrentBudget) > price;
    }
}
