using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : State
{
    public override void Enter()
    {
        model.ChangeAnimationState("Jump");
    }

    public override void Do()
    {
        if (data.onGround || data.isStun || data.isPunching || data.isInteracting)
        {
            isComplete = true;
            return;
        }
        if(rb.velocity.y > 0) model.ChangeAnimationState("Jump");
        else model.ChangeAnimationState("Falling");
    }

    public override void Exit()
    {
        isComplete = false;
    }
}
