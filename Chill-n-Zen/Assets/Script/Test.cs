using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
public class Test : MonoBehaviour
{
    [SerializeField] private GameObject _connected;
    // Start is called before the first frame update
    void Start()
    {
        _connected.SetActive(false);
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);

    }


    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            _connected.SetActive(true);
            // Continue with Play Games Services
        }
        else
        {
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }
    }

}
