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
        public AudioMixerSnapshot snapshot;
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

    private MusicGroup currentMusicGroup;
    private AudioSource currentMusicSource;
    private int currentMusicIndex;

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

    public void PlayMusicGroup(string musicQuery)
    {
        if (musicRegistry.TryGetValue(musicQuery, out var group))
        {
            currentMusicGroup = group;
            currentMusicIndex = 0;
            PlayMusic();
        }
        else
        {
            Debug.LogWarning($"Music query '{musicQuery}' not found.");
        }
    }

    private void PlayMusic()
    {
        Music music = currentMusicGroup.musics[currentMusicIndex];

        AudioSource newMusicSource = Instantiate(audioSourcePrefab, Vector3.zero, Quaternion.identity, transform);

        newMusicSource.gameObject.name = "Music Source";

        newMusicSource.clip = music.clip;
        newMusicSource.outputAudioMixerGroup = musicMixerGroup;

        newMusicSource.Play();

        if (currentMusicSource != null)
        {
            Sequence sequence = DOTween.Sequence();

            newMusicSource.volume = 0f;

            sequence.Append(currentMusicSource.DOFade(0f, musicFadeDuration));
            sequence.Join(newMusicSource.DOFade(1f, musicFadeDuration));
            sequence.OnComplete(() =>
                {
                    if (currentMusicSource != null) 
                    {
                        if (currentMusicSource.gameObject != null)
                        {
                            Destroy(currentMusicSource.gameObject);
                        }
                    }

                    currentMusicSource = newMusicSource;
                }
            );
        } 
        else
        {
            newMusicSource.volume = 1f;
            currentMusicSource = newMusicSource;
        }

        currentMusicIndex = (currentMusicIndex + 1) % currentMusicGroup.musics.Length;
        
    }
}