using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SYSTEM : MonoBehaviour
{
    //VARIABILI PORTA
    /*[SerializeField] private Button3D _openDoorButton; //launcher
    [SerializeField] private Door _door; //launcherGO : effetto sul listener DOOR
    public Transform PlayerCapsule;*/
    //AZIONI PORTA ...

    //VARIABILI BOOK
    [SerializeField] private Book _countLauncherBook; //launcher
    private int counter = 0;
    //AZIONI BOOK 
    public Action After3Books;

    //VARIABILI NPC
    //AZIONI NPC

    void Start()
    {
    }

    private void IncrementCounter(int BookNumber)
    {
        if(counter == 3)
        {
            //AZIONE PER NPC 
            if(After3Books != null)
            {
                After3Books(); //Azione per NPC invocata
            }
            counter = 0;
            Debug.LogError("Hai preso più di 3 libri");
            return;
        }
        counter = counter + BookNumber;
    }
}