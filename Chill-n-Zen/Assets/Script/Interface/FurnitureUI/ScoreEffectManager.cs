using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreEffectManager : MonoBehaviour
{
    [SerializeField] GameObject _scoreEffectPrefab;
    [SerializeField] Transform _endingPositionTransform;

    [SerializeField] Color _scoreColor;
    [SerializeField] Color _comboColor;

    public void SpawnEffect(Vector3 startingPosition, int score, bool isCombo)
    {
        GameObject newEffect = Instantiate(_scoreEffectPrefab, startingPosition, Quaternion.identity);
        ItemScoreEffect effectScript = newEffect.GetComponent<ItemScoreEffect>();

        string textToDisplay = "";
        if (isCombo)
            textToDisplay = "<color=#" + ColorUtility.ToHtmlStringRGB(_scoreColor) + "> Combo : " + score + " points !";
        else
            textToDisplay = "<color=#" + ColorUtility.ToHtmlStringRGB(_comboColor) + ">+ " + score + " points";

        effectScript.TextTransformPosition = startingPosition;
        effectScript.EndingPosition = _endingPositionTransform.position;
        effectScript.TextToDisplay = textToDisplay;
        effectScript.StartEffect();
    }

    [Button]
    public void TestDisplayEffect() => SpawnEffect(Vector3.zero, 80, false);
    [Button]
    public void TestDisplayEffectCombo() => SpawnEffect(Vector3.zero, 500, true);
}
