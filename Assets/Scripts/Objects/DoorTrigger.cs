using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : Obstacle, IDamageable
{
    [SerializeField] private GameObject[] targetA;
    [SerializeField] private GameObject[] targetB;
    [SerializeField] private Crystal targetC;
    private float _currentLife;
    public float dmg;

    public void GetDamage(float dmg)
    {
        Debug.Log("interactuando");

        foreach (var item in targetA)
        {
            item.gameObject.SetActive(!item.activeSelf);
        }
        foreach (var item in targetB)
        {
            item.gameObject.SetActive(!item.activeSelf);
        }
        if (targetC != null) 
            targetC.CallCrystal(null);
    }
}
