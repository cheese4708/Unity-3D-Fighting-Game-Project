using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource MusicSource;   // For music/loops
    public AudioSource SFXSource;     // For one shot sounds

    void Awake()
    {

        // Singleton Setup
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip Clip, bool Loop)
    {
        MusicSource.clip = Clip;
        MusicSource.loop = Loop;
        MusicSource.Play();
    }

    public void PlaySFX(AudioClip Clip)
    {
        SFXSource.PlayOneShot(Clip);
    }

    public void StopMusic()
    {
        MusicSource.Stop();
    }
}