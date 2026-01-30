using UnityEngine;

/// <summary>
/// Play a simple sounds using Play one shot with volume, and pitch
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class PlayQuickSound : MonoBehaviour
{
    
    [Tooltip("The sound that is played")]
    public AudioClip sound = null;

    [Tooltip("The volume of the sound")]
    public float volume = 1.0f;

    [Tooltip("The range of pitch the sound is played at (-pitch, pitch)")]
    [Range(0, 1)] public float randomPitchVariance = 0.0f;
    
    [Tooltip("Loop button")]
    public bool isLooping = false;

    private AudioSource audioSource = null;

    private float defaultPitch = 1.0f;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
        audioSource.clip = sound;
        audioSource.volume = volume;
        audioSource.loop = isLooping;
    }

    public void PlayOneShot()
    {
        float randomVariance = Random.Range(-randomPitchVariance, randomPitchVariance);
        randomVariance += defaultPitch;

        audioSource.pitch = randomVariance;
        audioSource.PlayOneShot(sound, volume);
        audioSource.pitch = defaultPitch;
    }
    
    public void PlayAudio()
    {
        float randomVariance = Random.Range(-randomPitchVariance, randomPitchVariance);
        randomVariance += defaultPitch;
        
        //verificare se conviene metterlo sull'Awake
        //audioSource.clip = sound;
        //audioSource.volume = volume;
        //audioSource.loop = isLooping;

        audioSource.pitch = randomVariance;
        audioSource.Play();
        audioSource.pitch = defaultPitch;
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
    
    private void OnValidate()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }
}