using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : State
{
    public override void Enter()
    {

    }

    public override void Do()
    {
    }

    public override void Exit()
    {
        isComplete = false;
    }
}
