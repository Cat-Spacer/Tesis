using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkState : State
{
    public override void Enter()
    {
        model.ChangeAnimationState("Shrink");
        isComplete = true;
    }

    public override void Do()
    {
        Debug.Log("On shrink state");
    }

    public override void Exit()
    {
        isComplete = false;
    }
}
