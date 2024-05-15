using UnityEngine;
using Unity.Netcode;

public class CatCharMultiplayer : PlayerCharacterMultiplayer
{
    public override void Special()
    {
        
    }
    public override void Punch()
    {
        var obj = Physics2D.OverlapCircle(_data.attackPoint.position, _data.attackRange.x, _data.attackableLayer);

        if (obj == null) return;
        var attackable = obj.GetComponent<IPlayerInteract>();
        if (attackable == null) return;
        attackable.GetKnockback(_data.punchForce, transform.right + transform.up, _data.stunForce);
        EventManager.Instance.Trigger("OnPunchPlayer", true);
        // if (LiveCamera.instance.IsOnAir())
        // {
        //     LiveCamera.instance.ChangePeace(-1);
        // }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_data.attackPoint.position, _data.attackRange.x);
        Gizmos.DrawWireCube(_data.groundPos.position, _data.groundCheckArea);
        Gizmos.DrawWireCube(transform.position, _data.jumpInpulseArea);
        Gizmos.DrawWireCube(transform.position, _data.interactSize);
        
        //Gizmos.DrawWireCube(_data.bounceDetectionRight.position, _data.bounceSize);
        //Gizmos.DrawWireCube(_data.bounceDetectionLeft.position, _data.bounceSize);
        
    }
}