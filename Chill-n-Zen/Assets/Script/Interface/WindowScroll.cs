using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowScroll : MonoBehaviour
{
    [SerializeField] RectTransform _backgroundTransform;
    [SerializeField] float _animationDuration;
    Vector3 _backgroundStartPosition;
    Coroutine _animationRoutine;

    private void Awake()
    {
        _backgroundStartPosition = _backgroundTransform.anchoredPosition;
        print(_backgroundStartPosition.x);
    }

    private void OnValidate()
    {
        if(_animationDuration <= 0)
        {
            Debug.LogWarning("animationSpeed peut pas être négative");
            _animationDuration = 1;
        }
    }

    public void StartScroll()
    {
        _animationRoutine = StartCoroutine(SelectionWindowAnimationRoutine());
    }

    IEnumerator SelectionWindowAnimationRoutine()
    {
        float timeElapsed = 0.0f;
        Vector2 finalPosition = new Vector2(-_backgroundStartPosition.x, _backgroundStartPosition.y);

        while (timeElapsed < _animationDuration)
        {
            float lerpProgression = timeElapsed / _animationDuration;
            _backgroundTransform.anchoredPosition = Vector2.Lerp(_backgroundStartPosition, finalPosition, lerpProgression);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _backgroundStartPosition = finalPosition;
    }
}
