using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HamsterCanvas : PlayerCanvas
{
    [SerializeField] private GameObject p1LeftKey;
    [SerializeField] private GameObject p1RightKey;
    [SerializeField] private GameObject p1UpKey;
    [SerializeField] private GameObject p1DownKey;
    
    [SerializeField] private GameObject p2LeftKey;
    [SerializeField] private GameObject p2RightKey;
    [SerializeField] private GameObject p2UpKey;
    [SerializeField] private GameObject p2DownKey;
    
    private GameObject leftKey;
    private GameObject rightKey;
    private GameObject upKey;
    private GameObject downKey;

    protected override void Start()
    {
        EventManager.Instance.Subscribe(EventType.ViewPlayerIndicator, OnViewPlayerIndicator);
        interactButton = p1InteractButton;
        playerIndicator = p1Indicator;
        leftKey = p1LeftKey;
        rightKey = p1RightKey;
        upKey = p1UpKey;
        downKey = p1DownKey;
    }

    public void HideArrows()
    {
        upKey.SetActive(false);
        downKey.SetActive(false);
        rightKey.SetActive(false);
        leftKey.SetActive(false);
    }
    public void CheckTubeDirections(Tube tube)
    {
        if (tube.HasUpPath())
        {
            upKey.SetActive(true);
        }
        if (tube.HasDownPath())
        {
            downKey.SetActive(true);
        }
        if (tube.HasRightPath())
        {
            rightKey.SetActive(true);
        }
        if (tube.HasLeftPath())
        {
            leftKey.SetActive(true);
        }
    }

    public override void SetPlayerInteractKeys(SO_Inputs inputs)
    {
        if (inputs.inputType == Type.WASD)
        {
            interactButton = p1InteractButton;
            playerIndicator = p1Indicator;
            leftKey = p1LeftKey;
            rightKey = p1RightKey;
            upKey = p1UpKey;
            downKey = p1DownKey;
        }
        else
        {
            interactButton = p2InteractButton;
            playerIndicator = p2Indicator;
            leftKey = p2LeftKey;
            rightKey = p2RightKey;
            upKey = p2UpKey;
            downKey = p2DownKey;
        }
        
    }
}
