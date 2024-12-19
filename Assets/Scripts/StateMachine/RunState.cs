public class RunState : State
{
    public override void Enter()
    {
        model.ChangeAnimationState("Run");
        model.PlayParticle(ParticleType.Run);
        SoundManager.instance.Play(
            character.GetCharType() == CharacterType.Cat ? SoundsTypes.Steps : SoundsTypes.HamsterSteps, gameObject,
            true);
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
        SoundManager.instance.Pause(
            character.GetCharType() == CharacterType.Cat ? SoundsTypes.Steps : SoundsTypes.HamsterSteps, gameObject);
        model.StopParticle(ParticleType.Run);
        isComplete = false;
    }
}
