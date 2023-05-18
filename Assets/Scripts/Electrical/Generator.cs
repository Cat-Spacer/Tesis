using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public List<GameObject> _connection;
    //public List<GameObject> _electricityParticle;
    [SerializeField] private bool _test = false;
    [SerializeField] private int _energyNeeded;
    [SerializeField] private float _delaySeconds = 1.0f;
    public GameObject buttons = null;
    //public List<ParticleSystem> electricParticles;
    private Hamster _hamster;

    private void Start()
    {
        if (_test)
            StartGenerator();

        _hamster=FindObjectOfType<Hamster>();
    }

    public void ReturnButton()
    {
        _hamster.ReturnToCat();
        buttons.SetActive(false);
    }

    public int EnergyNeeded { get { return _energyNeeded; } }
    //public GameObject Buttons() { return _buttons; }

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
                    //_electricityParticle[i].GetComponent<Electricty>().Activate();
                }
                else if (_connection[i] != null && _connection[i].GetComponentInChildren<IElectric>() != null)
                {
                    _connection[i].GetComponentInChildren<IElectric>().TurnOn();
                    //_electricityParticle[i].GetComponentInChildren<Electricty>().Activate();
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

    public void StartGenerator(bool start = true)
    {
        StartCoroutine(Delay(start));
    }

    //public void PowerOffGenerator() 
    //{
    //    StartCoroutine(Delay(false));
    //}
}
