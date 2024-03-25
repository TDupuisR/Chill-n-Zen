using GameManagerSpace;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _soundSource;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private List<AudioClip> _listSounds;

    private Dictionary<string, AudioClip> _soundDictionary;
    private bool _isSet;

    void Start()
    {
        _soundDictionary = new Dictionary<string, AudioClip>();
        foreach (var sound in _listSounds)
        {
            _soundDictionary.Add(sound.name, sound);
        }
        _musicSource.volume = GameManager.saveData.MusicVolume;
        _soundSource.volume = GameManager.saveData.SfxVolume;
        _isSet = true;
    }

    void ChangeVolume(AudioSource audioSource)
    {
        audioSource.volume = GameManager.saveData.SfxVolume;
    }


    public void PlaySound(string soundName)
    {
        if (!_isSet)
            return;

        AudioClip clip;
        if (_soundDictionary.TryGetValue(soundName, out clip))
        {
            _soundSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("Sound not found: " + soundName);
        }
    }

    public void PlayMusic(string musicName)
    {
        if (!_isSet)
            return;

        AudioClip clip;
        if (_soundDictionary.TryGetValue(musicName, out clip))
        {
            _musicSource.clip = clip;
            _musicSource.Play();
        }
        else
        {
            Debug.LogError("Sound not found: " + musicName);
        }
    }
}
