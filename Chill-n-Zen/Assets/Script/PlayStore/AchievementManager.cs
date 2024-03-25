using GameManagerSpace;
using GooglePlayGames;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    private bool _check = true;
    public bool connected = false;
    [SerializeField] Item _cookingPlate;


    private void OnEnable()
    {
        TileSystem.OnSceneChanged += CheckAchievementFloorIsLava;
        TileSystem.OnSceneChanged += CheckAchievementBotanist;
        GameManager.OnSceneLoad += CheckAchievementGoodStart;
        GameManager.OnSceneLoad += CheckAchievementInteriorDesigner;
        GameManager.OnSceneLoad += CheckAchievementGraduate;
    }

    private void OnDisable()
    {
        TileSystem.OnSceneChanged -= CheckAchievementFloorIsLava;
        TileSystem.OnSceneChanged -= CheckAchievementBotanist;
        GameManager.OnSceneLoad -= CheckAchievementGoodStart;
        GameManager.OnSceneLoad -= CheckAchievementInteriorDesigner;
        GameManager.OnSceneLoad -= CheckAchievementGraduate;
    }

    public void CheckAchievementFloorIsLava()
    {
        if(connected)
        {
            GMStatic.requestObj floor = new GMStatic.requestObj();
            floor.itemRequested = new List<Item>();
            floor.itemRequested.Add(_cookingPlate);
            floor.nbRequested = TileSystem.Instance.TilesList.Count - 1;
            if (GameManager.requestManager.CheckObjRequest(floor))
            {
                PlayGamesPlatform.Instance.UnlockAchievement("CgkI5ZWvkocPEAIQAQ");
            }
        }
    }
    public void CheckAchievementBotanist()
    {
        if(connected)
        {
            GMStatic.requestUsage plante = new GMStatic.requestUsage();
            plante.usageRequested = new List<GMStatic.tagUsage>();
            plante.usageRequested.Add(GMStatic.tagUsage.Plant);
            plante.nbRequested = 10;
            if (GameManager.requestManager.CheckTypeRequest(plante))
            {
                PlayGamesPlatform.Instance.UnlockAchievement("CgkI5ZWvkocPEAIQBA");
            }
        }
    }
    public void CheckAchievementGoodStart()
    {
        if (GameManager.saveData.LoadStar(1) != null && connected)
        {
            PlayGamesPlatform.Instance.UnlockAchievement("CgkI5ZWvkocPEAIQBQ");
        }
    }
    public void CheckAchievementInteriorDesigner()
    {
        if (connected)
        {
            _check = true;
            for (int i = 1; i <= 10; i++)
            {
                List<bool> currentStars = GameManager.saveData.LoadStar(i);
                if (currentStars != null)
                {
                    if (currentStars[2] == false)
                    {
                        _check = false;
                    }
                }
            }
            if (_check == true)
            {
                PlayGamesPlatform.Instance.UnlockAchievement("CgkI5ZWvkocPEAIQAg");
            }
        }       
    }
    public void CheckAchievementGraduate()
    {
        if (connected)
        {
            _check = true;
            for (int i = 1; i <= 10; i++)
            {
                if (GameManager.saveData.LoadStar(i) == null)
                {
                    _check = false;
                }
            }
            if (_check == true)
            {
                PlayGamesPlatform.Instance.UnlockAchievement("CgkI5ZWvkocPEAIQAw");
            }
        }
    }
}
