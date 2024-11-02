using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;

public class IdleState : State
{
    public override void Enter()
    {
        model.ChangeAnimationState("Idle");
    }

    public override void Do()
    {
        if (!data.onGround || input.left_Input || input.right_Input || data.isStun || data.isPunching || data.isInteracting)
        {
            isComplete = true;
        }
    }

    public override void Exit()
    {
        isComplete = false;
    }
}
