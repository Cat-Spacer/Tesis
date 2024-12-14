public class RunState : State
{
    public override void Enter()
    {
        model.ChangeAnimationState("Run");
        model.PlayParticle(ParticleType.Run);
        if(character.GetCharType() == CharacterType.Cat) SoundManager.instance.Play(SoundsTypes.Steps, gameObject, true);
        else SoundManager.instance.Play(SoundsTypes.HamsterOnTubes, gameObject, true);
    }

    public override void Do()
    {
        if ((!input.left_Input && !input.right_Input) || data.isStun || data.isPunching || data.isInteracting || data.onJumpImpulse || !data.onGround || GameManager.Instance.pause)
        {
            isComplete = true;
        }
    }

    public override void Exit()
    {
        if(character.GetCharType() == CharacterType.Cat) SoundManager.instance.Pause(SoundsTypes.Steps, gameObject);
        else SoundManager.instance.Pause(SoundsTypes.HamsterOnTubes, gameObject);
        model.StopParticle(ParticleType.Run);
        isComplete = false;
    }
}
