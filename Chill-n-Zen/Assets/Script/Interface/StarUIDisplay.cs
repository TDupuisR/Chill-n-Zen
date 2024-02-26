using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarUIDisplay : MonoBehaviour
{
    [SerializeField] List<Image> _starImgList;
    [SerializeField] List<float> _starMilestone;
    [Space(5)]
    [SerializeField] Sprite _lockedStar;
    [SerializeField] Sprite _unlockedStar;

    private void Awake()
    {
        if (_starImgList.Count != _starMilestone.Count)
        {
            Debug.LogWarning("Warning ! Le nombre d'étoiles n'est pas le même que le nombre de milestone à atteindre !");
        }
    }

    public void CheckForNewMilestone(float value)
    {
        for(int i=0 ; i < _starMilestone.Count; i++)
        {
            if(value >= _starMilestone[i])
            {
                _starImgList[i].sprite = _unlockedStar;
            }
            else
            {
                _starImgList[i].sprite = _lockedStar;
            }
        }
    }
}
