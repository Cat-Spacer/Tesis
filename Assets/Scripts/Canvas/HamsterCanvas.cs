using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterCanvas : PlayerCanvas
{
    [SerializeField] private GameObject leftArrow, rightArrow, upArrow, downArrow;

    private void Start()
    {
        upArrow.SetActive(false);
        downArrow.SetActive(false);
        rightArrow.SetActive(false);
        leftArrow.SetActive(false);
    }

    public void HideArrows()
    {
        upArrow.SetActive(false);
        downArrow.SetActive(false);
        rightArrow.SetActive(false);
        leftArrow.SetActive(false);
    }
    public void CheckTubeDirections(Tube tube)
    {
        var connections = tube.Connections();
        if (connections.Contains("Up"))
        {
            upArrow.SetActive(true);
        }
        if (connections.Contains("Down"))
        {
            downArrow.SetActive(true);
        }
        if (connections.Contains("Right"))
        {
            rightArrow.SetActive(true);
        }
        if (connections.Contains("Left"))
        {
            leftArrow.SetActive(true);
        }
    }

}
