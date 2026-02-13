using UnityEngine;


public class MusicStarter : MonoBehaviour
{
    [SerializeField] private MusicType musicType;

    public enum MusicType
    {
        Menu,
        Level
    }

    private void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (musicType == MusicType.Menu)
        {
            AudioManager.Instance.PlayMusicGroup("Menu");
        }

        else if (musicType == MusicType.Level)
        {
            AudioManager.Instance.PlayMusicGroup("Level");
        }
    }
}
