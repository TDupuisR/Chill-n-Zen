using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletedManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] List<Image> _starImg;
    [SerializeField] TMP_Text _finalScoreText;
    [Space(3)]
    [SerializeField] WindowScroll _windowScroll;
    [SerializeField] List<GameObject> _objectsToDisable;
    [Space(3)]
    [SerializeField] Sprite _lockedStar;
    [SerializeField] Sprite _unlockedStar;


    bool _initialize = true;

    public void OnEnable()
    {
        if(_initialize)
        {
            _initialize = false;
            gameObject.SetActive(false);
            return;
        }


        foreach(GameObject obj in _objectsToDisable)
        {
            obj.SetActive(false);
        }

        bool[] starState = new bool[3] { ObjectivesUI.Instance.HasPrimaryStar, ObjectivesUI.Instance.HasSecondaryStar, ObjectivesUI.Instance.HasScoreStar};
        for(int i = 0; i < starState.Length; i++)
        {
            _starImg[i].sprite = starState[i] ? _unlockedStar : _lockedStar;
        }

        //_finalScoreText.text = "Final score : ";
        _windowScroll.StartScroll();
    }
}
