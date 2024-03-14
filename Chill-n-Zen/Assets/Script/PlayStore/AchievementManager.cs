using GameManagerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    private bool _check = true;
    public void CheckAchievementFloorIsLava()
    {
        Social.ReportProgress("CgkI5ZWvkocPEAIQAQ", 100.0f, (bool success) => { });
    }
    public void CheckAchievementBotanist()
    {
        Social.ReportProgress("CgkI5ZWvkocPEAIQBA", 100.0f, (bool success) => { });
    }
    public void CheckAchievementGoodStart()
    {
        if (GameManager.saveData.LoadStar(1) != null)
        {
            Social.ReportProgress("CgkI5ZWvkocPEAIQBQ", 100.0f, (bool success) => { });
        }
    }
    public void CheckAchievementInteriorDesigner()
    {
        _check = true;
        for(int i = 1; i <= 10; i++)
        {
            if (GameManager.saveData.LoadStar(i)[2] == false)
            {
                _check = false;
            }
        }
        if(_check == true)
        {
            Social.ReportProgress("CgkI5ZWvkocPEAIQAg", 100.0f, (bool success) => { });
        }
        
    }
    public void CheckAchievementGraduate()
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
            Social.ReportProgress("CgkI5ZWvkocPEAIQAw", 100.0f, (bool success) => { });
        }
    }
}
