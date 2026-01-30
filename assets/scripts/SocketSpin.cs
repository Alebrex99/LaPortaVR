using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketSpin : MonoBehaviour
{
    XRSocketInteractor socketInteractor;
    public Disco discoScript;

    private void Awake()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
    }

    private void OnEnable()
    {
        socketInteractor.selectEntered.AddListener(OnSelectEntered);
    }

    private void OnDisable()
    {
        socketInteractor.selectEntered.RemoveListener(OnSelectEntered);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        discoScript = args.interactableObject.transform.gameObject.GetComponent<Disco>();
        
        discoScript.GetComponentInChildren<CdRotate>().WaitRotate();
        discoScript.StartMusic();
        discoScript.GetComponent<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask("DiscPlaying");
        discoScript.PlayMusicClip();
    }
}