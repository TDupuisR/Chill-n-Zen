using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContemplativeButton : MonoBehaviour
{
    [SerializeField] GameObject _gameplayObjects;
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] Button _button;
    [SerializeField] float _fadeDuration;
    bool _active = false;
    Coroutine _fadeCoroutine;

    private void OnEnable()
    {
        ItemSpawner.onItemSelected += DisableButton;
        FurnitureWindowManager.wasItemPlaced += EnableButton;
    }
    private void OnDisable()
    {
        ItemSpawner.onItemSelected -= DisableButton;
        FurnitureWindowManager.wasItemPlaced -= EnableButton;
    }

    void EnableButton() { _button.interactable = true; }
    void DisableButton() { _button.interactable = false; }

    public void Activate()
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(fadeInOut(_active));
        _active = !_active;
    }

    IEnumerator fadeInOut(bool isActive) 
    {
        if(isActive)
            _gameplayObjects.SetActive(true);

        float timeElapsed = 0.0f;
        int direction = isActive ? 0 : 1;
        // 0 = fade in, 1 = fade out
        while (timeElapsed < _fadeDuration)
        {
            float lerpProgression = timeElapsed / _fadeDuration;
            if (direction == 1) 
                lerpProgression = 1 - lerpProgression;
            print(lerpProgression);
            _canvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, lerpProgression);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        if (!isActive)
            _gameplayObjects.SetActive(false);
    }
}
