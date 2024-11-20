using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevateState : State
{
    public override void Enter()
    {
        Debug.Log("Elevate");
        model.ChangeAnimationState("Elevate");
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(.5f);
        data.onJumpImpulse = false;
        isComplete = true;
    }
    public override void Exit()
    {
        isComplete = false;
    }
}
