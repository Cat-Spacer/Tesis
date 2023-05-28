using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Hamster))]
public class HamsterEnergy : MonoBehaviour
{
    private Hamster _hamster = null;
    [SerializeField] private GameObject[] energyIcons = null;

    void Start()
    {
        if (_hamster == null) _hamster = GetComponent<Hamster>();
    }

    void Update()
    {
        EnergyManagment();
    }

    private void EnergyManagment()
    {
        if (!_hamster || energyIcons == null) return;

        for (int i = 0; i < energyIcons.Length && energyIcons[i] != null; i++)
            if ((i + 1) <= _hamster.Energy)
                energyIcons[i].SetActive(true);
            else
                energyIcons[i].SetActive(false);
    }
}