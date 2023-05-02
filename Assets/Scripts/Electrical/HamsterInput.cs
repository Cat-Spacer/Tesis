using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterInput : MonoBehaviour
{
    Hamster _hamster;
    Vector3 screenPosition;
    List<Vector3> targetPosition = new List<Vector3>();

    public HamsterInput(Hamster ham)
    {
        _hamster = ham;
    }

    public void OnUpdate()
    {
        Control();
    }

    void Control()
    {
        screenPosition = Input.mousePosition;
        //targetPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        foreach (var c in Camera.allCameras) targetPosition.Add(c.ScreenToWorldPoint(screenPosition));

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            foreach (var t in targetPosition)
            {
                _hamster.GetInTube(t);
            }
        }
    }
}
