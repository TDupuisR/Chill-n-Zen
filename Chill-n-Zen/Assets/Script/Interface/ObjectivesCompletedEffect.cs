using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesCompletedEffect : MonoBehaviour
{
    [SerializeField] Image _img;
    [SerializeField] TMP_Text _text;
    [SerializeField] WindowScroll _window;
    [Space(5)]
    [SerializeField] float _animationDuration;

    public Sprite ImgToImplement {  get; set; }
    public TMP_Text TextToImplement {  get; set; }

    public void ObjectiveCompletedEffect()
    {
        _img.sprite = ImgToImplement;
        _text = TextToImplement;

        StartCoroutine(CompletedEffectAniation());
    }

    IEnumerator CompletedEffectAniation()
    {
        _window.StartScroll();
        yield return new WaitForSeconds(_animationDuration / 2.0f);
        _window.StartScroll();
        yield return new WaitForSeconds(_animationDuration / 2.0f);
        Destroy(gameObject);
    }
}
