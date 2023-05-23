using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public List<GameObject> _connection;
    [SerializeField] private bool _test = false;
    [SerializeField] private int _energyNeeded;
    [SerializeField] private float _delaySeconds = 1.0f;
    public GameObject buttons = null;
    private Hamster _hamster;

    private void Start()
    {
        if (_test)
            StartGenerator();

        _hamster = FindObjectOfType<Hamster>();
    }

    public void ReturnButton()
    {
        _hamster.ReturnToCat();
        buttons.SetActive(false);
    }


    IEnumerator Delay(bool power = true)
    {
        yield return new WaitForSeconds(_delaySeconds);
        for (int i = 0; i < _connection.Count; i++)
        {
            if (power)
            {
                if (_connection[i] != null && _connection[i].GetComponent<IElectric>() != null)
                {
                    _connection[i].GetComponent<IElectric>().TurnOn();
                }
                else if (_connection[i] != null && _connection[i].GetComponentInChildren<IElectric>() != null)
                {
                    _connection[i].GetComponentInChildren<IElectric>().TurnOn();
                }
            }
            else
            {

                if (_connection[i] != null && _connection[i].GetComponent<IElectric>() != null)
                {
                    _connection[i].GetComponent<IElectric>().TurnOff();
                }
                else if (_connection[i] != null && _connection[i].GetComponentInChildren<IElectric>() != null)
                {
                    _connection[i].GetComponentInChildren<IElectric>().TurnOff();
                }
            }
        }
    }

    public int EnergyNeeded { get { return _energyNeeded; } }

    public void StartGenerator(bool start = true)
    {
        if (EnergyNeeded <= _hamster.Energy)
        {
            _hamster.AddEnergy(-EnergyNeeded);
            StartCoroutine(Delay(start));
        }
    }
}