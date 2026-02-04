using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Mouth : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    public void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayAudioMouth()
    {
        _audioSource.PlayOneShot(_audioClip);
    }
}
