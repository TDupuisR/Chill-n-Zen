using GameManagerSpace;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreToReach : MonoBehaviour
{
    [SerializeField] StarUIDisplay _starUI;
    [SerializeField] TMP_Text _text;
    int _score = 0;

    public bool IsScoreReached { get; private set; }

    public static Action<bool> OnCheckScore;

    private void OnEnable()
    {
        TileSystem.OnScoreChanged += CheckScore;
        LevelManager.OnFinishInitialization += Initialisation;

    }
    private void OnDisable()
    {
        TileSystem.OnScoreChanged -= CheckScore;
        LevelManager.OnFinishInitialization -= Initialisation;
    }

    public void Initialisation()
    {
        _score = GameManager.levelManager.ScoreToReach;

        _text.text = _score.ToString() + " pts";
    }

    private void CheckScore(int newScore)
    {
        if (_score <= newScore)
            IsScoreReached = true;
        else
            IsScoreReached = false;

        OnCheckScore?.Invoke(IsScoreReached);
    }
    
}
