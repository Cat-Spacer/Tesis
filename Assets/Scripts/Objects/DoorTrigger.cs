using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : Obstacle
{
    [SerializeField] private GameObject target;
    [SerializeField] private Crystal targetC;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        target.gameObject.SetActive(!target.activeSelf);
       if (targetC!=null) targetC.CallCrystal();
        //Destroy(gameObject);
        
    }
}
