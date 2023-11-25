using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour,IPlayerInteract 
{
    [SerializeField] protected CharacterData _data;
    private CharacterModel _model;
    protected Rigidbody2D _rb;
    protected Action _HitAction = delegate {  };
    protected Action _DebuffAction = delegate {  };
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _data = GetComponent<CharacterData>();
        _model = GetComponent<CharacterModel>();
        _data.characterPhysicsMat = _rb.sharedMaterial;
        _data.gravity = new Vector2(0, -Physics2D.gravity.y);
    }


    protected virtual void FixedUpdate()
    {
        ArtificialGravity();
        GroundFriction();
        _HitAction();
        _DebuffAction();
        IsFalling();
    }
#region MOVEMENT
    public void Movement(bool onInput ,int direction)
    {
        if (!onInput || _data.isStun)
        {
            _data.isRunning = false;
            if (!_data.isJumping && OnGround())
            {
                _model.ChangeAnimationState("Idle");
            }
            return;
        }
        FaceDirection(direction);
        _data.isRunning = true;
        _data.isAirRunning = false;

        if (OnGround() && !_data.isJumping)
        {
            var xMove =  _rb.velocity.x + (_data.faceDirection * _data.runAcel * Time.fixedDeltaTime);
        
            if(_data.faceDirection == 1 && _data.maxSpeed > Mathf.Abs(_rb.velocity.x)) xMove = Mathf.Clamp(_rb.velocity.x + xMove, 0, _data.maxSpeed);
            else if (_data.faceDirection == -1 && _data.maxSpeed > Mathf.Abs(_rb.velocity.x)) xMove = Mathf.Clamp(_rb.velocity.x + xMove, -_data.maxSpeed, 0);
      
            _rb.velocity = new Vector2(xMove, _rb.velocity.y);
            _model.ChangeAnimationState("Run");
            //Debug.Log("Ground run");
        }
        else
        {
            var xMove = _rb.velocity.x + (_data.faceDirection * _data.airRunAcel * Time.fixedDeltaTime);
        
            if(_data.faceDirection == 1 && _data.maxSpeed > Mathf.Abs(_rb.velocity.x)) xMove = Mathf.Clamp(_rb.velocity.x + xMove, 0, _data.maxSpeed);
            else if (_data.faceDirection == -1 && _data.maxSpeed > Mathf.Abs(_rb.velocity.x)) xMove = Mathf.Clamp(_rb.velocity.x + xMove, -_data.maxSpeed, 0);
            
            _rb.velocity = new Vector2(xMove, _rb.velocity.y);
            //Debug.Log("Air run");
        }
    }
    public void GroundFriction()
    {
        if (OnGround() && !_data.isRunning)
        {
            var decelerate = Mathf.Lerp(_rb.velocity.x, 0, _data.groundFriction);
            _rb.velocity = new Vector2( decelerate, _rb.velocity.y);
        }
    }
    public void FaceDirection(int direction)
    {
        if (_data.isStun) return;
        _data.faceDirection = direction;
        if (_data.faceDirection == 1) transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        else transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
    }
#endregion
#region JUMP
    public void JumpUp(bool jump)
    {
        Debug.Log("Try Jump");
        if (_data.isStun || _data.isJumping) return;
        Debug.Log("Jump");
        if (jump && OnGround())
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _data.jumpForce);
            _data.isJumping = true;
            _data.jumpCounter = 0;
        }

        if (_rb.velocity.y > 0 && _data.isJumping)
        {
            Debug.Log("Jump");
            _model.ChangeAnimationState("Jump");
            _data.jumpCounter += Time.deltaTime;
            if (_data.jumpCounter > _data.jumpTime)
            {
                _data.isJumping = false;
                StopJump();
            }
            else
            {
                _rb.velocity += _data.gravity * _data.jumpMultiplier * Time.fixedDeltaTime;
            }
        }

    }
    public void StopJump()
    {
        if (_data.isStun) return;
        _data.isJumping = false;
        float stopForce = Mathf.Lerp(_rb.velocity.y, 0,  _data.jumpStopForce);
        _rb.velocity = new Vector2(_rb.velocity.x, stopForce);
        
        // if (!OnGround())
        // {
        //     _data.isJumping = false;
        // }
    }
#endregion
#region KNOCKBACK

public void GetKnockback(float pushForce,Vector2 dir,float stunForce)
{
    _data.knockbackDir = new Vector2(dir.x,  _data.knockbackDir.y);
    _data.knockbackForce = pushForce;
    _data.onKnockback = true;
    _DebuffAction += KnockbackEffect;
    GetStun(stunForce);
}



void KnockbackEffect()
{
    _data.knockbackCounter += Time.fixedDeltaTime;
    if (_data.knockbackCounter < _data.knockbackTime)
    {
        _rb.velocity = _data.knockbackDir * _data.knockbackForce;
        _data.knockbackForce -= Time.fixedDeltaTime * _data.knockbackSpeedDecel;
    }
    else
    {
        _DebuffAction -= KnockbackEffect;
        _data.knockbackCounter = 0;
        _data.onKnockback = false;
    }
}

#endregion
#region STUN

public void Stun()
{
    _data.stunCounter -= Time.deltaTime;
    if (_data.stunCounter <= 0)
    {
        _DebuffAction -= Stun;
        _data.isStun = false;
        _model.GetStun(_data.isStun);
        _data.stunCounter = 0;
    }
}

public void GetStun(float intensity)
{
    if (!_data.isStun)
    {
        _data.isStun = true;
        _model.GetStun(_data.isStun);
        _DebuffAction += Stun;
    }
    _data.stunCounter += intensity;
}

#endregion

#region CHAR_ACTIONS

public virtual void Punch(){}

public virtual void JumpImpulse()
{
    var otherPlayer = Physics2D.OverlapBox(transform.position, _data.jumpInpulseArea, 0, _data.playerMask);
    Debug.Log(otherPlayer);
    if (otherPlayer)
    {
        var playerInteract = otherPlayer.gameObject.GetComponent<IPlayerInteract>();
        if (playerInteract != null)
        {
            playerInteract.GetJumpImpulse(_data.jumpImpulse); 
            Debug.Log("JumpUmpulse");

        }
    }
}

public void GetJumpImpulse(float pushForce)
{
    Debug.Log("FIUMM");
    _rb.velocity = new Vector2(_rb.velocity.x, pushForce);
}

#endregion
#region OTHER

    public void ArtificialGravity()
    {
        if (_data.onKnockback) return;
        if (_rb.velocity.y < 0) _rb.velocity -= _data.gravity * _data.fallMultiplier * Time.fixedDeltaTime;
    }
    bool OnGround()
    {
        return _data.onGround = Physics2D.OverlapBox(_data.groundPos.position, _data.groundCheckArea, 0, _data.groundLayer);
    }

    void IsFalling()
    {
        if (_rb.velocity.y < 0 && !OnGround())
        {
            _model.ChangeAnimationState("OnAir");
            _data.isFalling = true;
        }
        else _data.isFalling = false;
    }
#endregion
}

