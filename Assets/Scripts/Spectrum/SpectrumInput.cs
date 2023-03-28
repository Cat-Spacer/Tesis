using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumInput
{
    SpectrumBehaviour _spectrum;
    Vector3 screenPosition;
    Vector3 targetPosition;

    public SpectrumInput(SpectrumBehaviour sb)
    {
        _spectrum = sb;
    }

    public void OnUpdate()
    {
        Control();
    }
    void Control()
    {
        screenPosition = Input.mousePosition;
        targetPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        _spectrum.Move(targetPosition);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _spectrum.Clone();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _spectrum.SpectrumMode();
        }
    }
}
