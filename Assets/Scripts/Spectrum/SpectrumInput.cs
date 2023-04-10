using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumInput
{
    SpectrumBehaviour _spectrum;
    SpectrumInventory _inventory;
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            _spectrum.SaveObject();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _spectrum.CloneFromInventory(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _spectrum.CloneFromInventory(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _spectrum.CloneFromInventory(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _spectrum.CloneFromInventory(3);
        }
    }
}
