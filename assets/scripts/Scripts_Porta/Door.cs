using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Door : MonoBehaviour
{
    public Action DoorOpened;
    public Action DoorClosed;
    public Action<bool> DoorRotating;

    [SerializeField] private GameObject _doorHindge;
    [SerializeField] private float _openingTime = 1f;
    [SerializeField] private float _closingTime = 0.5f; //tempi durata animazione, non dopo quanto si chiude

    [SerializeField] private AudioClip soundOpenDoor;
    [SerializeField] private AudioClip soundCloseDoor;
    [SerializeField] private float volume = 1.0f;
    private AudioSource _audioSource;
    
    private bool _isRotating = false;
    private bool _isOpen = false;
    private Quaternion _originalRotation;

    private void Start()
    {
        _originalRotation = _doorHindge.transform.localRotation; //salva la posizione inziale originale porta
        //aggiungi componente audioSource al gameObject
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.spatialBlend = 1.0f;
    }

    public void OpenDoor(float rotation)
    {
        if (_isRotating || _isOpen)
            return;
        //applica rotazione (passata dopo pressione tasto 90°) attorno a asse y
        Quaternion targetRotation = Quaternion.Euler(_originalRotation.x, rotation, _originalRotation.z);
        //inizio animazione di durata openingtime totale
        StartCoroutine(AnimateDoor(targetRotation, _openingTime));
        _audioSource.PlayOneShot(soundOpenDoor,volume);
    }

    public void CloseDoor()
    {
        if (_isRotating || !_isOpen)
            return;

        StartCoroutine(AnimateDoor(_originalRotation, _closingTime));
        _audioSource.PlayOneShot(soundCloseDoor,volume);
        //AZIONE PER FSM
        if (DoorClosed != null) //controllo listeners iscritti: PORTA
            DoorClosed();
    }
    //OVERLOAD CLOSE DOOR : chiusura manuale porta
    public void CloseDoor(float rotation)
    {
        if (_isRotating || !_isOpen)
            return;

        StartCoroutine(AnimateDoor(_originalRotation, _closingTime));
    }

    private IEnumerator AnimateDoor(Quaternion targetRotation, float animationTime)
    {
        _isRotating = true;

        if (DoorRotating != null)
            DoorRotating.Invoke(!_isOpen);

        float animationTimer = 0;
        Quaternion startRotation = _doorHindge.transform.localRotation;
         
        while (animationTimer < animationTime)
        {
            animationTimer += Time.deltaTime;
            _doorHindge.transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, animationTimer / animationTime);
            yield return null;
        }

        _isRotating = false;
        _isOpen = !_isOpen;

        if (_isOpen && DoorOpened != null)
            DoorOpened.Invoke();
        if (!_isOpen && DoorClosed != null)
            DoorClosed.Invoke();
    }

    public void OpenAndClose(float rotation)
    {
        OpenDoor(rotation);
        //CloseDoor(-rotation); //per fare chiusura con interazione : da aggiungere un nuovo Button3D per maniglia interna
        //Invoke("CloseDoor", _openingTime + 3f); //porta si chiude dopo 2 secondi
        //SCELTA :CHIUSURA TRAMITE TRIGGER (vedi script)
    }



}
