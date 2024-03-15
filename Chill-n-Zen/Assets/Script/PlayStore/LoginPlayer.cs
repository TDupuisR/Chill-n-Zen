using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SceneManagement;
using GameManagerSpace;

public class LoginPlayer : MonoBehaviour
{
    [SerializeField] AchievementManager achievementManager;
    public void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            PlayGamesPlatform.Instance.UnlockAchievement("CgkI5ZWvkocPEAIQBg");
        }
        else
        {
            achievementManager = GameManager.achievementManager.GetComponent<AchievementManager>();
            achievementManager.SetActive(false);
        }
        SceneManager.LoadScene(1);
    }
}
