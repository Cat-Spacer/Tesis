using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractState : State
{
    Coroutine coroutine;
    public override void Enter()
    {
        model.ChangeAnimationState("Interact");
        SoundManager.instance.Play(SoundsTypes.Interact, gameObject);
        coroutine = StartCoroutine(Delay());
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
