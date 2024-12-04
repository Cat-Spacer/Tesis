using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWall : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private PlayerCharacter catPlayer;
    [SerializeField] private PlayerCharacter hamsterPlayer;
    [SerializeField] private GameObject wallLeft;
    [SerializeField] private GameObject wallRight;
    private bool catCheck;
    private bool hamsterCheck;
    private bool alreadyShowed;
    void ShowTutorial()
    {
        if (catCheck && hamsterCheck && !alreadyShowed)
        {
            alreadyShowed = true;
            wallLeft.SetActive(true);
            StartCoroutine(FinishTutorial());
        }
    }
    void DeactivateTutorial()
    {
        if(wallRight != null) wallRight.SetActive(false);
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
            //DeactivateTutorial();
        }
        else if (hamsterPlayer != null && other.gameObject == hamsterPlayer.gameObject)
        {
            hamsterCheck = false;
            //DeactivateTutorial();
        }
    }

    IEnumerator FinishTutorial()
    {
        yield return new WaitForSeconds(time);
        
        DeactivateTutorial();
    }
}
