using System;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine;
using Unity.Netcode;

public enum CharacterType
{
    Cat,
    Hamster,
    Null
}

public class PlayerCharacterMultiplayer : NetworkBehaviour,IPlayerInteract, IDamageable//, IEquatable<PlayerCharacterMultiplayer>, INetworkSerializable
{
    [SerializeField] protected CharacterData _data;
    private CharacterModel _model;
    protected Rigidbody2D _rb;
    private BoxCollider2D coll;
    protected Action _HitAction = delegate {  };
    protected Action _DebuffAction = delegate {  };

    private Material _material;
    private int _dissolveAmount = Shader.PropertyToID("_DissolveAmount");
    private float _dissolveTime = 2f;

    [SerializeField] private CharacterType charType;
    public virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        _data = GetComponent<CharacterData>();
        _model = GetComponent<CharacterModel>();
        _material = GetComponentInChildren<SpriteRenderer>().material;
        //_data.characterPhysicsMat = _rb.sharedMaterial;
        _data.gravity = new Vector2(0, -Physics2D.gravity.y);
        _rb.isKinematic = false;
        _data._fallSpeedYDampingChangeTreshold = MyCameraManager.instance._fallSpeedYDampingChangeThreshold;
    }

    protected virtual void Update()
    {
        if (_rb.velocity.y < _data._fallSpeedYDampingChangeTreshold && !MyCameraManager.instance.IsLerpingYDamping &&
            !MyCameraManager.instance.LerpedFromPlayerFalling)
        {
            MyCameraManager.instance.LerpYDamping(true);
        }

        if (_rb.velocity.y >= 0f && !MyCameraManager.instance.IsLerpingYDamping &&
            MyCameraManager.instance.LerpedFromPlayerFalling)
        {
            MyCameraManager.instance.LerpedFromPlayerFalling = false;
            MyCameraManager.instance.LerpYDamping(false);
        }
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
    public void Movement(bool onInput ,float direction)
    {
        if (direction == 0 || _data.isStun || !_data.canMove)
        {
            _data.isRunning = false;
            if (!_data.isJumping && OnGround())
            {
                _model.ChangeAnimationState("Idle");
            }
            return;
        }

        _model.FaceDirection((int)direction);
        _data.isRunning = true;

        // if (OnGround() && !_data.isJumping)
        // {
        //     var xMove =  _rb.velocity.x + (_data.faceDirection * _data.runAcel * Time.fixedDeltaTime);
        //
        //     if(_data.faceDirection == 1 && _data.maxSpeed > Mathf.Abs(_rb.velocity.x)) xMove = Mathf.Clamp(_rb.velocity.x + xMove, 0, _data.maxSpeed);
        //     else if (_data.faceDirection == -1 && _data.maxSpeed > Mathf.Abs(_rb.velocity.x)) xMove = Mathf.Clamp(_rb.velocity.x + xMove, -_data.maxSpeed, 0);
        //
        //     _rb.velocity = new Vector2(xMove, _rb.velocity.y);
        //     _model.ChangeAnimationState("Run");
        // }
        // else
        // {
        //     var xMove = _rb.velocity.x + (_data.faceDirection * _data.airRunAcel * Time.fixedDeltaTime);
        //
        //     if(_data.faceDirection == 1 && _data.maxSpeed > Mathf.Abs(_rb.velocity.x)) xMove = Mathf.Clamp(_rb.velocity.x + xMove, 0, _data.maxSpeed);
        //     else if (_data.faceDirection == -1 && _data.maxSpeed > Mathf.Abs(_rb.velocity.x)) xMove = Mathf.Clamp(_rb.velocity.x + xMove, -_data.maxSpeed, 0);
        //     
        //     _rb.velocity = new Vector2(xMove, _rb.velocity.y);
        // }
        
        if (OnGround() && !_data.isJumping)
        {
            _model.ChangeAnimationState("Run");
            var xMove = direction * _data.runAcel * Time.fixedDeltaTime;
            // if(_data.faceDirection > 1 && _data.maxSpeed > Mathf.Abs(_rb.velocity.x)) xMove = Mathf.Clamp(_rb.velocity.x + xMove, 0, _data.maxSpeed);
            // else if(_data.faceDirection < 1 && _data.maxSpeed > Mathf.Abs(_rb.velocity.x)) xMove = Mathf.Clamp(_rb.velocity.x + xMove, -_data.maxSpeed, 0);
            _rb.velocity = new Vector2(xMove, _rb.velocity.y);
        }
        else
        {
            _model.ChangeAnimationState("Run");
            var xMove = direction * _data.airRunAcel * Time.fixedDeltaTime;
            // if(_data.faceDirection > 1 && _data.maxSpeed > Mathf.Abs(_rb.velocity.x)) xMove = Mathf.Clamp(_rb.velocity.x + xMove, 0, _data.maxSpeed);
            // else if(_data.faceDirection < 1 && _data.maxSpeed > Mathf.Abs(_rb.velocity.x)) xMove = Mathf.Clamp(_rb.velocity.x + xMove, -_data.maxSpeed, 0);
            _rb.velocity = new Vector2(xMove, _rb.velocity.y);
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
    #endregion
#region JUMP

    public void JumpUp(bool jump)
    {
        //Debug.Log("Try Jump");
        if (_data.isStun || _data.isJumping || !_data.canJump) return;
        if (jump && OnGround())
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _data.jumpForce);
            _data.isJumping = true;
            _data.jumpCounter = 0;
        }

        if (_rb.velocity.y > 0 && _data.isJumping)
        {
            //Debug.Log("Jump");
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
public void Teletransport()
{
    _model.Teletransport();
}
#region CHAR_ACTIONS

    public virtual void Special(){}
    public virtual void Punch(){}
    public void Interact(bool onPress)
    {
        var interact = Physics2D.OverlapBox(transform.position, _data.interactSize, 0, _data.interactMask);
        if (interact == null)
        {
            _data.canvas.InteractEvent(false);
            if (_data._interactObj != null)
            {
                _data._interactObj.ShowInteract(false);
                _data._interactObj = null;
            }
            return;
        }
        _data._interactObj = interact.GetComponent<IInteract>();
        if (_data._interactObj != null)
        {
            _data.canvas.InteractEvent(true);
            _data._interactObj.ShowInteract(true);
            if (onPress)
            {
                _data._interactObj.Interact(gameObject);
            }
            return;
        }
        var item = interact.GetComponent<ItemNetwork>();
        if (item != null)
        {
            _data.canvas.InteractEvent(true);
            if (onPress)
            {
                _data._onHandNetwork = item;
                _data._onHandNetwork.PickUp(this, true);
            }
        }
    }
    
    public virtual void JumpImpulse()
    {
        var otherPlayer = Physics2D.OverlapBox(transform.position, _data.jumpInpulseArea, 0, _data.playerMask);
        if (otherPlayer)
        {
             var playerInteract = otherPlayer.gameObject.GetComponent<IPlayerInteract>();
             if (playerInteract == null) return;
             var player = playerInteract.GetNetworkObject();
             if (player == null) return;
             JumpImpulseRpc(player);
        }
    }

    [Rpc(SendTo.Everyone)]
    void JumpImpulseRpc(NetworkObjectReference player)
    {
        player.TryGet(out NetworkObject playerNetworkObject);
        playerNetworkObject.GetComponent<IPlayerInteract>().GetJumpImpulse(_data.jumpImpulse);
        //playerInteract.GetJumpImpulse(_data.jumpImpulse);
    }
    public void GetJumpImpulse(float pushForce)
    {
        _rb.velocity = new Vector2(_rb.velocity.x, pushForce);
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }

    public ItemNetwork GiveItem(ItemTypeNetwork type)
    {
        if (_data._onHandNetwork == null) return default;
        if (_data._onHandNetwork.Type() == type)
        {
            var item = _data._onHandNetwork;
            item.transform.parent = null;
            _data._onHandNetwork = null;
            return item;
        }
        return null;
    }
    public void PickUp(ItemNetwork item)
    {
        if (item == null) return;
        _data._onHandNetwork = item;
        item.NetworkObject.TrySetParent(_data._inventoryPos.transform);
        //item.transform.position = _data._inventoryPos.transform.position;
    }

    public void DropItem()
    {
        if (_data._onHandNetwork == null) return;
        _data._onHandNetwork.transform.parent = null;
        _data._onHandNetwork.Drop(new Vector2(_model.GetFaceDirection(), _data._dropOffset.y), _data.dropForce);
        _data._onHandNetwork = null;
    }

#endregion
#region OTHER

    public Transform GetInventoryTransform()
    {
        return _data._inventoryPos;
    }
    public void ArtificialGravity()
    {
        if (_data.onKnockback) return;
        if (_rb.velocity.y < 0) _rb.velocity -= _data.gravity * (_data.fallMultiplier * Time.fixedDeltaTime);
    }
    bool OnGround()
    {
        return _data.onGround = Physics2D.OverlapBox(_data.groundPos.position, _data.groundCheckArea, 0, _data.groundLayer);
    }

    void IsFalling()
    {
        if (_rb.velocity.y < 0 && !OnGround())
        {
            //_model.ChangeAnimationState("OnAir");
            _data.isFalling = true;
        }
        else _data.isFalling = false;
    }

    public void StopMovement()
    {
        _rb.velocity = Vector3.zero;
    }
    void Freeze(bool freeze)
    {
        if(freeze) _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        else
        {
            _rb.constraints = RigidbodyConstraints2D.None;
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void CollidersSwitch(bool switcher)
    {
        coll.enabled = switcher;
    }
    public void GetDamage()
    {
        DieRpc();
        Freeze(true);
        CollidersSwitch(false);
        Die();
    }

    [Rpc(SendTo.NotMe)]
    void DieRpc()
    {
        Freeze(true);
        Die();
    }

    IEnumerator Vanish()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _dissolveTime)
        {
            elapsedTime += Time.deltaTime * 1.5f;

            float lerpedDissolve = Mathf.Lerp(0, 1.1f, (elapsedTime / _dissolveTime));

            _material.SetFloat(_dissolveAmount, lerpedDissolve);
            yield return null;
        }
        transform.position = GameManager.Instance.GetRespawnPoint();
        Revive();
    }
    IEnumerator Appear()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _dissolveTime)
        {
            elapsedTime += Time.deltaTime * 1.5f;

            float lerpedDissolve = Mathf.Lerp(1.1f, 0f, (elapsedTime / _dissolveTime));

            _material.SetFloat(_dissolveAmount, lerpedDissolve);
            yield return null;
        }
        _data.canMove = true;
        _data.canJump = true;
        CollidersSwitch(true);
        Freeze(false);
    }
    void Die()
    {
        _data.canMove = false;
        _data.canJump = false;
        StartCoroutine(Vanish()); //RPC seguramente...
    }

    void Revive()
    {
        StartCoroutine(Appear());
    }

    public CharacterType GetCharType(){ return charType;} 

#endregion

    public bool Equals(PlayerCharacterMultiplayer other)
    {
        return true;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        // if (serializer.IsReader)
        // {
        //     var reader = serializer.GetFastBufferReader();
        // }
        // else
        // {
        //     var writer = serializer.GetFastBufferWriter();
        // }
    }
    
}

