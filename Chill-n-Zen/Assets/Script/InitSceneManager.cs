using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitSceneManager : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene(1);
    }
}
