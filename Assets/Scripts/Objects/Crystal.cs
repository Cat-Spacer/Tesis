using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{

    [SerializeField] Crystal _nextCrystal;
   
    [SerializeField] bool _forceStart = false;
    [SerializeField] LineCollision _line;
    [SerializeField] int crystalNumber;
    [SerializeField] bool _lastCrystal;
    [SerializeField] GameObject _door;
    [SerializeField] Crystal _nextCrystalIn;

    private void Start()
    {
        _line = FindObjectOfType<LineCollision>();
    }
    private void Update()
    {
        if (_forceStart)
        {
            _line.SetLight(_nextCrystal.transform.position, crystalNumber, this, _nextCrystalIn);
            _forceStart = false;
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

    bool called =  false;
    public void CallCrystal()
    {
        if (called) return;
        Debug.Log("call");
        if (_nextCrystalIn == null) return;
        _line.SetLight(_nextCrystal.transform.position, crystalNumber, this,  _nextCrystalIn);
        called = true;
    }



}
