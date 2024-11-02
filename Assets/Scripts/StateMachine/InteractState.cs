using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractState : State
{
    public override void Enter()
    {
        model.ChangeAnimationState("Interact");
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
        isComplete = false;
    }
}
