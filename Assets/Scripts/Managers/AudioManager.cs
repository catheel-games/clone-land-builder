using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [Serializable]
    public struct Sound
    {
        public string name;
        public AudioClip clip;
        public Vector2 pitch;
    }

    [Serializable]
    public struct Music
    {
        public string name;
        public AudioClip clip;
    }

    [Serializable]
    public struct SoundGroup
    {
        public string name;
        public Sound[] sounds;
    }

    [Serializable]
    public struct MusicGroup
    {
        public string name;
        public Music[] musics;
    }

    [SerializeField] private AudioSource audioSourcePrefab;
    [SerializeField] private float musicFadeDuration = 2f;

    [Header("Sounds")]
    [SerializeField] private AudioMixerGroup soundMixerGroup;
    [SerializeField] private SoundGroup[] soundGroups;

    [Header("Music")]
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private MusicGroup[] musicGroups;


    private Dictionary<string, Dictionary<string, Sound>> soundRegistry = new Dictionary<string, Dictionary<string, Sound>>();
    private Dictionary<string, MusicGroup> musicRegistry = new Dictionary<string, MusicGroup>();

    private Dictionary<string, AudioSource> musicAudioSources = new Dictionary<string, AudioSource>();

    private AudioSource currentMusicSource;
    private Sequence currentMusicSequence;

    protected override void Awake()
    {
        base.Awake();

        foreach (SoundGroup group in soundGroups)
        {
            soundRegistry[group.name] = new Dictionary<string, Sound>();

            foreach (Sound sound in group.sounds)
            {
                soundRegistry[group.name][sound.name] = sound;
            }
        }

        foreach (MusicGroup group in musicGroups)
        {
            musicRegistry[group.name] = group;

            musicAudioSources[group.name] = Instantiate(audioSourcePrefab, Vector3.zero, Quaternion.identity, transform);
            musicAudioSources[group.name].gameObject.name = "Music Source";
            musicAudioSources[group.name].outputAudioMixerGroup = musicMixerGroup;
        }
    }

    public AudioSource PlaySound(string groupName, string soundName)
    {
        if (soundRegistry.TryGetValue(groupName, out var group))
        {
            if (group.TryGetValue(soundName, out var sound))
            {
                AudioSource source = Instantiate(audioSourcePrefab, Vector3.zero, Quaternion.identity, transform);

                source.gameObject.name = "Sound Source";

                source.clip = sound.clip;
                source.outputAudioMixerGroup = soundMixerGroup;
                source.pitch = UnityEngine.Random.Range(sound.pitch.x, sound.pitch.y);

                source.Play();
                Destroy(source.gameObject, (sound.clip.length / source.pitch) + 1f);

                return source;
            }
            else
            {
                Debug.LogWarning($"Sound '{soundName}' not found in group '{groupName}'.");
            }
        }
        else
        {
            Debug.LogWarning($"Sound group '{groupName}' not found.");
        }
        
        return null;
    }

    public AudioSource PlayMusicGroup(string groupName)
    {
        if (musicRegistry.TryGetValue(groupName, out var group))
        {
            AudioSource musicAudioSource = musicAudioSources[groupName];

            if (currentMusicSequence != null)
            {
                currentMusicSequence.Kill();
            }

            currentMusicSequence = DOTween.Sequence();

            foreach (Music music in group.musics)
            {
                currentMusicSequence.AppendCallback(() =>
                {
                    musicAudioSource.clip = music.clip;
                    musicAudioSource.Play();
                });

                currentMusicSequence.AppendInterval(music.clip.length);
            }

            currentMusicSequence.SetLoops(-1, LoopType.Restart);

            if (currentMusicSource != null)
            {
                Sequence sequence = DOTween.Sequence();

                sequence.Append(currentMusicSource.DOFade(0f, musicFadeDuration));
                sequence.Join(musicAudioSource.DOFade(1f, musicFadeDuration));
                sequence.OnComplete(() => {
                    currentMusicSource.Stop();
                    currentMusicSource = musicAudioSource;
                });
            } else
            {
                musicAudioSource.volume = 1f;
                currentMusicSource = musicAudioSource;
            }

            return musicAudioSource;
        }
        else
        {
            Debug.LogWarning($"Music group '{groupName}' not found.");
        }

        return null;
    }
}