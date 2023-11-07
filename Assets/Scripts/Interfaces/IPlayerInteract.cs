using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInteract
{
    void GetKnockback(float pushForce, Vector2 dir, float stunForce);
    void GetJumpImpulse(float pushForce);
}
