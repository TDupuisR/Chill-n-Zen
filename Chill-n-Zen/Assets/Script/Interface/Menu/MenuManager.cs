using GameManagerSpace;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject _menuObject;
    [SerializeField] GameObject _creditsObject;

    private void Start()
    {
        GameManager.audioManager.PlayMusic("Menu");
    }

    public void GotoSelectionLevels()
    {
        GameManager.Instance.ChangeScene(2);
        GameManager.audioManager.PlaySound("Bouton entrer");
    }

    public void GotoCredits()
    {
        _menuObject.SetActive(false);
        _creditsObject.SetActive(true);
    }

    public void GotoMenu()
    {
        _menuObject.SetActive(true);
        _creditsObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
