using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpeningButton : MonoBehaviour
{
    [SerializeField] private Button3D _openDoorButton; //launcher
    [SerializeField] private Door _door; //launcherGO : effetto sul listener DOOR
    public Transform PlayerCapsule;
    void Start()
    {
        _openDoorButton.OnButtonPressed += OnDoorOpenButtonPressed; //funzione di risposta all'evento OnButtonPressed
    }

    private void OnDoorOpenButtonPressed()
    {
        //IMPLEMENTAZIONE DA STRADA:
        /*int randomrotation = Random.Range(-90, 90);
        int numSteps = (int)Mathf.Floor(randomrotation / 15f);
        int adjustedRotation = numSteps * 15;
        _door.OpenAndClose(adjustedRotation)*/

        //IMPLEMENTAZIONE:
        Vector3 othersPositionRelativeToDoor = (PlayerCapsule.transform.position - transform.position).normalized;
        //the result of the dot product returns > 0 if relative position 
        float dotResult = Vector3.Dot(othersPositionRelativeToDoor, transform.forward);
        float doorRotation = dotResult > 0 ? 90f : -90f;

        /*if (_door != null)
            _door.OpenAndClose(doorRotation);*/

        _door.OpenAndClose(doorRotation);
    }
}
