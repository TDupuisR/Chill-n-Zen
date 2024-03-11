using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarUIDisplay : MonoBehaviour
{
    [SerializeField] List<Image> _starImgList;
    [SerializeField] List<GameObject> _cadenasImgList;
    [Space(5)]
    [SerializeField] Sprite _lockedStar;
    [SerializeField] Sprite _unlockedStar;

    private void Awake()
    {
        foreach(Image start in _starImgList)
        {
            start.sprite = _lockedStar;
        }
    }

    public void UnlockStar(int number, bool isUnlocked)
    {
        _starImgList[number].sprite = isUnlocked ? _unlockedStar : _lockedStar;
    }

    public void UnlockOtherStars(bool unlock)
    {
        foreach (GameObject gO in _cadenasImgList)
        {
            gO.SetActive(!unlock);
        }
    }
}
