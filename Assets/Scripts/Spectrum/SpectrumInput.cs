using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumInput : MonoBehaviour
{
    Vector3 screenPosition;
    public Vector3 targetPosition;
    private void Update()
    {
        screenPosition = Input.mousePosition;
        targetPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        if (true)
        {

        }
    }
}
