using GameManagerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void RestartLevel() => GameManager.Instance.ChangeScene(SceneManager.GetActiveScene().buildIndex);
}
