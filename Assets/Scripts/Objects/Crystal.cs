using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour, IDamageable
{
    [SerializeField] private Crystal _nextCrystal;
    public Crystal _prevCrystal;
    public LineCollision line { get; set; }
    [SerializeField] private bool _forceStart = false, _rotable = true;
    [SerializeField] private int _crystalNumber, _cristalsLenght = 0, _sidesForRotation = 4;
    [SerializeField] private bool _lastCrystal/*, _usedCrystal = false*/;
    [SerializeField] private GameObject _door;
    public bool _activatedCrystal = false;
    // [SerializeField] Crystal _nextCrystalIn;

    private void Start()
    {
        if (!line)
            line = GetComponent<LineCollision>();

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

    /// <summary>
    /// Desactivates a GameObject if it's the last crystal
    /// </summary>
    /// <param name="linked"></param>
    public void CheckIfLastCrystal(bool linked)
    {
        if (_lastCrystal && _door != null)
        {
            Debug.Log($"{gameObject.name} last");
            _door.SetActive(!linked);
            //CallCrystal(_prevCrystal);
        }
        else
            Debug.Log($"{gameObject.name} not last");
    }

    //bool called = false;
    public void CallCrystal(Crystal prevCrystal)
    {
        if (/*called || */prevCrystal == this) return;
        Debug.Log($"{gameObject.name} call. _prevCrystal = {prevCrystal}");
        _prevCrystal = prevCrystal;
        line.SetLight(_prevCrystal, this, _nextCrystal);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"{gameObject.name} colisiono con {collision.gameObject.name}");
        if (!_prevCrystal) return;
        _prevCrystal.CallCrystal(_prevCrystal._prevCrystal);
        CallCrystal(_prevCrystal);
        Debug.Log($"{gameObject.name} entre a los calls");
    }
}
