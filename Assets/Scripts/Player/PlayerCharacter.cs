using System;
using System.Collections;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IDamageable, IStun
{
    public Inputs input { get; private set; }
    [SerializeField] protected CharacterData _data;
    protected CharacterModel _model;
    protected Rigidbody2D _rb;
    private BoxCollider2D _coll2D;
    protected Action _HitAction = delegate { };
    protected Action _DebuffAction = delegate { };

    private State _oldState;
    public State state;
    public State idleState;
    public State groundState;
    public State airState;
    public State runState;
    public State stunState;
    public State punchState;
    public State specialState;
    public State interactState;


    private Material _material;
    private int _dissolveAmount = Shader.PropertyToID("_DissolveAmount");
    private float _dissolveTime = 2f;

    [SerializeField] private CharacterType charType;

    private bool doorInteracting;
    public virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _coll2D = GetComponent<BoxCollider2D>();
        _data = GetComponent<CharacterData>();
        _model = GetComponent<CharacterModel>();
        _material = GetComponentInChildren<SpriteRenderer>().material;
        input = GetComponent<Inputs>();
        //_data.characterPhysicsMat = _rb.sharedMaterial;
        _data.gravity = new Vector2(0, -Physics2D.gravity.y);
        _rb.isKinematic = false;

        idleState.SetUp(_rb, _model, input, _data, this);
        groundState.SetUp(_rb, _model, input, _data, this);
        airState.SetUp(_rb, _model, input, _data, this);
        runState.SetUp(_rb, _model, input, _data, this);
        stunState.SetUp(_rb, _model, input, _data, this);
        interactState.SetUp(_rb, _model, input, _data, this);
        punchState.SetUp(_rb, _model, input, _data, this);
        specialState.SetUp(_rb, _model, input, _data, this);
        _oldState = idleState;
        state = idleState;
    }

    private void Update()
    {
        if (!doorInteracting)
        {
            return;
        }

        if (state.isComplete)
        {
            state.Exit();
            SelectState();
        }
        else
        {
            state.Do();
        }
    }

    void SelectState()
    {
        if (_data.isStun)
        {
            state = stunState;
        }
        else if(_data.isPunching) state = punchState;
        else if (OnGround())
        {
            if (_data.isInteracting)
            {
                state = interactState;
            }
            else if (_data.onJumpImpulse)
            {
                state = specialState;
            }
            if (_data.canMove && (input.left_Input || input.right_Input) && !_data.isPunching && !_data.isInteracting && !_data.onJumpImpulse && _data.onGround)
            {
                state = runState;
            }
            else if (!input.left_Input && !input.right_Input && !_data.isInteracting && !_data.isPunching && !_data.onJumpImpulse && _data.onGround)
            {
                state = idleState;
            }
        }
        else state = airState;
        
        if (state != _oldState)
        {
            _oldState = state;
            state.Enter();
        }
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
        if (!run || !_data.canMove || _data.isStun || _data.isPunching)
        {
            _data.isRunning = false;
            _model.StopParticle(ParticleType.Run);
            return;
        }
        float xMove = direction;
        if (OnGround())
        {
            xMove *= _data.runAcel;
            _model.PlayParticle(ParticleType.Run);
        }
        else
        {
            xMove *= _data.airRunAcel;
            _model.StopParticle(ParticleType.Run);
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
            _rb.velocity = new Vector2(decelerate, _rb.velocity.y);
        }
    }
    #endregion
    #region JUMP

    public void JumpUp(bool jump)
    {
        if (!jump || _data.isStun || _data.isJumping || !_data.canJump) return;
        if (OnGround())
        {
            if (charType == CharacterType.Cat) SoundManager.instance.Play(SoundsTypes.CatJump, gameObject);
            else SoundManager.instance.Play(SoundsTypes.HamsterJump, gameObject);
            _rb.velocity = new Vector2(_rb.velocity.x, _data.jumpForce);
            _data.isJumping = true;
            _data.jumpCounter = 0;
            _model.PlayParticle(ParticleType.Jump);
        }
        else if (_data.canDoubleJump && charType == CharacterType.Cat)
        {
            SoundManager.instance.Play(SoundsTypes.CatJump, gameObject);
            _data.canDoubleJump = false;
            _rb.velocity = new Vector2(_rb.velocity.x, _data.doubleJumpForce);
            _data.isJumping = true;
            _data.jumpCounter = 0;
            _model.PlayParticle(ParticleType.Jump);
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
        float stopForce = Mathf.Lerp(_rb.velocity.y, 0, _data.jumpStopForce);
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

    public virtual void Special() { }

    public virtual void Punch()
    {
        if (!_data.canPunch) return;

        _data.isPunching = true;
        _data.canPunch = false;
        _data.canMove = false;
        StartCoroutine(PunchCd());
        if (charType == CharacterType.Cat) SoundManager.instance.Play(SoundsTypes.CatAttack, gameObject);
        else SoundManager.instance.Play(SoundsTypes.HamsterAttack, gameObject);
        _model.PlayParticle(ParticleType.Attack);
        var obj = Physics2D.OverlapCircle(_data.attackPoint.position, _data.attackRange.x, _data.attackableLayer);
        if (obj)
        {
            var body = obj.gameObject.GetComponent<Rigidbody2D>();
            if (body != null)
            {
                Vector2 direction = new Vector2(_model.GetFaceDirection(), .8f);
                body.AddForce(direction * _data.punchForce);
                Debug.Log("Attack player");
                if (LiveCamera.instance != null && LiveCamera.instance.IsOnAir())
                {
                    if (EventManager.Instance != null) EventManager.Instance.Trigger(EventType.OnChangePeace, -1);
                }
                if (EventManager.Instance != null) EventManager.Instance.Trigger(EventType.OnUpdateEgoPoints, charType, 1);
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
        _data.isPunching = false;
        _data.canPunch = true;
        _data.canMove = true;
    }
    public void Interact(bool onPress)
    {
        if (onPress)
        {
            StartCoroutine(InteractCd());
            _data.isInteracting = true;
        }
        if (onPress && _data._onHand != null)
        {
            _data._onHand.Drop(new Vector2(_data.faceDirection, 1), 5);
            DropItem();
            return;
        }
        var interact = Physics2D.OverlapBox(transform.position, _data.interactSize, 0, _data.interactMask);
        if (interact == null) //ShowInteract
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
        if (_data._interactObj != null) //Interact
        {
            _data.canvas.InteractEvent(true);
            _data._interactObj.ShowInteract(true);
            if (onPress)
            {
                _data._interactObj.Interact(gameObject);
            }
        }
    }

    IEnumerator InteractCd()
    {
        yield return new WaitForSecondsRealtime(1f);
        _data.isInteracting = false;
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
        _data._onHand.transform.position = transform.position;
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

    private bool wasOnGround;
    bool OnGround()
    {
        _data.onGround = Physics2D.OverlapBox(
            _data.groundPos.position,
            _data.groundCheckArea,
            0,
            _data.groundLayer
        );

        // Si no está en el suelo, actualiza el estado y retorna false.
        if (!_data.onGround)
        {
            wasOnGround = false; // Marca que no está en el suelo.
            return false;
        }

        // Si está en el suelo pero no lo estaba en el cuadro anterior, activa las partículas.
        if (!wasOnGround)
        {
            _model.PlayParticle(ParticleType.Land);
            _data.canDoubleJump = true;
        }

        // Marca que ahora está en el suelo.
        wasOnGround = true;

        return true;
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
        if (freeze) _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        else
        {
            _rb.constraints = RigidbodyConstraints2D.None;
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void CollidersSwitch(bool switcher)
    {
        _coll2D.enabled = switcher;
    }
    public void GetDamage()
    {
        _model.PlayParticle(ParticleType.Damage);
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
        if (charType == CharacterType.Cat) transform.position = GameManager.Instance.GetCatRespawnPoint();
        else transform.position = GameManager.Instance.GetHamsterRespawnPoint();
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
        _model.PlayParticle(ParticleType.Die);
        SoundManager.instance.Play(SoundsTypes.Death, gameObject);
        _data.canMove = false;
        _data.canJump = false;
        StartCoroutine(Vanish());
    }

    void Revive()
    {
        _model.PlayParticle(ParticleType.Revive);
        state = idleState;
        SoundManager.instance.Play(SoundsTypes.Death, gameObject);
        StartCoroutine(Appear());
    }
    public void ReceiveInputs(SO_Inputs newInput)
    {
        _model.ChangeAnimationState("ExitDoor");
        _data.canvas.SetPlayerInteractKeys(newInput);
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
        state = idleState;
        _data.canMove = true;
        doorInteracting = true;
        if(charType == CharacterType.Cat)
        {
            if (LiveCamera.instance != null) LiveCamera.instance.StartLiveCamera(true);
            EventManager.Instance.Trigger(EventType.StartTimer);
        }
    }
    public CharacterType GetCharType() { return charType; }

    #endregion


}

