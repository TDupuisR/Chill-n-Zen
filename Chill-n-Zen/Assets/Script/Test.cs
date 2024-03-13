using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
public class Test : MonoBehaviour
{
    [SerializeField] private GameObject _connected;
    // Start is called before the first frame update

    void Awake()
    {
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        _connected.SetActive(false);
    }
 
    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Social.ReportProgress("CgkI5ZWvkocPEAIQBg", 100.0f, (bool success) => { });
        }
    }
}
