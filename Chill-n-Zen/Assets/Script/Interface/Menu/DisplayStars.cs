using GameManagerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayStars : MonoBehaviour
{
    [SerializeField] List<Image> _starList;
    [SerializeField] int _level;
    [SerializeField] Sprite _unlockedSprite;


    private void Start()
    {
        if(PlayerPrefs.HasKey("star" + _level))
        {
            List<bool> levelStars = GameManager.saveData.LoadStar(_level);

            for(int i = 0; i < levelStars.Count; i++)
            {
                if (levelStars[i])
                    _starList[i].sprite = _unlockedSprite;
            }
        }
    }
}
