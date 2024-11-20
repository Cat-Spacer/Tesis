using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractState : State
{
    public override void Enter()
    {
        Debug.Log("Interact State");
        model.ChangeAnimationState("Interact");
        SoundManager.instance.Play(SoundsTypes.Interact);
        StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        isComplete = true;
    }
    public override void Exit()
    {
        isComplete = false;
    }
}
