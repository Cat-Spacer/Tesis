using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevateState : State
{
    public override void Enter()
    {
        model.ChangeAnimationState("Elevate");
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        isComplete = true;
    }
    public override void Do()
    {

    }

    public override void Exit()
    {

    }
}
