using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform _buttonObjectivesRect;
    [SerializeField] GameObject _checkboxPrefab;
    [SerializeField] Transform _objectPrimaryParent;
    [SerializeField] Transform _objectSecondaryParent;

    [Header("Objectives fields")]
    [SerializeField] float _spaceBTWObj;
    [SerializeField] Color _notCompletedColor;
    [SerializeField] Color _completedColor;
    [SerializeField] Sprite _uncheckedSprite;
    [SerializeField] Sprite _checkedSprite;

    List<ObjectivesCheckbox> _primaryObjectives = new List<ObjectivesCheckbox>();
    List<ObjectivesCheckbox> _secondaryObjective = new List<ObjectivesCheckbox>();
    int _numberOfPrimaryObjectives;
    int _numberOfSecondaryObjectives;

    [Header("Button glowing")]
    [SerializeField] Image _glowSprite;
    [SerializeField] AnimationCurve _glowCurve;
    [SerializeField] float _glowSpeed;
    Coroutine _glowingRoutine;

    [Header("Obj completed effect")]
    [SerializeField] GameObject _completedEffectPrefab;


    [Header("Finish Level")]
    [SerializeField] Button _completeLevelButton;

    public int NumberOfPrimaryObjectives
    {
        get => _numberOfPrimaryObjectives;
        set
        {
            _numberOfPrimaryObjectives = value;
            SetNumberOfObjectives(_numberOfPrimaryObjectives, _primaryObjectives, _objectPrimaryParent);
        }
    }

    public int NumberOfSecondaryObjectives
    {
        get => _numberOfSecondaryObjectives;
        set
        {
            _numberOfSecondaryObjectives = value;
            SetNumberOfObjectives(_numberOfSecondaryObjectives, _secondaryObjective, _objectSecondaryParent);
        }
    }

    private void Awake()
    {
        _glowingRoutine = StartCoroutine(GlowingAnimation());
    }

    void SetNumberOfObjectives(int count, List<ObjectivesCheckbox> objectiveList, Transform objectParent)
    {
        objectiveList.Clear();
        Vector2 currentPosition = Vector2.zero;
        for(int i=0; i< count; i++)
        {
            GameObject newObj = Instantiate(_checkboxPrefab, objectParent);   
            newObj.transform.localPosition = currentPosition;

            ObjectivesCheckbox objScript = newObj.GetComponent<ObjectivesCheckbox>();
            objectiveList.Add(objScript);

            UpdateObjective(objScript, false);

            currentPosition -= new Vector2(0, _spaceBTWObj);
        }
    }
    [Button]
    public void TESTSPAWNMERDE() => NumberOfPrimaryObjectives = 5;

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


    void UpdateObjective(ObjectivesCheckbox objectiveList, bool isValid)
    {
        objectiveList.Img.sprite = isValid ? _checkedSprite : _uncheckedSprite;
        objectiveList.Img.color = isValid ? _completedColor : _notCompletedColor;

        ObjectiveCompletedEffect(objectiveList);
    }

    public void InvertButtonSprite() => _buttonObjectivesRect.localScale = new Vector3(-_buttonObjectivesRect.localScale.x, 1,1);

    #region Complete Level
    /*
     Complete Level Functions
     */
    public void UnlockButton()
    {
        _completeLevelButton.interactable = true;
    }

    public void CompleteLevel()
    {
        throw new System.NotImplementedException();
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

    [Button]
    public void TestObjectivesEffect() => ObjectiveCompletedEffect(_primaryObjectives[0]);
    public void ObjectiveCompletedEffect(ObjectivesCheckbox objectiveToDisplay)
    {
        GameObject newEffect = Instantiate(_completedEffectPrefab, Vector3.zero, Quaternion.identity);
        ObjectivesCompletedEffect effectScript = newEffect.GetComponent<ObjectivesCompletedEffect>();
        effectScript.TextToImplement = objectiveToDisplay.Text;
        effectScript.ImgToImplement = objectiveToDisplay.Img.sprite;
        effectScript.ObjectiveCompletedEffect();
    }
}
