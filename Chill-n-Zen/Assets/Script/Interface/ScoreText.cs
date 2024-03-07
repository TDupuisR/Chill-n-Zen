using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    int _score = 0;

    public bool IsScoreReached { get; private set; }

    private void OnEnable()
    {
        TileSystem.OnScoreChanged += CheckScore;
    }
    private void OnDisable()
    {
        TileSystem.OnScoreChanged -= CheckScore;
    }

    public void Initialisation(int score)
    {
        _score = score;

        _text.text = _score.ToString();
    }

    private void CheckScore(int newScore)
    {
        if (_score <= newScore)
            IsScoreReached = true;
        else
            IsScoreReached = false;
    }
    
}
