using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterChar : PlayerCharacter
{
    public override void Punch()
    {
        var obj = Physics2D.OverlapCircle(_data.attackPoint.position, _data.attackRange.x, _data.attackableLayer);

        if (obj == null) return;
        var attackable = obj.GetComponent<IPlayerInteract>();
        if (attackable == null) return;
        
        Debug.Log("PUNCH " + attackable);
        attackable.GetKnockback(_data.punchForce, transform.right + transform.up, _data.stunForce);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_data.groundPos.position, _data.groundCheckArea);
        Gizmos.DrawWireSphere(_data.attackPoint.position, _data.attackRange.x);
        //Gizmos.DrawWireCube(_data.bounceDetectionRight.position, _data.bounceSize);
        //Gizmos.DrawWireCube(_data.bounceDetectionLeft.position, _data.bounceSize);
    }
}
