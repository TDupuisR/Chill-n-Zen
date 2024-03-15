using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BudgetManager : MonoBehaviour
{
    int _currentBudget;

    public int CurrentBudget
    {
        get => _currentBudget;
        set
        {
            _currentBudget = value;

            OnBudgetChanged?.Invoke(_currentBudget);
        }
    }

    public static Action OnSetDefaultBudget;
    public static Action<int> OnBudgetChanged;

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
        return CurrentBudget <= 0;
    }
}
