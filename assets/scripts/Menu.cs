using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
        _audioSource.clip = _audioClip;
        _audioSource.loop = true;
        _audioSource.Play();
    }
}
