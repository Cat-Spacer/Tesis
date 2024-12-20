using UnityEngine;
using UnityEngine.InputSystem;

public class CatCharacter : PlayerCharacter
{
    private void Awake()
    {
        if (GameManager.Instance) GameManager.Instance.SetCatCharacter = this;
    }

    protected override void OnUpdatePoints(object[] obj)
    {
        base.OnUpdatePoints(obj);
       var type = (CharacterType)obj[0];

        if (type == CharacterType.Cat)
            _model.PlayParticle(ParticleType.ExtraScore);
    }

    public override void Special()
    {
        if (!_data.onGround && !_data.isPunching && !_data.isStun) return;
        _data.onJumpImpulse = true;
        SoundManager.instance.Play(SoundsTypes.CatDash);
        var otherPlayer = Physics2D.OverlapBox(transform.position, _data.jumpInpulseArea, 0, _data.playerMask);
        if (otherPlayer)
        {
            var body = otherPlayer.gameObject.GetComponent<Rigidbody2D>();
            if (body == null) return;
            body.AddForce(Vector3.up * _data.jumpImpulse);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_data.attackPoint.position, _data.attackRange.x);
        Gizmos.DrawWireCube(_data.groundPos.position, _data.groundCheckArea);
        Gizmos.DrawWireCube(transform.position, _data.jumpInpulseArea);
        //Gizmos.DrawWireCube(transform.position, _data.interactSize);
        
        //Gizmos.DrawWireCube(_data.bounceDetectionRight.position, _data.bounceSize);
        //Gizmos.DrawWireCube(_data.bounceDetectionLeft.position, _data.bounceSize);
        
    }
 
}