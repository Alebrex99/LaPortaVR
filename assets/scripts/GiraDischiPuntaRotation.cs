using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GiraDischiPuntaRotation : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private float duration;
    [SerializeField] private float angle = 40f;

    public void StartPlaying() => Invoke("Rotate", time);
    public void Rotate()
    {
        gameObject.transform.DORotate(new Vector3(0, 0, -angle), duration, RotateMode.LocalAxisAdd);
    }
    public void StopPlaying() => Invoke("RotateOpposite", time);
    public void RotateOpposite()
    {
        gameObject.transform.DORotate(new Vector3(0, 0, angle), duration, RotateMode.LocalAxisAdd);
    }
}
