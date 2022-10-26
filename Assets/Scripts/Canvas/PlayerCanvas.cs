using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] CustomMovement _player;
    [SerializeField] GameObject[] liberateButtons;
    [SerializeField] GameObject interactButton;
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    public void TrapEvent(bool trapState)
    {
        if (trapState)
        { 
            foreach (var button in liberateButtons)
            {
                button.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (var button in liberateButtons)
            {
                button.gameObject.SetActive(false);
            }
        }
    }
    public void InteractEvent(bool interactState)
    {
        if (interactState) interactButton.gameObject.SetActive(true);
        else interactButton.gameObject.SetActive(false);
    }
}
