using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{
    public override void Enter()
    {
        model.GetStun(true);
        if(character.GetCharType() == CharacterType.Cat) SoundManager.instance.Play(SoundsTypes.CatDamage, gameObject);
        else SoundManager.instance.Play(SoundsTypes.HamsterDamage, gameObject);

        model.PlayParticle(ParticleType.Damage);
    }

    public override void Do()
    {
        Debug.Log("On Stun state");
        if (!data.isStun)
        {
            isComplete = true;
        }
    }

    public override void Exit()
    {
        isComplete = false;
        model.GetStun(false);
    }
}
