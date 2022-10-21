using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour, IDamageable
{
    [SerializeField] private Crystal _nextCrystal;
    [SerializeField] private bool _forceStart = false;
    [SerializeField] private LineCollision _line;
    [SerializeField] private int _crystalNumber, _cristalsLenght = 0, _sidesForRotation = 4;
    [SerializeField] private bool _lastCrystal/*, _usedCrystal = false*/;
    [SerializeField] private GameObject _door;
    // [SerializeField] Crystal _nextCrystalIn;

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
            _line.SetLight(_nextCrystal.transform.position, _crystalNumber, _cristalsLenght, this, _nextCrystal);
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
        if (_nextCrystal == null) return;//                           added  ↓  added
        _line.SetLight(_nextCrystal.transform.position, _crystalNumber, _cristalsLenght, this, _nextCrystal);
        called = true;
    }

    public void RotateCrystal()
    {
        float rotationZ = transform.rotation.y / _sidesForRotation;
        transform.Rotate(0, 0, rotationZ);
    }

    public void GetDamage(float dmg)
    {
        RotateCrystal();
    }
}
