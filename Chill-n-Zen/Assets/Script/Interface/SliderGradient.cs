using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SliderGradient : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] Image _sliderImg;
    [SerializeField] Gradient _sliderGradient;

    private void Awake()
    {
        _slider.interactable = false;
    }

    public void UpdateSlider()
    {
        _sliderImg.color = _sliderGradient.Evaluate(_slider.normalizedValue);
    }
}
