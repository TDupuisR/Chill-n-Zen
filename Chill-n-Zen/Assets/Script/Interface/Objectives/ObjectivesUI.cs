/*bite*/ using GameManagerSpace;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ObjectivesUI : MonoBehaviour
{
    public static ObjectivesUI Instance;

    [Header("References")]
    [SerializeField] GameObject _levelCompletedManager;
    [SerializeField] ScoreToReach _scoreToReach;
    [SerializeField] StarUIDisplay _starUI;
    [SerializeField] Transform _buttonObjectivesRect;
    [SerializeField] GameObject _checkboxPrefab;
    [SerializeField] Transform _objectPrimaryParent;
    [SerializeField] Transform _objectSecondaryParent;
    [SerializeField] SwipeScrollbar _scroll;

    [Header("Reference fill")]
    [SerializeField] RectTransform _fillRect;
    [SerializeField] float _spaceFillFactor;

    [Header("Objectives fields")]
    [SerializeField] float _spaceBTWObj;
    [SerializeField] Color _notCompletedColor;
    [SerializeField] Color _completedColor;
    [SerializeField] Sprite _uncheckedSprite;
    [SerializeField] Sprite _checkedSprite;

    List<ObjectivesCheckbox> _primaryObjectives = new List<ObjectivesCheckbox>();
    List<ObjectivesCheckbox> _secondaryObjectives = new List<ObjectivesCheckbox>();
    bool _hasCompletedPrimaryObjectives;

    [Header("Button glowing")]
    [SerializeField] Image _glowSprite;
    [SerializeField] AnimationCurve _glowCurve;
    [SerializeField] float _glowSpeed;
    Coroutine _glowingRoutine;

    [Header("Obj completed effect")]
    [SerializeField] GameObject _completedEffectPrefab;
    [SerializeField] float _timeBTWEffects;
    List<ObjectivesCheckbox> _objectiveEffectQueueList = new List<ObjectivesCheckbox>();
    bool _isEffectQueueBusy;
    Coroutine _effectQueueCoroutine;

    [Header("Finish Level")]
    [SerializeField] Button _completeLevelButton;

    [Header("UnityEvent")]
    [SerializeField] UnityEvent OnFinishSetup;

    public bool HasPrimaryStar { get; private set; }
    public bool HasSecondaryStar { get; private set; }
    public bool HasScoreStar { get; private set; }

    public static Action OnFinishInitialisation;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError(" (error : 1x0) Too many ObjectivesUI instance ", gameObject);
            Destroy(gameObject);
        }
        Instance = this;

        _glowingRoutine = StartCoroutine(GlowingAnimation());
    }

    private void OnEnable()
    {
        RequestManager.OnFinishInitialisation += InitAllObjectives;
        TileSystem.OnSceneChanged += UpdateAllObjectives;
        //ScoreToReach.OnCheckScore += ;
    }

    private void OnDisable()
    {
        RequestManager.OnFinishInitialisation -= InitAllObjectives;
        TileSystem.OnSceneChanged -= UpdateAllObjectives;
        //ScoreToReach.OnCheckScore -= ;
    }

    private void Start()
    {
        OnFinishSetup?.Invoke();
    }

    void InitAllObjectives()
    {
        List<string> textList = GameManager.requestManager.ReturnDescriptions(true);
        List<bool> valueToSet = GameManager.requestManager.ReturnStatus(true);
        int count = textList.Count;
        InitializeObjectives(textList.Count, _primaryObjectives, _objectPrimaryParent, textList, valueToSet);

        textList = GameManager.requestManager.ReturnDescriptions(false);
        valueToSet = GameManager.requestManager.ReturnStatus(false);
        count += textList.Count;
        InitializeObjectives(textList.Count, _secondaryObjectives, _objectSecondaryParent, textList, valueToSet);

        if(count > 4)
        {
            _scroll.UpdateSize(count / 2 + 1);
            _fillRect.offsetMin = new Vector2(_fillRect.offsetMin.x, -(count * _spaceFillFactor));
        }
        else
        {
            _scroll.UpdateSize(2);
            _fillRect.offsetMin = new Vector2(_fillRect.offsetMin.x, -450);
        }
        
        UpdateAllObjectives();
    }

    void InitializeObjectives(int count, List<ObjectivesCheckbox> objectiveList, Transform objectParent, List<string> textToAdd, List<bool> valueToSet)
    {
        objectiveList.Clear();
        Vector2 currentPosition = new Vector2(-57, 0);
        for (int i=0; i< count; i++)
        {
            GameObject newObj = Instantiate(_checkboxPrefab, objectParent);
            newObj.transform.localPosition = currentPosition;

            ObjectivesCheckbox objScript = newObj.GetComponent<ObjectivesCheckbox>();
            objectiveList.Add(objScript);

            SetObjectiveText(objScript, textToAdd[i]);
            UpdateSingleObjective(objScript, valueToSet[i], true);

            currentPosition -= new Vector2(0, _spaceBTWObj);
        }

        OnFinishInitialisation?.Invoke();
    }

    public void SetObjectiveText(ObjectivesCheckbox objectiveObject, string text)
    {
        if (text == "" || text == null)
        {
            Debug.LogWarning("text is empty");
            objectiveObject.Text.text = "";
            return;
        }
        objectiveObject.Text.text = text;
    }


    private void UpdateAllObjectives()
    {
        //Primary objectives
        List<bool> valueToSet = GameManager.requestManager.ReturnStatus(true);
        if(valueToSet.Count > 0 )
        {
            for (int i = 0; i < valueToSet.Count; i++)
            {
                UpdateSingleObjective(_primaryObjectives[i], valueToSet[i]);
            }
            CheckCompleteObjectives(true, valueToSet);
        }

        //Secondary objectives
        valueToSet = GameManager.requestManager.ReturnStatus(false);
        if(valueToSet.Count > 0)
        {
            for (int i = 0; i < valueToSet.Count; i++)
            {
                UpdateSingleObjective(_secondaryObjectives[i], valueToSet[i]);
            }
            CheckCompleteObjectives(false, valueToSet);
        }

        CheckCompletedScoreObjective(_scoreToReach.IsScoreReached);
    }
    void UpdateSingleObjective(ObjectivesCheckbox objectiveList, bool isValid, bool skipEffect = false)
    {
        Sprite oldSprite = objectiveList.Img.sprite;

        objectiveList.Img.sprite = isValid ? _checkedSprite : _uncheckedSprite;
        objectiveList.Img.color = isValid ? _completedColor : _notCompletedColor;

        if(!skipEffect && oldSprite != objectiveList.Img.sprite)
            AddToEffectList(objectiveList);
    }

    void CheckCompleteObjectives(bool primary, List<bool> objectivesList)
    {
        bool isEverythingComplete = !objectivesList.Contains(false);

        switch (primary)
        {
            case true:
                _starUI.UnlockStar(0, isEverythingComplete);
                _hasCompletedPrimaryObjectives = isEverythingComplete;
                _starUI.UnlockOtherStars(isEverythingComplete);
                UnlockFinishButton(isEverythingComplete);
                HasPrimaryStar = isEverythingComplete;
                break;
            case false:
                if(_hasCompletedPrimaryObjectives)
                {
                    _starUI.UnlockStar(1, isEverythingComplete);
                    HasSecondaryStar = isEverythingComplete;
                }
                break;
        }
    }
   
    void CheckCompletedScoreObjective(bool unlocked)
    {
        if (_hasCompletedPrimaryObjectives)
        {
            _starUI.UnlockStar(2, unlocked);
            HasScoreStar = unlocked;
        }
    }

    public List<string> GetMissingSecondaryObjectives()
    {
        List<bool> secondaryObjectives = GameManager.requestManager.ReturnStatus(false);
        List<string> secondarySolutions = GameManager.requestManager.ReturnSolution(false);
        List<string> solutionList = new List<string>();
        for (int i = 0; i < secondaryObjectives.Count; i++)
        {
            if (!secondaryObjectives[i])
                solutionList.Add(secondarySolutions[i]);
        }

        return solutionList;
    }
    public void InvertButtonSprite() => _buttonObjectivesRect.localScale = new Vector3(-_buttonObjectivesRect.localScale.x, 1,1);

    #region Complete Level
    /*
     Complete Level Functions
     */
    void UnlockFinishButton(bool unlock) => _completeLevelButton.interactable = unlock;
    public void CompleteLevel()
    {
        GameManager.saveData.SetStar(GameManager.levelManager.LevelNumber, HasPrimaryStar, HasSecondaryStar, HasScoreStar);
        _levelCompletedManager.SetActive(true);
    }
    #endregion

    #region Glowing animation function
    /*
    Glowing animation function
     */
    public void StopGlowing()
    {
        StopCoroutine(_glowingRoutine);
        _glowSprite.color = new Color(1, 1, 1, 0);
    }

    IEnumerator GlowingAnimation()
    {
        while(true)
        {
            float lerpProgression = Mathf.Repeat(Time.time * _glowSpeed, 1);
            float opacity = Mathf.Lerp(0,1,_glowCurve.Evaluate(lerpProgression));
            _glowSprite.color = new Color(1,1,1, opacity);
            
            yield return new WaitForFixedUpdate();
        }
    }
    #endregion

    #region Objective effect
    [Button]
    public void TestObjectivesEffect() => ObjectiveCompletedEffect(_primaryObjectives[0]);
    [Button]
    public void TestObjectivesQueueEffect() => AddToEffectList(_primaryObjectives[0]);
    public void AddToEffectList(ObjectivesCheckbox newObjective)
    {
        _objectiveEffectQueueList.Add(newObjective);

        if (!_isEffectQueueBusy)
            _effectQueueCoroutine = StartCoroutine(ObjectiveEffectQueue());
    }

    public void ObjectiveCompletedEffect(ObjectivesCheckbox objectiveToDisplay)
    {
        GameObject newEffect = Instantiate(_completedEffectPrefab, Vector3.zero, Quaternion.identity);
        ObjectivesCompletedEffect effectScript = newEffect.GetComponent<ObjectivesCompletedEffect>();
        effectScript.TextToImplement = objectiveToDisplay.Text;
        effectScript.TextToImplement.text = objectiveToDisplay.Text.text;
        effectScript.ImgToImplement = objectiveToDisplay.Img.sprite;
        effectScript.LaunchEffect();
    }


    IEnumerator ObjectiveEffectQueue()
    {
        _isEffectQueueBusy = true;
        while (_objectiveEffectQueueList.Count > 0)
        {
            ObjectiveCompletedEffect(_objectiveEffectQueueList[0]);
            _objectiveEffectQueueList.RemoveAt(0);
            yield return new WaitForSeconds(_timeBTWEffects);
        }
        _isEffectQueueBusy = false;
    }
    #endregion
}
