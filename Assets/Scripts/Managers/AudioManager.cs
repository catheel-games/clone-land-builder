using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{

    [Header ("Mixer")]
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private float offSoundDb;
    [SerializeField] private float onSoundDb;

    [Header ("Sources - Clips")]
    public List<AudioSource> _audioSources;

    [SerializeField] private List<SoundList> _clips = new List<SoundList>(6);

    public void Play(string clipName, int AudioSource, int listID,
                    bool isPitched = false, float minPitch = 1, float maxPitch = 1, bool loop = false)
    {

        AudioSource source = _audioSources[AudioSource];
        AudioClip clip = _clips[listID].Sounds.Find(e => e.Name == clipName).Clip;

        source.pitch = isPitched ? UnityEngine.Random.Range(minPitch, maxPitch) : 1;

        source.clip = clip;
        source.loop = loop;
        source.Play();

    }

    public void PlayOtherSource(string clipName, AudioSource sourceName, int listID,
                                bool isPitched = false, float minPitch =1, float maxPitch =1, bool loop = false)
    {
        AudioClip clip = _clips[listID].Sounds.Find(e => e.Name == clipName).Clip;

        sourceName.pitch = isPitched ? UnityEngine.Random.Range(minPitch, maxPitch) : 1;

        sourceName.clip = clip;
        sourceName.loop = loop;
        sourceName.Play();

    }


    public void ChangeMixerGroupVolume(string mixerGroupName, bool IsOn = true)
    {
        float volume = IsOn ? onSoundDb : offSoundDb;

        _mixer.SetFloat(mixerGroupName, volume);
    }


    [Serializable]
    private struct Sound
    {
        public string Name;
        public AudioClip Clip;
    }

    [Serializable]
    private struct SoundList
    {
        public string Name;
        public List <Sound> Sounds;
    }

    [Serializable]
    private struct AudioOut
    {
        public string Name;
        public AudioSource AudioSource;
    }
}