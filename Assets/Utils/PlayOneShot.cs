using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayOneShot : MonoBehaviour
{
    [Serializable]
    public struct SoundEntry
    {
        [SerializeField] private string _name;
        [FMODUnity.EventRef] [SerializeField] private string _fileLocation;

        public string Name => _name;
        public string FileLocation => _fileLocation;
    }

    public List<SoundEntry> soundEntries = new List<SoundEntry>();
    private Dictionary<string, string> _dictionaryOfSounds = new Dictionary<string, string>();

    private void Awake()
    {
        // Convert List to Dictionary
        _dictionaryOfSounds = soundEntries.Distinct().ToDictionary(entry => entry.Name, entry => entry.FileLocation);
    }

    public void PlaySound(int soundIndex)
    {
        if (soundIndex >= _dictionaryOfSounds.Count)
        {
            Debug.LogError("soundIndex was out of range!");
            return;
        }
        FMODUnity.RuntimeManager.PlayOneShot(_dictionaryOfSounds.ElementAt(soundIndex).Value);
    }

    public void PlaySound(string soundName)
    {
        if (!_dictionaryOfSounds.TryGetValue(soundName, out string value))
        {
            Debug.LogError(soundName + " does not exist!");
            return;
        }
        FMODUnity.RuntimeManager.PlayOneShot(value);
    }
}