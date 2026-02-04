using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClockScript : MonoBehaviour
{
    private GameObject hoursHand;
    private GameObject minutesHand;
    private GameObject secondsHand;
    private DateTime begin;
    private const float HourRotation = 360.0f / 43200.0f;
    private const float MinuteRotation = 360.0f / 3600.0f;
    private const float SecondRotation = 360.0f / 60.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        //trova riferimenti lancette
        hoursHand = transform.Find("Clock_Analog_A_Hour").GameObject();
        minutesHand = transform.Find("Clock_Analog_A_Minute").GameObject();
        secondsHand = transform.Find("Clock_Analog_A_Second").GameObject();
        
        //imposta rotazione iniziale
        begin = DateTime.Now;
        Quaternion localRotation = Quaternion.Euler((float)(begin.Hour * 30 + begin.Minute * 0.5), 0f, 0f);
        hoursHand.transform.rotation = hoursHand.transform.rotation * localRotation;
        
        localRotation = Quaternion.Euler(begin.Minute * 6, 0f, 0f);
        minutesHand.transform.rotation = minutesHand.transform.rotation * localRotation;
        
        localRotation = Quaternion.Euler(begin.Second * 6, 0f, 0f);
        secondsHand.transform.rotation = secondsHand.transform.rotation * localRotation;
        
    }

    // Update is called once per frame
    void Update()
    {
        float rotationStep = HourRotation * Time.deltaTime;
        Quaternion localRotation = Quaternion.Euler(rotationStep, 0f, 0f);
        hoursHand.transform.rotation = hoursHand.transform.rotation * localRotation;

        rotationStep = MinuteRotation * Time.deltaTime;
        localRotation = Quaternion.Euler(rotationStep, 0f, 0f);
        minutesHand.transform.rotation = minutesHand.transform.rotation * localRotation;

        rotationStep = SecondRotation * Time.deltaTime;
        localRotation = Quaternion.Euler(rotationStep, 0f, 0f);
        secondsHand.transform.rotation = secondsHand.transform.rotation * localRotation;
    }
}
