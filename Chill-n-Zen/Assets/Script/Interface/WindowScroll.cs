using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowScroll : MonoBehaviour
{
    [SerializeField] RectTransform _backgroundTransform;
    [SerializeField] AnimationCurve _animationCurve;
    [SerializeField] float _animationDuration;
    [SerializeField] float _boundaryActionZoneX; //For 1280x720 screen
    Vector3 _backgroundStartPosition;
    Vector3 _backgroundStartRectTransformPosition;
    Coroutine _animationRoutine;


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
        _animationRoutine = StartCoroutine(SelectionWindowAnimationRoutine());
    }

    IEnumerator SelectionWindowAnimationRoutine()
    {
        float timeElapsed = 0.0f;
        Vector2 finalPosition = new Vector2(-_backgroundStartPosition.x, _backgroundStartPosition.y);

        while (timeElapsed < _animationDuration)
        {
            float lerpProgression = timeElapsed / _animationDuration;
            _backgroundTransform.anchoredPosition = Vector2.Lerp(_backgroundStartPosition, finalPosition, _animationCurve.Evaluate(lerpProgression));
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _backgroundStartPosition = finalPosition;
    }

    public void UpdateCamActionZoneUL(CameraControls _cam)
    {
        _cam.ChangeUpperLeftCamActionZone(new Vector2(GiveAdapativeBoundaryActionZoneX(), 0.0f)); 
    }

    public void UpdateCamActionZoneLR(CameraControls _cam)
    {
        _cam.ChangeLowerRightCamActionZone(new Vector2(GiveAdapativeBoundaryActionZoneX(), Screen.height));
    }

    float GiveAdapativeBoundaryActionZoneX()
    {
        return _boundaryActionZoneX * Screen.width / 1280.0f;
    }
}
