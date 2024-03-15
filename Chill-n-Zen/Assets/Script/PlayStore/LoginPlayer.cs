using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SceneManagement;

public class LoginPlayer : MonoBehaviour
{
    public void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status != SignInStatus.Success)
        {
            PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
        }
        PlayGamesPlatform.Instance.UnlockAchievement("CgkI5ZWvkocPEAIQBg");
        SceneManager.LoadScene(1);
    }
}
