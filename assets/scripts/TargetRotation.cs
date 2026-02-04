using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRotation : MonoBehaviour
{
    public float rotationSpeed = 30.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rotationStep = rotationSpeed * Time.deltaTime;
        Quaternion localRotation = Quaternion.Euler(0f, rotationStep, 0f);
        transform.rotation = transform.rotation * localRotation;
    }
}
