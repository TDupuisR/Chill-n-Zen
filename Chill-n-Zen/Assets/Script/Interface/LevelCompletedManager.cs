using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletedManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] List<Image> _starImg;
    [SerializeField] TMP_Text _finalScoreText;
    [Space(3)]
    [SerializeField] WindowScroll _windowScroll;
    [SerializeField] List<GameObject> _objectsToDisable;

    public void Awake()
    {
        foreach(GameObject obj in _objectsToDisable)
        {
            obj.SetActive(false);
        }

        _windowScroll.StartScroll();
    }
}
