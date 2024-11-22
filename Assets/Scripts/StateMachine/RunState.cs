using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : State
{
    public override void Enter()
    {
        model.ChangeAnimationState("Run");
        if(character.GetCharType() == CharacterType.Cat) SoundManager.instance.Play(SoundsTypes.Steps, gameObject, true);
        else SoundManager.instance.Play(SoundsTypes.HamsterOnTubes, gameObject, true);
    }

    public override void Do()
    {
        if ((!input.left_Input && !input.right_Input) || data.isStun || data.isPunching || data.isInteracting || data.onJumpImpulse)
        {
            isComplete = true;
        }
    }

    public override void Exit()
    {
        if(character.GetCharType() == CharacterType.Cat) SoundManager.instance.Pause(SoundsTypes.Steps, gameObject);
        else SoundManager.instance.Pause(SoundsTypes.HamsterOnTubes, gameObject);
        isComplete = false;
    }
}
