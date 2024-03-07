using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesCheckbox : MonoBehaviour
{
    [SerializeField] Image _img;
    [SerializeField] TMP_Text _text;

    public Image Img { get => _img; set => _img = value; }
    public TMP_Text Text { get => _text; set => _text = value; }

}
