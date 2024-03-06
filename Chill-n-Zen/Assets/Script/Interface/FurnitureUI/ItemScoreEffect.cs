using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemScoreEffect : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TMP_Text _text;
    [SerializeField] Transform _textTransform;

    [Header("Appear Text")]
    [SerializeField] float _secondsBTWText;

    [Header("Translation")]
    [SerializeField] float _translationDuration;
    [SerializeField] AnimationCurve _translationCurve;


    public Vector3 TextTransformPosition { get => _textTransform.position; set => _textTransform.position = value; }
    public Vector3 EndingPosition { get; set; }
    public string TextToDisplay { get ; set; }

    public void StartEffect()
    {
        StartCoroutine(EffectAnimation());
    }
    IEnumerator EffectAnimation()
    {
        //Add Text & make a sound
        _text.text = TextToDisplay;

        //Translation to progression slider
        Vector3 startingPosition = _textTransform.position;
        float timeElapsed = 0.0f;
        while (timeElapsed < _translationDuration)
        {
            float lerpProgression = timeElapsed / _translationDuration;
            _textTransform.position = Vector2.Lerp(startingPosition, EndingPosition, _translationCurve.Evaluate(lerpProgression));
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        //Destroy object when done
        Destroy(gameObject);
    }
}
