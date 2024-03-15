using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int LevelNumber { get; set; }
    public int ScoreToReach { get; set; }

    public static Action OnFinishInitialization;
}
