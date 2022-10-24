using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour, IDamageable
{
    [SerializeField] private Crystal _nextCrystal, _prevCrystal;
    [SerializeField] private bool _forceStart = false, _rotable = true;
    [SerializeField] private LineCollision _line;
    [SerializeField] private int _crystalNumber, _cristalsLenght = 0, _sidesForRotation = 4;
    [SerializeField] private bool _lastCrystal/*, _usedCrystal = false*/;
    [SerializeField] private GameObject _door;
    public bool _activatedCrystal = false;
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
        {
            CallCrystal(_prevCrystal);
            _forceStart = false;
            //_usedCrystal = true;
        }
    }

    public void CheckIfLastCrystal(bool linked)
    {
        if (_lastCrystal && _door != null)
        {
            Debug.Log($"{gameObject.name} last");
            _door.SetActive(!linked);
        }
        else
            Debug.Log($"{gameObject.name} not last");
    }

    //bool called = false;
    public void CallCrystal(Crystal prevCrystal)
    {
        if (/*called || */_nextCrystal == null || prevCrystal == this) return;
        Debug.Log($"{gameObject.name} call. _prevCrystal = {prevCrystal}");
        _prevCrystal = prevCrystal;
        _line.SetLight(_prevCrystal, this, _nextCrystal);
        _activatedCrystal = true;
        //called = true;
    }

    public void RotateCrystal()
    {
        Debug.Log($"{gameObject.name} rotó {360 / _sidesForRotation} grados");
        transform.Rotate(0, 0, transform.rotation.y + (360 / _sidesForRotation));
        //called = false;
        if (_activatedCrystal) CallCrystal(_prevCrystal);
    }

    public Crystal GetPrevCrystal()
    {
        return _prevCrystal;
    }
    public Crystal GetNextCrystal()
    {
        return _nextCrystal;
    }

    public void GetDamage(float dmg)
    {
        if (_rotable)
            RotateCrystal();
    }
}
