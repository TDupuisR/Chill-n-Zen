using GameManagerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGetSound : MonoBehaviour
{
    private void Start()
    {
        GameManager.audioManager.PlayMusic("Level");
    }

    public void PlaySound(string sound)
    {
        GameManager.audioManager.PlaySound(sound);
    }
}
