using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Disco : MonoBehaviour
{
    public string title;
    public AudioClip[] clips;
    private AudioSource[] _audioSources;
    public int numberOfClips = 3;
    //per FSM
    public Action CDStart;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfClips; i++)
        {
            gameObject.AddComponent<AudioSource>();
            _audioSources = GetComponents<AudioSource>();
            _audioSources[i].spatialBlend = 0.7f;
        }
    }
    //per audio 
    public void PlayMusicClip()
    {
        for (int i = 0; i < numberOfClips; i++)
        {
            if(i == 0)
                _audioSources[i].PlayOneShot(clips[i],GameManager.Instance.initMusicVolume);
            else
                _audioSources[i].PlayOneShot(clips[i],0.0f);
        }
    }

    public IEnumerator EaseVolume(float a, float b, float duration)
    {
        float timer = 0;
        while (timer <= duration)
        {
            // foreach (var source in _audioSources)
            //     source.volume = Mathf.Lerp(a, b, timer / duration);
            _audioSources[0].volume = Mathf.Lerp(a, b, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        // foreach (var source in _audioSources)
        //     source.volume = b;
        _audioSources[0].volume = b;
    }
    
    public void StartMusic()
    {
        if (CDStart != null)
        {
            CDStart();
        }
    }
}
