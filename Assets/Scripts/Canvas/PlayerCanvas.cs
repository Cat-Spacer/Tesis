using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] GameObject interactButton;

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void InteractEvent(bool interactState)
    {
        if (interactState) interactButton.gameObject.SetActive(true);
        else interactButton.gameObject.SetActive(false);
    }
}
