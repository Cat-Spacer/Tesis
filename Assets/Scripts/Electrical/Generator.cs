using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public List<GameObject> _connection;
    [SerializeField] private bool _test = false;

    private void Start()
    {
        if (_test)
            StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
        foreach (var obj in _connection)
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
    }

    public void StartGenerator()
    {
        StartCoroutine(Delay());
    }
}
