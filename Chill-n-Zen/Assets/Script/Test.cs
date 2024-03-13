using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
public class Test : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    public void Start()
    {
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            text.text = "SuccessConnexion";
            Social.ReportProgress("CgkI5ZWvkocPEAIQBg", 100.0f, (bool success) => { });
        }
        else
        {
            text.text = "FailConnexion";
            PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
        }
    }
}
