using GameManagerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedBackSignManager : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private float _scaleSpeed;
    [SerializeField] private float _minimumSize = 8f;
    [SerializeField] private float _magnitudeSinus = 0.1f;
    private Vector3 _initialSize ;
    private float _scaleFactor;

    private void Start()
    {
        _initialSize = _rectTransform.localScale;
    }

    private void FixedUpdate()
    {
        if(GameManager.budgetManager.IsOutOfBudget() == true)
        {
            _scaleFactor = _minimumSize + Mathf.Sin(Time.time * _scaleSpeed);
            _scaleFactor = _scaleFactor * _magnitudeSinus;
            _rectTransform.localScale = new Vector3(_scaleFactor, _scaleFactor, 1.0f);
        }
        _rectTransform.localScale = _initialSize;
        
    }
}
