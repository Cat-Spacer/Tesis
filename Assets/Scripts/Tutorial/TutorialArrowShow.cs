using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArrowShow : MonoBehaviour
{
    [SerializeField] private GameObject text;
    [SerializeField] GameObject arrow;
    [SerializeField] private PlayerCharacter catPlayer;
    [SerializeField] private PlayerCharacter hamsterPlayer;
    private bool catCheck;
    private bool hamsterCheck;

    private void Start()
    {
        EventManager.Instance.Subscribe(EventType.OnFinishGame, OnFinishGame);
        if(text != null) text.SetActive(false);
        if(arrow != null) arrow.SetActive(false);
    }

    private void OnFinishGame(object[] obj)
    {
        if(text != null) text.SetActive(false);
        if(arrow != null) arrow.SetActive(false);
    }

    void ShowTutorial()
    {
        if (catCheck && hamsterCheck)
        {
            if(text != null) text.SetActive(true);
            if(arrow != null) arrow.SetActive(true);
        }

    }
    void DeactivateTutorial()
    {
        if(text != null) text.SetActive(false);
        if(arrow != null) arrow.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == catPlayer.gameObject)
        {
            catCheck = true;
            ShowTutorial();
        }
        else if (other.gameObject == hamsterPlayer.gameObject)
        {
            hamsterCheck = true;
            ShowTutorial();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (catPlayer != null && other.gameObject == catPlayer.gameObject)
        {
            catCheck = false;
            DeactivateTutorial();
        }
        else if (hamsterPlayer != null && other.gameObject == hamsterPlayer.gameObject)
        {
            hamsterCheck = false;
            DeactivateTutorial();
        }
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe(EventType.OnFinishGame, OnFinishGame);
    }
}
