using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
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
        _text.text = "Score : " + score + " pts";
    }
}
