using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.XR.Interaction.Toolkit;

public class NpcBehaviour : MonoBehaviour
{

    private Animator _animator;
    private AudioSource _audioSource;
    private PlayQuickSound _voice;
    [SerializeField] private AudioClip[] clips;
    public bool _isTalkingNpc = false;
    public float lastDelay = 0;

    // Start is called before the first frame update
    void Start()
    {
        _voice = GetComponent<PlayQuickSound>();
        _audioSource = GetComponentInChildren<AudioSource>(); //audioSOurce.clip, prendi clip
        _animator = GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogWarning($"There is no Animator attached to GameObject named: {gameObject.name}");
        }
    }

    // Update is called once per frame
    void Update()
    {   
        if (_animator != null)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                // beckoning animation (fa cenno di entrare)
                _animator.SetTrigger("BeckoningMaskTrigger");
            }
        
            if (Input.GetKeyDown(KeyCode.T))
            {
                // talking animation
                _animator.SetTrigger("TalkingMaskTrigger");
            }
        
            if (Input.GetKeyDown(KeyCode.P))
            {
                // pointing animation
                _animator.SetTrigger("PointingMaskTrigger");
            }
            
            if (Input.GetKeyDown(KeyCode.I))
            {
                // beckoning animation
                _animator.SetTrigger("IdleTrigger");
            }
            
        }
        
    }

    //FUNZIONE NELLO STATO MUSIC STATE
    public void StartWelcomeVoice()
    {
        _audioSource.clip = clips[0];
        _audioSource.PlayOneShot(clips[0]);
        _isTalkingNpc = true; //inizio a parlare
        Debug.Log($"ho iniziaro a parlare{_isTalkingNpc} WELCOME");
        _animator.SetTrigger("BeckoningMaskTrigger");
        Invoke("SetIsTalkingNpc", _audioSource.clip.length); //smetto di parlare quando clip finisce
        
    }

    //FUNZIONI NELLO STATO SEARCH BOOK STATE
    public void StartSearchVoice(float voiceDelay) => Invoke("StartVoice", voiceDelay);
    public void StartVoice()
    {
        _audioSource.clip = clips[1];
        _audioSource.PlayOneShot(clips[1]);
        _isTalkingNpc = true;
        Debug.Log($"ho iniziaro a parlare{_isTalkingNpc}: SEARCH BOOK");
        Invoke("SetIsTalkingNpc", _audioSource.clip.length); //smetto di parlare quando clip finisce
    }
    public void StartAudioLeary()
    {
        _audioSource.clip = clips[2];
        _audioSource.PlayOneShot(clips[2]);
        _isTalkingNpc = true;
        Debug.Log($"ho iniziaro a parlare{_isTalkingNpc}: BOOK - LEARY");
        Invoke("SetIsTalkingNpc", _audioSource.clip.length); //smetto di parlare quando clip finisce

    }
    public void StartAudioHuxley()
    {
        _audioSource.clip = clips[3];
        _audioSource.PlayOneShot(clips[3]);
        _isTalkingNpc = true;
        Debug.Log($"ho iniziaro a parlare{_isTalkingNpc}: BOOK - HUXLEY");
        Invoke("SetIsTalkingNpc", _audioSource.clip.length); //smetto di parlare quando clip finisce
    }
    public void StartAudioDevereux()
    {
        _audioSource.clip = clips[4];
        _audioSource.PlayOneShot(clips[4]);
        _isTalkingNpc = true;
        Debug.Log($"ho iniziaro a parlare{_isTalkingNpc}: BOOK - DEVEREUX");
        Invoke("SetIsTalkingNpc", _audioSource.clip.length); //smetto di parlare quando clip finisce
    }

    //FUNZIONI MUSHROOM STATE
    public void StartAudioMushroomsAfterTime()
    {
        lastDelay = _audioSource.clip.length+4;
        Invoke("StartAudioMushrooms", lastDelay);
    }
    public void StartAudioMushrooms()//dopo l'ultimo libro npc dice di andare a prendere i funghi
    {
        _audioSource.clip = clips[5];
        _audioSource.PlayOneShot(clips[5]);
        _isTalkingNpc = true;
        Debug.Log($"ho iniziaro a parlare{_isTalkingNpc}: MUSHROOM");
        Invoke("SetIsTalkingNpc", _audioSource.clip.length); //smetto di parlare quando clip finisce
    }
    
    public void StartAudioEffects(float voiceDelay) => Invoke("StartEffects", voiceDelay); //
    public void StartEffects()
    {
        _audioSource.clip = clips[6];
        _audioSource.PlayOneShot(clips[6]);
        _isTalkingNpc = true;
        Debug.Log($"ho iniziaro a parlare{_isTalkingNpc}: EFFECTS");
        Invoke("SetIsTalkingNpc", _audioSource.clip.length); //smetto di parlare quando clip finisce
    }

    //FUNZIONI EFFECTS STATE
    public void FinalSpeech()
    {
        _audioSource.clip = clips[7];
        _audioSource.PlayOneShot(clips[7]);
        _isTalkingNpc = true;
        Debug.Log($"ho iniziaro a parlare{_isTalkingNpc}: FINAL SPEECH");
        Invoke("SetIsTalkingNpc", _audioSource.clip.length); //smetto di parlare quando clip finisce
    }


    //NON TOCCARE : ANIMAZIONI
    public void TalkingToPointing(float pointingDelay) //float pointingDelay
    {
        


        if (_isTalkingNpc)
        {
            _animator.ResetTrigger("IdleTrigger");
            _animator.SetTrigger("TalkingPointingTrigger");
            _animator.SetFloat("PointingDelay", pointingDelay-lastDelay); //talking --> pointing: guarda Audio 
           
        }
        else
        {
            _animator.ResetTrigger("TalkingMaskTrigger");
            IdleAnimation();
        }
        /*if (!_audioSource.isPlaying)
        {
            _animator.SetTrigger("IdleTrigger");
            Debug.Log("TalkingToIdle --> audio source NOT ISPLAYING");
        }
        else
        {
            _animator.SetTrigger("TalkingPointingTrigger");
            _animator.SetFloat("PointingDelay",pointingDelay); //talking --> pointing: guarda Audio 
            Debug.Log("TalkingToIdle --> audio source ISPLAING");
        }*/
    }
    public void TalkingToIdle()
    {
        if (_isTalkingNpc)
        {
            _animator.ResetTrigger("IdleTrigger");
            _animator.SetTrigger("TalkingMaskTrigger");
        }
        else
        {
            _animator.ResetTrigger("TalkingMaskTrigger");
            IdleAnimation();
        }
           
        //Invoke("IdleAnimation", _audioSource.clip.length);
        /*if (!_audioSource.isPlaying)
        {
            _animator.SetTrigger("IdleTrigger");
            Debug.Log("TalkingToIdle --> audio source NOT ISPLAYING");   
        }
        else
        {
            _animator.SetTrigger("TalkingMaskTrigger");

            Debug.Log("TalkingToIdle --> audio source ISPLAING");
        }*/
    }
    public void IdleAnimation()
    {
        _animator.SetTrigger("IdleTrigger");
    }
    public void PointingAnimation()
    {
        _animator.SetTrigger("PointingTrigger");
    }
    public void SetIsTalkingNpc()
    {
        _isTalkingNpc = false;
        Debug.Log($"ho smesso di parlare{_isTalkingNpc}");
    }
    public AudioSource getAudioSource()
    {
        return _audioSource;
    }
}
