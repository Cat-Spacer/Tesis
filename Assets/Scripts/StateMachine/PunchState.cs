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
        if (!data.isPunching)
        {
            isComplete = true;
        }
    }

    public override void Exit()
    {
        isComplete = false;
    }
}
