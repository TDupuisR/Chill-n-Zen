using GameManagerSpace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void RestartLevel() => GameManager.Instance.ChangeScene(SceneManager.GetActiveScene().buildIndex);
}
