using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    //game
    int _maxLevel;

    //sound
    float _sfxVolume;
    float _musicVolume;

    //public accessors
    public int MaxLevel
    {
        get
        {
            if (PlayerPrefs.HasKey("maxLevel"))
            {
                _maxLevel = PlayerPrefs.GetInt("maxLevel");
            }
            else
            {
                _maxLevel = 0;
            }

            return _maxLevel;
        }
        set
        {
            if (_maxLevel < 0)
            {
                Debug.LogWarning("max level can't be negative");
                _maxLevel = 0;
                return;
            }

            _maxLevel = value;
            PlayerPrefs.SetInt("maxLevel", value);
            PlayerPrefs.Save();

        }
    }

    public float SfxVolume
    {
        get
        {
            if (PlayerPrefs.HasKey("sfxVolume"))
            {
                _sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
            }
            else
            {
                _sfxVolume = 1.0f;
            }

            return _sfxVolume;
        }
        set
        {
            if (_sfxVolume < 0)
            {
                Debug.LogWarning("sfx volume need to be btw 0 & 1");
                _sfxVolume = Mathf.Clamp01(_sfxVolume);
            }

            _sfxVolume = value;
            PlayerPrefs.SetFloat("sfxVolume", value);
            PlayerPrefs.Save();

        }
    }

    public float MusicVolume
    {
        get
        {
            if (PlayerPrefs.HasKey("musicVolume"))
            {
                _musicVolume = PlayerPrefs.GetFloat("musicVolume");
            }
            else
            {
                _musicVolume = 1.0f;
            }

            return _musicVolume;
        }
        set
        {
            if (_musicVolume < 0)
            {
                Debug.LogWarning("music volume need to be btw 0 & 1");
                _musicVolume = Mathf.Clamp01(_musicVolume);
            }

            _musicVolume = value;
            PlayerPrefs.SetFloat("musicVolume", value);
            PlayerPrefs.Save();

        }
    }

    public void SetStar(int level, bool firstStar, bool secondStar, bool scoreStar)
    {
        string saveString = "";
        saveString += firstStar ? "1" : "0";
        saveString += secondStar ? "1" : "0";
        saveString += scoreStar ? "1" : "0";

        if(saveString.Length != 3)
            Debug.LogError("SaveString incorrect format : " + saveString);

        PlayerPrefs.SetString("star" + level.ToString(), saveString);
        PlayerPrefs.Save();
    }

    public List<bool> LoadStar(int level)
    {
        if (!PlayerPrefs.HasKey("star" + level.ToString()))
        {
            Debug.LogWarning("Doesn't have key \" star" + level.ToString() + "\"");
            return null;
        }

        List<bool> starList = new List<bool>() {false , false , false};
        string starSave = PlayerPrefs.GetString("star" + level.ToString());
        print(starList.Count);
        for (int i = 0; i < 3; i++)
        {
            starList[i] = starSave[i] == 49;
        }

        return starList;
    }

    public void ClearAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
