using GameManagerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{
    [SerializeField] private int _numberLevel;

    public void ChooseLevel()
    {
        GameManager.Instance.ChangeScene(_numberLevel + 3);
    }
}
