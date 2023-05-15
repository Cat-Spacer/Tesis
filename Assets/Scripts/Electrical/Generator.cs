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
    public List<ParticleSystem> electricParticles;
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
        foreach (var obj in _connection)
        {
            if (power)
            {
                if (obj != null && obj.GetComponent<IElectric>() != null)
                {
                    obj.GetComponent<IElectric>().TurnOn();
                }
                else if (obj != null && obj.GetComponentInChildren<IElectric>() != null)
                {
                    obj.GetComponentInChildren<IElectric>().TurnOn();
                }
            }
            else
            {

                if (obj != null && obj.GetComponent<IElectric>() != null)
                {
                    obj.GetComponent<IElectric>().TurnOff();
                }
                else if (obj != null && obj.GetComponentInChildren<IElectric>() != null)
                {
                    obj.GetComponentInChildren<IElectric>().TurnOff();
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
