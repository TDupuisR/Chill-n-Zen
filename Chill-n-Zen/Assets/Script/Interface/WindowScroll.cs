using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowScroll : MonoBehaviour
{
    [SerializeField] RectTransform _backgroundTransform;
    [SerializeField] AnimationCurve _animationCurve;
    [SerializeField] float _animationDuration;
    [SerializeField] float _boundaryActionZone; //For 1280x720 screen
    [SerializeField] bool _isVerticalScroll; 
    Vector3 _backgroundStartPosition;
    Vector3 _backgroundStartRectTransformPosition;
    Coroutine _animationRoutine;
    bool _displayed;

    private void Awake()
    {
        _backgroundStartPosition = _backgroundTransform.anchoredPosition;
        _backgroundStartRectTransformPosition = _backgroundTransform.GetComponent<RectTransform>().position;
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
        _animationRoutine = StartCoroutine(SelectionWindowAnimationRoutine(_isVerticalScroll));
    }

    public void HideIfDisplayed()
    {
        if(_displayed)
        {
            StartScroll();
        }
    }

    IEnumerator SelectionWindowAnimationRoutine(bool isVertical)
    {
        float timeElapsed = 0.0f;
        Vector2 finalPosition;

        if (isVertical)
        {
            finalPosition = new Vector2(_backgroundStartPosition.x, -_backgroundStartPosition.y);
        }
        else
        {
            finalPosition = new Vector2(-_backgroundStartPosition.x, _backgroundStartPosition.y);
        }

        while (timeElapsed < _animationDuration)
        {
            float lerpProgression = timeElapsed / _animationDuration;
            _backgroundTransform.anchoredPosition = Vector2.Lerp(_backgroundStartPosition, finalPosition, _animationCurve.Evaluate(lerpProgression));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        _displayed = !_displayed;
        _backgroundStartPosition = finalPosition;
    }

    public void UpdateCamActionZoneDown(UIGetCam _cam)
    {
        _cam.cam.ChangeDownLeftCamActionZone(new Vector2(0.0f, GiveAdapativeBoundaryActionZoneY())); 
    }
    public void UpdateCamActionZoneUp(UIGetCam _cam)
    {
        _cam.cam.ChangeUpRightCamActionZone(new Vector2(Screen.width, GiveAdapativeBoundaryActionZoneY()));
    }
    public void UpdateCamActionZoneLeft(UIGetCam _cam)
    {
        _cam.cam.ChangeDownLeftCamActionZone(new Vector2(GiveAdapativeBoundaryActionZoneX(), 0.0f));
    }
    public void UpdateCamActionZoneRight(UIGetCam _cam)
    {
        _cam.cam.ChangeUpRightCamActionZone(new Vector2(GiveAdapativeBoundaryActionZoneX(), Screen.height));
    }
    float GiveAdapativeBoundaryActionZoneX() => _boundaryActionZone * Screen.width / 1280.0f;
    float GiveAdapativeBoundaryActionZoneY() => _boundaryActionZone * Screen.height / 720.0f;
}
