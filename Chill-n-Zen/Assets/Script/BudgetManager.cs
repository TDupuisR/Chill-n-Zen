using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BudgetManager : MonoBehaviour
{
    [SerializeField] int _currentBudget;

    public int CurrentBudget
    {
        get => _currentBudget;
        set
        {
            _currentBudget = value;

            _onBudgetChanged?.Invoke(_currentBudget);
        }
    }

    public Action<int> _onBudgetChanged;

    private void Start()
    {
        _onBudgetChanged?.Invoke(_currentBudget);
    }
    private void OnEnable()
    {
        TileSystem.OnItemAdded += RemoveToBudget;
        TileSystem.OnItemRemoved += AddToBudget;
    }
    private void OnDisable()
    {
        TileSystem.OnItemAdded -= RemoveToBudget;
        TileSystem.OnItemRemoved -= AddToBudget;

    }

    private void AddToBudget(Item item)
    {
        CurrentBudget += item.price;
    }
    private void RemoveToBudget(Item item)
    {
        CurrentBudget -= item.price;
    }
    public bool IsOutOfBudget()
    {
        return CurrentBudget < 0;
    }
}
