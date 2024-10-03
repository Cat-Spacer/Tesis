using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : State
{
    public override void Enter()
    {
        
    }

    public override void Do()
    {
        if (data.onGround || data.isStun)
        {
            isComplete = true;
            return;
        }
        if(rb.velocity.y > 0) model.ChangeAnimationState("Jump");
        else model.ChangeAnimationState("Falling");
    }

    public override void Exit()
    {

    }
}
