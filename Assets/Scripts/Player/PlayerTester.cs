using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTester : MonoBehaviour
{
    [SerializeField] private EnergyPower _energyPower;

    void Update()
    {
        //_energyPower.EnergyDrain(20f);
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.LeftShift)) && _energyPower.EnergyDrain(20f))
            Debug.Log($"Puedo saltar, golper o dashear");
        else
            _energyPower.EnergyRegen();
    }
}