using UnityEngine;
using System;

public class Book : MonoBehaviour
{
    private bool isGrabbed = false;
    public Action<Book, bool> OnBookGrabbed;
    public string title;
    public NpcBehaviour npc;

    private void Start()
    {
        
    }

    public void prendiLibro()
    {
        if (isGrabbed)
            return;
        //AZIONE : quando libro viene preso lancia un azione un +1 che il SYSTEM aggiungerà al contatore
        if (!GameManager.Instance.IsNpcTalking())
        {
            if (OnBookGrabbed != null) //controllo listeners iscritti: PORTA
                OnBookGrabbed(this, isGrabbed); //gli passiamo l'istanza del libro che è stato preso
            isGrabbed = true;
        }
    }

    public String getTitle()
    {
        return title;
    }
 
}
