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
        var player = attackable.GetNetworkObject();
        if (player == null) return;
        PunchRpc(player);
        if (LiveCameraNetwork.Instance.IsOnAir())
        {
            LiveCameraNetwork.Instance.ChangePeace(-1);
        }
    }
    [Rpc(SendTo.Everyone)]
    void PunchRpc(NetworkObjectReference player)
    {
        Debug.Log("Punch");
        player.TryGet(out NetworkObject playerNetworkObject);
        playerNetworkObject.GetComponent<IPlayerInteract>().GetKnockback(_data.punchForce, transform.right + transform.up, _data.stunForce);
        //EventManager.Instance.Trigger(EventType.OnPunchPlayer, true);
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