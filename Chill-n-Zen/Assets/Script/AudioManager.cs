using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _soundSource;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private List<AudioClip> _listSounds;
    [Range(0f, 1f)]
    [SerializeField] private float _musicVolume;
    [Range(0f, 1f)]
    [SerializeField] private float _soundVolume;
    private Dictionary<string, AudioClip> _soundDictionary;

    void Start()
    {
        _soundDictionary = new Dictionary<string, AudioClip>();
        foreach (var sound in _listSounds)
        {
            _soundDictionary.Add(sound.name, sound);
        }
        _musicSource.volume = _musicVolume;
        _soundSource.volume = _soundVolume;
    }

    void ChangeVolume(float amount, AudioSource audioSource)
    {
        audioSource.volume = Mathf.Clamp01(amount);
    }


    void PlaySound(string soundName)
    {
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
}
