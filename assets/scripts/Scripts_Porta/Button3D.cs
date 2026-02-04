using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.XR.Interaction.Toolkit;

public class Button3D : MonoBehaviour
{
    /*LAUNCHER dell'azione: FPS manager prende questo oggetto puntato dal ray (Button) passando
     per Button3DInteractable, chiama Interact(FPS_manager) su di lui , dentro è chiamata Press() di classe
    Button: INIZIO AZIONE*/ 
    public Action OnButtonPressed;
    //[SerializeField] XRGrabInteractable grabInteractable;

    public Transform manigliaExt; 
    public Transform manigliaInt;
    public float localXFinalRotManiglia; //posizione finale maniglia

    public float pressDuration = 0.3f;
    public float releaseDuration = 0.1f;

    public Color unpressedColor;
    public Color pressedColor;

    private MeshRenderer _renderer;
    private bool isPressed = false;
    private float initialLocalXRot;

    /*private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnButtonPressed().invoke());
    }

    private void ciccio(SelectEnterEventArgs arg0)
    {
        throw new NotImplementedException();
    }*/

    void Start ()
    {
        initialLocalXRot = manigliaExt.localRotation.x;

        _renderer = manigliaExt.GetComponent<MeshRenderer>();
        if (_renderer != null)
            _renderer.material.color = unpressedColor;

    }

    public void Press()
    {
        if (isPressed)
            return;

        isPressed = true;
        if (_renderer != null)
            _renderer.material.color = pressedColor;

        /*finita animazione bottone -> invocata azione
         Listeners azione OnButtonPressed() -> DoorOpeningButton(apertura/chiusura porta)*/
        Sequence pressSequence = DOTween.Sequence();
        //ANIMAZIONE MANIGLIA 
        pressSequence.Append(manigliaExt.DOLocalRotate(new Vector3(localXFinalRotManiglia, 0,0), pressDuration).OnComplete(() =>
        {
            //When Button has reached the end of travel rise event
            if (OnButtonPressed != null) //controllo listeners iscritti: PORTA
                OnButtonPressed();
        }));
        pressSequence.Append(manigliaExt.DOLocalRotate(new Vector3(initialLocalXRot,0,0), releaseDuration));
        pressSequence.OnComplete(() =>
        {
            isPressed = false;
            if (_renderer != null)
                _renderer.material.color = unpressedColor;
        });

    }
}
