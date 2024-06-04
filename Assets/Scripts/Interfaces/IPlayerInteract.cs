using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface IPlayerInteract
{
    void GetKnockback(float pushForce, Vector2 dir, float stunForce);
    void GetJumpImpulse(float pushForce);

    NetworkObject GetNetworkObject();
}
