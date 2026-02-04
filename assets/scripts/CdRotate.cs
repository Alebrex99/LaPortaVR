using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CdRotate : MonoBehaviour
{
    [SerializeField] private float time = 2f;
    [SerializeField] private float rotationSpeed = 60f;
    
    private bool _rotationEnabled = false;

    private void Update()
    {
        if (_rotationEnabled)
        {
            float rotationStep = rotationSpeed * Time.deltaTime;
            Quaternion localRotation = Quaternion.Euler(0f, 0f, rotationStep);
            gameObject.transform.rotation *= localRotation;
        }
    }
    public void WaitRotate() => Invoke("EnableRotation", time);
    public void EnableRotation() => _rotationEnabled = true;
    public void DisableRotation() => _rotationEnabled = false;
}
