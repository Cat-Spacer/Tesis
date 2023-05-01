using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterInput : MonoBehaviour
{
    Hamster _hamster;
    Vector3 screenPosition;
    Vector3 targetPosition;

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
        targetPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        //targetPosition = Camera.main.ScreenPointToRay(screenPosition, Camera.MonoOrStereoscopicEye.Mono).origin;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _hamster.GetInTube(targetPosition);
        }
    }

}
