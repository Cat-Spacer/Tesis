using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] CustomMovement _player;
    [SerializeField] GameObject[] liberateButtons;
    [SerializeField] GameObject interactButton;
    [SerializeField] Slider trapSlider;

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void TrapEvent(bool trapState, float trapLife)
    {
        if (trapState)
        { 
            foreach (var button in liberateButtons)
            {
                button.gameObject.SetActive(true);
            }
            trapSlider.maxValue = trapLife;
            trapSlider.gameObject.SetActive(true);
        }
        else
        {
            foreach (var button in liberateButtons)
            {
                button.gameObject.SetActive(false);
            }
            trapSlider.gameObject.SetActive(false);
        }
    }

    public void TrapLifeUpdate(float trapLife)
    {
        trapSlider.value = trapLife;
    }

    public void InteractEvent(bool interactState)
    {
        if (interactState) interactButton.gameObject.SetActive(true);
        else interactButton.gameObject.SetActive(false);
    }
}