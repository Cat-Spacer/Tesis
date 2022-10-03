using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{

    [SerializeField] Crystal _nextCrystal;
   
    [SerializeField] bool _forceStart = false;
    [SerializeField] LineCollision _line;
    [SerializeField] int crystalNumber;

    private void Start()
    {
        _line = FindObjectOfType<LineCollision>();
    }
    private void Update()
    {
        if (_forceStart)
        {
            _line.SetLight(_nextCrystal.transform.position, crystalNumber);
            _forceStart = false;
        }
           
    }

    

}
