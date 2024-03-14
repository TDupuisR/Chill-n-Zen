using GameManagerSpace;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void GotoSelectionLevels()
    {
        GameManager.Instance.ChangeScene(1);
    }

    public void GotoOptions()
    {
        throw new System.NotImplementedException();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
