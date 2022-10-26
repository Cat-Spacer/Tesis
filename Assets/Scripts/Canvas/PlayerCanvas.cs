using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] CustomMovement _player;
    [SerializeField] GameObject[] buttons;
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    public void TrapEvent(bool trapState)
    {
        if (trapState)
        { 
            foreach (var button in buttons)
            {
                button.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (var button in buttons)
            {
                button.gameObject.SetActive(false);
            }
        }
    }
}
