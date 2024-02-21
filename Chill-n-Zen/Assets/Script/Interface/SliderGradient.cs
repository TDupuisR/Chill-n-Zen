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

    public float progression
    {
        get => progression;
        set
        {
            UpdateSlider(value);
        }
    }


    void UpdateSlider(float value)
    {
        _slider.value = value;
        _sliderImg.color = _sliderGradient.Evaluate(value);
    }
}
