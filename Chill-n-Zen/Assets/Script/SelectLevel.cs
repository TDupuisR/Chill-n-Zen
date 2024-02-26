using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{
    [SerializeField] private int _numberLevel;

    private void Start()
    {
        SceneManager.LoadScene(_numberLevel);
    }
}
