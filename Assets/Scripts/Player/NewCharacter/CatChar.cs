using UnityEngine;

public class CatChar : PlayerCharacter
{
    //private MushroomType _type;
    public CatCanvas canvas;
    //private bool _canInteract = false, canSpit = false;
    protected override void Update()
    {
        base.Update();
        
    }

    // public override void Interact()
    // {
    //     var coll = Physics2D.OverlapCircle(transform.position, 2, _data.interactMask);
    //     if (!coll) return;
    //     var interact = coll.gameObject.GetComponent<IInteract>();
    //     if (interact == null) return;
    //     
    //     interact.Interact();
    // }

    public override void Special()
    {
        
    }
    public override void Punch()
    {
        var obj = Physics2D.OverlapCircle(_data.attackPoint.position, _data.attackRange.x, _data.attackableLayer);

        if (obj == null) return;
        var attackable = obj.GetComponent<IPlayerInteract>();
        if (attackable == null) return;
        
        Debug.Log("PUNCH " + attackable);
        attackable.GetKnockback(_data.punchForce, transform.right + transform.up, _data.stunForce);
        if (LiveCamera.instance.IsOnAir())
        {
            LiveCamera.instance.ChangePeace(-1);
        }
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