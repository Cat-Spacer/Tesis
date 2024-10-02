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
        if(rb.velocity.y > 0) model.ChangeAnimationState("Jump");
        else model.ChangeAnimationState("Falling");
        if (data.onGround) isComplete = true;
    }

    public override void Exit()
    {

    }
}
