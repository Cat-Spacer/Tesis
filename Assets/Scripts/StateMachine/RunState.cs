using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : State
{
    public override void Enter()
    {
        model.ChangeAnimationState("Run");
    }

    public override void Do()
    {
        if (!input.left_Input && !input.right_Input)
        {
            isComplete = true;
        }
    }

    public override void Exit()
    {

    }
}
