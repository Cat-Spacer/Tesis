using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialShowTime : MonoBehaviour
{
    [SerializeField] LevelTimer timer;
    [SerializeField] private PlayerCharacter catPlayer;
    [SerializeField] private PlayerCharacter hamsterPlayer;
    private bool catCheck;
    private bool hamsterCheck;

    void ShowTime()
    {
        if (catCheck && hamsterCheck) timer.TutorialShowTime("120");
    }
    void NotShowTime()
    {
        timer.TutorialDontShowTime();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == catPlayer.gameObject)
        {
            catCheck = true;
            ShowTime();
        }
        else if (other.gameObject == hamsterPlayer.gameObject)
        {
            hamsterCheck = true;
            ShowTime();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (catPlayer != null && other.gameObject == catPlayer.gameObject)
        {
            catCheck = false;
            NotShowTime();
        }
        else if (hamsterPlayer != null && other.gameObject == hamsterPlayer.gameObject)
        {
            hamsterCheck = false;
            NotShowTime();
        }
    }
}
