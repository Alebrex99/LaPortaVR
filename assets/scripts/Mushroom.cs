using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.XR.Interaction.Toolkit;


public class Mushroom : MonoBehaviour
{
    
    public Action<Mushroom> OnMushroomEaten;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mouth"))
        {
            other.GetComponent<Mouth>().PlayAudioMouth();
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if(OnMushroomEaten!= null)
        {
            OnMushroomEaten(this);
        }
    }
}
