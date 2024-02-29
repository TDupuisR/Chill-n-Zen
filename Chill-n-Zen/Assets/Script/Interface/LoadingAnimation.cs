using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingAnimation : MonoBehaviour
{
    [SerializeField] Image _img;
    [SerializeField] List<Sprite> _spriteList;
    [SerializeField] float _animationSpeed;
    int _currentFrame;
    float _currentTime;
    void Update()
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
