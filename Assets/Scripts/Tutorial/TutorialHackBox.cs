using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHackBox : MonoBehaviour
{
    [SerializeField] private bool catEnter;
    [SerializeField] private bool hamsterEnter;
    [SerializeField] private bool hacked;
    [SerializeField] private PlayerCharacter catPlayer;
    [SerializeField] private PlayerCharacter hamsterPlayer;
    [SerializeField] private TutorialAttackBox attackBox;
    
    void ActivateHack()
    {
        if (hacked) return;
        if(catEnter && hamsterEnter)
        {
            LiveCamera.instance.StartTutorialHackCamera();
            attackBox.CanAttack(true);
            hacked = true;
        }
    }

    void DeactivateHack()
    {
        if(!hacked) return;
        LiveCamera.instance.StopTutorialHackCamera();
        attackBox.CanAttack(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == catPlayer.gameObject)
        {
            catEnter = true;
            ActivateHack();
        }
        else if (other.gameObject == hamsterPlayer.gameObject)
        {
            hamsterEnter = true;
            ActivateHack();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (catPlayer != null && other.gameObject == catPlayer.gameObject)
        {
            catEnter = false;
            DeactivateHack();
        }
        else if (hamsterPlayer != null && other.gameObject == hamsterPlayer.gameObject)
        {
            hamsterEnter = false;
            DeactivateHack();
        }
    }
}
