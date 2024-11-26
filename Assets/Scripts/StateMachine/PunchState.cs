using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchState : State
{
    public override void Enter()
    {
        Debug.Log("Enter Punch");
        model.ChangeAnimationState("Punch");
    }

    public override void Do()
    {
        Debug.Log("Do Punch");
        if (!data.isPunching)
        {
            isComplete = true;
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit Punch");
        isComplete = false;
    }
}
