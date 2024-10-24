using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerCharacter : MonoBehaviour, IDamageable, IStun
{
    protected Inputs input;
    [SerializeField] protected CharacterData _data;
    protected CharacterModel _model;
    protected Rigidbody2D _rb;
    private BoxCollider2D coll;
    protected Action _HitAction = delegate {  };
    protected Action _DebuffAction = delegate {  };

    public State state;
    public State idleState;
    public State groundState;
    public State airState;
    public State runState;
    public State stunState;
    

    private Material _material;
    private int _dissolveAmount = Shader.PropertyToID("_DissolveAmount");
    private float _dissolveTime = 2f;

    [SerializeField] private CharacterType charType;

    private bool doorInteracting;
    public virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        _data = GetComponent<CharacterData>();
        _model = GetComponent<CharacterModel>();
        _material = GetComponentInChildren<SpriteRenderer>().material;
        input = GetComponent<Inputs>();
        //_data.characterPhysicsMat = _rb.sharedMaterial;
        _data.gravity = new Vector2(0, -Physics2D.gravity.y);
        _rb.isKinematic = false;

        idleState.SetUp(_rb, _model, input, _data);
        groundState.SetUp(_rb, _model, input, _data);
        airState.SetUp(_rb, _model, input, _data);
        runState.SetUp(_rb, _model, input, _data);
        stunState.SetUp(_rb, _model, input, _data);

        state = idleState;
    }
    
    private void Update()
    {
        if (!doorInteracting) return;
        if (state.isComplete)
        {
            SelectState();   
        }
        state.Do();
    }

    void SelectState()
    {
        if (_data.isStun)
        {
            state = stunState;
        }
        else
        {
            if (OnGround())
            {
                if (!input.left_Input && !input.right_Input)
                {
                    state = idleState;
                }
                else if(_data.canMove)
                {
                    state = runState;
                }
            }
            else
            {
                state = airState;
            }
        }
        state.Enter(); 
    }
    
    protected virtual void FixedUpdate()
    {
        ArtificialGravity();
        GroundFriction();
        _HitAction();
        IsFalling();
        
    }
#region MOVEMENT
    public void Movement(bool run, int direction)
    {
        if (!run || !_data.canMove || _data.isStun)
        {
            _data.isRunning = false;
            return;
        }
        float xMove = direction;
        if (OnGround())
        {
            xMove *= _data.runAcel;
        }
        else
        {
            xMove *= _data.airRunAcel;
        }
        _data.isRunning = true;
        _rb.velocity = new Vector2(xMove, _rb.velocity.y);
        _model.FaceDirection(direction);
    }
    void GroundFriction()
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
        if (OnGround())
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _data.jumpForce);
            _data.isJumping = true;
            _data.jumpCounter = 0;
        }
        else if(_data.canDoubleJump && charType == CharacterType.Cat)
        {
            _data.canDoubleJump = false;
            _rb.velocity = new Vector2(_rb.velocity.x, _data.doubleJumpForce);
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

#region STUN

    public void Stun(float stunForce)
    {
        _data.isStun = true;
        StartCoroutine(StunTimer(stunForce));
    }
    IEnumerator StunTimer(float stunForce)
    {
        yield return new WaitForSecondsRealtime(stunForce);
        _data.isStun = false;
    }

#endregion
    public void Teletransport()
    {
        _model.Teletransport();
    }
#region CHAR_ACTIONS

    public virtual void Special(){}

    public virtual void Punch()
    {
        if (!_data.canPunch) return;
        var obj = Physics2D.OverlapCircle(_data.attackPoint.position, _data.attackRange.x, _data.attackableLayer);
        if (obj)
        {
            var body = obj.gameObject.GetComponent<Rigidbody2D>();
            if (body != null)
            {
                Vector2 direction =  new Vector2(_model.GetFaceDirection(), .8f);
                body.AddForce(direction * _data.punchForce);
                _data.canPunch = false;
                EventManager.Instance.Trigger(EventType.OnChangePeace, -1);
                StartCoroutine(PunchCd());
            }
            var player = obj.gameObject.GetComponent<IStun>();
            if (player != null)
            {
                player.Stun(_data.stunForce);
            }
        }
    }

    IEnumerator PunchCd()
    {
        yield return new WaitForSecondsRealtime(_data.punchCd);
        _data.canPunch = true;
    }
    public void Interact(bool onPress)
    {
        if (onPress && _data._onHand != null)
        {
            _data._onHand.Drop(new Vector2(_data.faceDirection, 1), 5);
            DropItem();
            return;
        }
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
        }
    }
    // public virtual void JumpImpulse()
    // {
    //     var otherPlayer = Physics2D.OverlapBox(transform.position, _data.jumpInpulseArea, 0, _data.playerMask);
    //     if (otherPlayer)
    //     {
    //          var playerInteract = otherPlayer.gameObject.GetComponent<IPlayerInteract>();
    //          if (playerInteract == null) return;
    //          var player = playerInteract.GetNetworkObject();
    //          if (player == null) return;
    //          JumpImpulseRpc(player);
    //     }
    // }
    public void GetJumpImpulse(float pushForce)
    {
        _rb.velocity = new Vector2(_rb.velocity.x, pushForce);
    }

    public void PickUp(Item item)
    {
        if (item == null) return;
        _data._onHand = item;
    }

    public Item GiveItem()
    {
        var itemToReturn = _data._onHand;
        _data._onHand = null;
        return itemToReturn;
    }
    public void DropItem()
    {
        if (_data._onHand == null) return;
        _data._onHand.Drop(new Vector2(_model.GetFaceDirection(), _data._dropOffset.y), _data.dropForce);
        _data._onHand = null;
    }

#endregion
#region OTHER

    public Transform GetInventoryTransform()
    {
        return _data._inventoryPos;
    } 
    void ArtificialGravity()
    {
        if (_data.onKnockback) return;
        if (_rb.velocity.y < 0) _rb.velocity -= _data.gravity * (_data.fallMultiplier * Time.fixedDeltaTime);
    }
    bool OnGround()
    {
        _data.onGround = Physics2D.OverlapBox(_data.groundPos.position, _data.groundCheckArea, 0, _data.groundLayer);
        if (_data.onGround)
        {
            _data.canDoubleJump = true;
            return true;
        }
        return false;
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
        Freeze(true);
        CollidersSwitch(false);
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
    public void ReceiveInputs(SO_Inputs newInput)
    {
        _model.ChangeAnimationState("ExitDoor");
        input.SetInput(newInput);
        _data.canMove = false;
    }

    public void EnterDoor()
    {
        _data.canMove = false;
        doorInteracting = false;
    }

    public void EnterDoorAnimation()
    {
        _model.ChangeAnimationState("EnterDoor");
    }
    public void ExitDoor()
    {
        _model.ChangeAnimationState("ExitDoor");
        StartCoroutine(ExitDoorWaitMovement());
    }

    IEnumerator ExitDoorWaitMovement()
    {
        yield return new WaitForSecondsRealtime(1);
        _data.canMove = true;
        doorInteracting = true;
    }
    public CharacterType GetCharType(){ return charType;} 

#endregion


}

