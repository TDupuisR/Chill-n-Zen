using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    [SerializeField] TMP_Text _text;

    public int CurrentScore {  get; private set; }

    private void OnEnable()
    {
        TileSystem.OnScoreChanged += ActualizeScore;
    }
    private void OnDisable()
    {
        TileSystem.OnScoreChanged -= ActualizeScore;
    }

    private void ActualizeScore(int score)
    {
        CurrentScore = score;
        _text.text = "Score : " + CurrentScore + " pts";
    }
}
