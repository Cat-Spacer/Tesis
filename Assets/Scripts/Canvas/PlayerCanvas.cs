using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] PlayerCharacter playerCharacter;
    [SerializeField] protected GameObject p1InteractButton;
    [SerializeField] protected GameObject p2InteractButton;

    protected GameObject interactButton;

    protected virtual void Start()
    {
        interactButton = p1InteractButton;
    }
    public virtual void SetPlayerInteractKeys(SO_Inputs inputs)
    {
        if (inputs.inputType == Type.WASD)
        {
            interactButton = p1InteractButton;
        }
        else
        {
            interactButton = p2InteractButton;
        }
    }
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
