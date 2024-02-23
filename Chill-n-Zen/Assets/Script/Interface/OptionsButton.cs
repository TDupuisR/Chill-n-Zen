using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsButton : MonoBehaviour
{
    [SerializeField] Transform _furniturePosition;
    [SerializeField] Transform _restartPosition;
    [SerializeField] Button _furnitureButton;
    [SerializeField] Button _restartButton;
    [SerializeField] AnimationCurve _animationCurve;
    [SerializeField] float _duration;
    Coroutine _animationCoroutine;
    bool _isDeployed = false;


    public void DeployButtons()
    {
        _animationCoroutine = StartCoroutine(DeployButtonsAnimation(_duration));
    }

    IEnumerator DeployButtonsAnimation(float _animationDuration)
    {
        ActivateButtons(false);

        float timeElapsed = 0.0f;
        while (timeElapsed < _animationDuration)
        {
            float lerpProgression = timeElapsed / _animationDuration;
            float progression = _isDeployed ? 1 - lerpProgression : lerpProgression;
            _furnitureButton.transform.position = Vector2.Lerp(transform.position, _furniturePosition.position, _animationCurve.Evaluate(progression));
            _restartButton.transform.position = Vector2.Lerp(transform.position, _restartPosition.position, _animationCurve.Evaluate(progression));

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _isDeployed = !_isDeployed;
        if (_isDeployed)
        {
            ActivateButtons(true);
        }
        else
        {
            Vector2 reployPosition = transform.position;
            reployPosition.y = -Screen.height;
            _furnitureButton.transform.position = reployPosition;
            _restartButton.transform.position = reployPosition;
        }
    }
    void ActivateButtons(bool activate)
    {
        _furnitureButton.interactable = activate;
        _restartButton.interactable = activate;
    }
}
