using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : State
{
    public override void Enter()
    {
        model.ChangeAnimationState("Run");
        if(character.GetCharType() == CharacterType.Cat) SoundManager.instance.Play(SoundsTypes.Steps, true);
        else SoundManager.instance.Play(SoundsTypes.HamsterOnTubes, true);
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
        if(character.GetCharType() == CharacterType.Cat) SoundManager.instance.Pause(SoundsTypes.Steps);
        else SoundManager.instance.Pause(SoundsTypes.HamsterOnTubes);
        isComplete = false;
    }
}
