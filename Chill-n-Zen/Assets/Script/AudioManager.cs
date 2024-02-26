using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _soundSource;
    [SerializeField] private List<AudioClip> _listSounds;
    private Dictionary<string, AudioClip> _soundDictionary;

    void Start()
    {
        _soundDictionary = new Dictionary<string, AudioClip>();
        foreach (var sound in _listSounds)
        {
            _soundDictionary.Add(sound.name, sound);
        }
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
