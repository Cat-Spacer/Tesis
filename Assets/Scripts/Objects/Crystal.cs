using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] Crystal _nextCrystal;
    [SerializeField] bool _forceStart = false;
    [SerializeField] LineCollision _line;
    [SerializeField] int _crystalNumber, _cristalsLenght = 0;
    [SerializeField] bool _lastCrystal/*, _usedCrystal = false*/;
    [SerializeField] GameObject _door;
    [SerializeField] Crystal _nextCrystalIn;

    private void Start()
    {
        if (!_line)
            _line = GetComponent<LineCollision>();

        //_line = FindObjectOfType<LineCollision>();
    }
    private void Update()
    {
        if (_forceStart/* && !_usedCrystal*/)
        {//                                                                  added  ↓  added
            _line.SetLight(_nextCrystal.transform.position, _crystalNumber, _cristalsLenght, this, _nextCrystalIn);
            _forceStart = false;
            //_usedCrystal = true;
        }
    }

    public void CheckCrystal()
    {
        if (_lastCrystal)
        {
            Debug.Log(gameObject.name + " last");
            _door.SetActive(!_door.activeSelf);
        }
        else
        {
            Debug.Log("not last");
        }
    }

    bool called = false;
    public void CallCrystal()
    {
        if (called) return;
        Debug.Log("call");
        if (_nextCrystalIn == null) return;//                           added  ↓  added
        _line.SetLight(_nextCrystal.transform.position, _crystalNumber, _cristalsLenght, this, _nextCrystalIn);
        called = true;
    }
}
