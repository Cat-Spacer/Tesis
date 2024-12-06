using UnityEngine;

public class InteractState : State
{
    private float _timer = 0.5f;
    private float _resetTimer = 0.5f;
    public override void Enter()
    {
        model.ChangeAnimationState("Interact");
        
        SoundManager.instance.Play(SoundsTypes.Interact, gameObject);
   
    }
    public override void Do()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            isComplete = true;
        }
    }


    public override void Exit()
    {
        isComplete = false;
        _timer = _resetTimer;
    }
}
