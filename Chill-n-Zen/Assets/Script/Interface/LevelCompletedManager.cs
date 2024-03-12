using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletedManager : MonoBehaviour
{
    [SerializeField] ScoreText _scoreTxt;

    [Header("References")]
    [SerializeField] List<Image> _starImg;
    [SerializeField] TMP_Text _finalScoreText;
    [Space(3)]
    [SerializeField] WindowScroll _windowScroll;
    [SerializeField] List<GameObject> _objectsToDisable;
    [Space(3)]
    [SerializeField] Sprite _lockedStar;
    [SerializeField] Sprite _unlockedStar;
    [Space(3)]
    [SerializeField] GameObject _solutionObject;
    [SerializeField] TMP_Text _solutionText;

    bool _initialize = true;

    public void OnEnable()
    {
        if(_initialize)
        {
            _initialize = false;
            gameObject.SetActive(false);
            return;
        }
        //Disable other UI objects
        foreach(GameObject obj in _objectsToDisable)
        {
            obj.SetActive(false);
        }
        //Display stars
        bool[] starState = new bool[3] { ObjectivesUI.Instance.HasPrimaryStar, ObjectivesUI.Instance.HasSecondaryStar, ObjectivesUI.Instance.HasScoreStar};
        for(int i = 0; i < starState.Length; i++)
        {
            _starImg[i].sprite = starState[i] ? _unlockedStar : _lockedStar;
        }
        //Display final score
        _finalScoreText.text = "Final score : " + _scoreTxt.CurrentScore;
        //DisplaySolution
        List<string> solutionList = ObjectivesUI.Instance.GetMissingSecondaryObjectives();
        _solutionObject.SetActive(solutionList.Count > 0);
        _solutionText.text = "";
        if (solutionList.Count > 0)
        {
            foreach (string solution in solutionList)
            {
                _solutionText.text += "- " + solution + "\n";
            }
        }


        _windowScroll.StartScroll();
    }
}
