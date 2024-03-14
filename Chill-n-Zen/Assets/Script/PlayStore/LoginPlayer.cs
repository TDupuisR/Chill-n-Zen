using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
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
        Social.ReportProgress("CgkI5ZWvkocPEAIQBg", 100.0f, (bool success) => { });
        SceneManager.LoadScene(1);
    }
}
