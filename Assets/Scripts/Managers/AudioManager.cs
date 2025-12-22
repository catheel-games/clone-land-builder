using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Serializable]
    private struct AudioClipData
    {
        public string name;
        public AudioClip clip;
        public float pitch;
    }

    [Serializable]
    private struct AudioGroup
    {
        public string name;
        public AudioClipData[] clips;
    }

    [SerializeField]
    private AudioGroup[] audioGroups;
    private Dictionary<string, Dictionary<string, AudioClipData>> audioGroupRegistry;

    protected override void Awake()
    {
        base.Awake();

        audioGroupRegistry = new Dictionary<string, Dictionary<string, AudioClipData>>();

        foreach (var audioGroup in audioGroups)
        {
            Dictionary<string, AudioClipData> clipRegistry = new Dictionary<string, AudioClipData>();
        
            foreach (var clipData in audioGroup.clips)
            {
                clipRegistry[clipData.name] = clipData;
            }
        
            audioGroupRegistry[audioGroup.name] = clipRegistry;
        }
    }

    public void Play(string groupName, string clipName)
    {
        if (audioGroupRegistry.TryGetValue(groupName, out Dictionary<string, AudioClipData> clipRegistry))
        {
            if (clipRegistry.TryGetValue(clipName, out AudioClipData clipData))
            {
                AudioSource.PlayClipAtPoint(clipData.clip, Vector3.zero, 1.0f);
            }
        }
    }
}