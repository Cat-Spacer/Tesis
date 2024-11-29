using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAttackBox : MonoBehaviour
{
    [SerializeField] private bool canAttack;
    [SerializeField] private Inputs catInputs;
    [SerializeField] private Inputs hamsterInputs;
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (canAttack)
            {
                catInputs.GivePunchButton();
                hamsterInputs.GivePunchButton();
            }
            else
            {
                catInputs.TakeAwayPunchButton();
                hamsterInputs.TakeAwayPunchButton();
            }
        }
    }

    public void CanAttack(bool state)
    {
        canAttack = state;
    }
}
