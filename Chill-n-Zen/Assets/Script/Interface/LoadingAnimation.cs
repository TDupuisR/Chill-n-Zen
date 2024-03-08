using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingAnimation : MonoBehaviour
{
    [SerializeField] RectTransform _parent;
    [SerializeField] float _smoothSpeed = 1f;
    [SerializeField, Range(0, 100)] float _tolerance = 5f;

    [SerializeField] Image _img;
    [SerializeField] List<Sprite> _spriteList;
    [SerializeField] float _animationSpeed;
    int _currentFrame;
    float _currentTime;

    private bool _isActive = false;
    public bool AnimationFinished {  get; private set; }

    private void Start()
    {
        _isActive = true;

        StartCoroutine(TransitionLoading(0f, -2000f));
    }

    void FixedUpdate()
    {
        if (_isActive)
        {
            _currentTime += Time.deltaTime * _animationSpeed;
            if (_currentTime > 1.0f)
            {
                _currentTime = 0;

                _currentFrame++;
                if (_currentFrame >= _spriteList.Count)
                {
                    _currentFrame = 0;
                }
                _img.sprite = _spriteList[_currentFrame];
            }
        }
    }

    public IEnumerator TransitionLoading(float start, float target)
    {
        AnimationFinished = false;
        _isActive = true;

        _parent.anchoredPosition = new Vector2(start, _parent.anchoredPosition.y);

        while (Mathf.Abs(target -_parent.anchoredPosition.x) >= Mathf.Abs((target - start) * (_tolerance/100)))
        {
            _parent.anchoredPosition += new Vector2(((target - _parent.anchoredPosition.x) / _smoothSpeed) * Time.fixedDeltaTime, 0f);

            yield return new WaitForFixedUpdate();
        }
        Debug.Log("end transition");
        _parent.anchoredPosition = new Vector2(target, _parent.anchoredPosition.y);

        AnimationFinished = true;
    }
}
