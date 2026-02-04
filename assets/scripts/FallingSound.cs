using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSound : MonoBehaviour
{
    public AudioClip soundEffect;
    public float volume = 1.0f;
    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        //aggiungi componente audioSource al gameObject
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.spatialBlend = 1.0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if(soundEffect!=null)
        if (_audioSource && soundEffect)
        {
            if (collision.gameObject.CompareTag("Pavimento"))
            {
                _audioSource.PlayOneShot(soundEffect,volume);
            }
        }
    }
}
