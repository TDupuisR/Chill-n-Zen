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

    public void ClearAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
