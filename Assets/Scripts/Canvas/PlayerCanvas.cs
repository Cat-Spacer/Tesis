using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] PlayerCharacter playerCharacter;
    [SerializeField] protected GameObject p1InteractButton;
    [SerializeField] protected GameObject p2InteractButton;
    [SerializeField] protected GameObject p1Indicator;
    [SerializeField] protected GameObject p2Indicator;

    protected GameObject interactButton;
    protected GameObject playerIndicator;

    protected virtual void Start()
    {
        EventManager.Instance.Subscribe(EventType.ViewPlayerIndicator, OnViewPlayerIndicator);
        interactButton = p1InteractButton;
        playerIndicator = p1Indicator;
    }

    protected void OnViewPlayerIndicator(object[] obj)
    {
        var state = (bool)obj[0];
        playerIndicator.SetActive(state);
    }

    public virtual void SetPlayerInteractKeys(SO_Inputs inputs)
    {
        if (inputs.inputType == Type.WASD)
        {
            interactButton = p1InteractButton;
            playerIndicator = p1Indicator;
        }
        else
        {
            interactButton = p2InteractButton;
            playerIndicator = p2Indicator;
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
