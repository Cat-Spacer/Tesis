using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchState : State
{
    public override void Enter()
    {
        model.ChangeAnimationState("Punch");
    }

    public override void Do()
    {
        Debug.Log("Do");
        Debug.Log(data.isPunching);
        if (!data.isPunching)
        {
            Debug.Log("Entre");
            isComplete = true;
        }
    }

    public override void Exit()
    {
        isComplete = false;
    }
}
