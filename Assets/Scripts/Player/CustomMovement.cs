using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomMovement : PlayerDatas, IDamageable, ITrap
{
    private PlayerInput _playerInput;
    public Rigidbody2D rb;
    private Action _TimeCounterAction, _PlayerActions, _Inputs;
    [SerializeField] private Transform _respawnPoint;
    [SerializeField] private TrailRenderer _dashTrail;
    public Climb _climbScript;
    private BoxCollider2D _collider;
    public static bool isJumping = false;
    private GameObject _currentTrap;
    public float jumpForceClimbHeight = 400, jumpForceClimbLatitud = 150;
    private EnergyPower _energyPowerScript;
    private IInteract _interactObj;
    private Action _MovementState, _DashState;
    private float _baseJump;
    public GameObject boosterFeedBack;
    private Vector2 _startScale, _smallerScale, _startGroundCheckSize, _smallerGroundCheckSize;

    public float modiffyIceJumpY = 1, modiffyIceJumpX = 1;

    [SerializeField] private LayerMask _ignoredPhysics;

    float xMove;
    private void Awake()
    {
        _startScale = transform.localScale;
        _smallerScale = _startScale * 0.7f;
        _startGroundCheckSize = groundCheckSize;
        _smallerGroundCheckSize = _startGroundCheckSize * 0.7f;
        boosterFeedBack.gameObject.SetActive(false);
        _collider = GetComponent<BoxCollider2D>();
        _playerInput = GetComponent<PlayerInput>();
        _energyPowerScript = GetComponent<EnergyPower>();
        rb = GetComponent<Rigidbody2D>();

        _climbScript = new Climb();
        _climbScript.SetClimb(_playerInput, _collider, rb, transform, _upSpeed, _downSpeed,
            _distanceToRope, _climbLayerMask, _impulseDirectionExitForce, _impulseExitRopeForce, anim, this, gravityForceDefault, _energyPowerScript,
             groundLayer);
    }

    private void Start()
    {
        _baseJump = jumpForce;
        gravityForce = gravityForceDefault;
        defaultMaxSpeed = maxSpeed;
        coyoteTimeCounter = coyoteTime;
        jumpBufferCounterTime = jumpBufferTime;
        canAttack = true;
        _TimeCounterAction += TimeCounterCoyote;
        _Inputs = Inputs;
        _PlayerActions = delegate { };
        _MovementState = delegate { };
        _DashState = delegate { };
        GameManager.Instance.SetNewCheckPoint(transform);
        //_DashState = delegate { };
    }

    public void ForceStopMovement()
    {
        _MovementState = StopMovement;
    }

    private void Update()
    {
        _Inputs();
        Interact();
        _climbScript.UpdateClimb();
        Debug.DrawRay(transform.position, transform.right * _distanceToRope, Color.red);
    }

    private void FixedUpdate()
    {
        _DashState();
        _climbScript.FixedUpdateClimb();
        rb.AddForce(Vector2.down * gravityForce);
        _MovementState();
        GroundCheckPos();
        JumpUp(onJumpInput);
        JumpStop(onJumpInputReleased);
        Dash();
        //  if (jumpClimb && !PlayerInput.left_Input && !PlayerInput.right_Input)
        // Movement( 1);
    }

    private void Inputs()
    {
        if (PlayerInput.jumpInputDown) onJumpInput = true;
        if (PlayerInput.jumpInputUp) onJumpInputReleased = true;
        if (PlayerInput.dashInput) onDashInput = true;
        if (PlayerInput.up_Input) w_Input = true;
        else w_Input = false;
        if (PlayerInput.left_Input) a_Input = true;
        else a_Input = false;
        if (PlayerInput.down_Input) s_Imput = true;
        else s_Imput = false;
        if (PlayerInput.right_Input) d_Input = true;
        else d_Input = false;
        if (_playerInput.attackImput) attackInput = true;
        else attackInput = false;
        if (PlayerInput.interactionInput) interactionInput = true;
        else interactionInput = false;

        if (PlayerInput.right_Input && !Climb.isHorizontal && !isDashing && !Climb.MoveTowardsBool && PlayerInput.canRightMove)
            _MovementState = RightMovement;

        if (PlayerInput.left_Input && !Climb.isHorizontal && !isDashing && !Climb.MoveTowardsBool && PlayerInput.canLeftMove)
            _MovementState = LeftMovement;

        if ((PlayerInput.right_Input_UpKey || PlayerInput.left_Input_UpKey) && !Climb.isHorizontal && !isDashing && !Climb.MoveTowardsBool)
            _MovementState = StopMovement;
    }

    #region Interact
    void Interact()
    {
        var interact = Physics2D.OverlapBox(transform.position, _interactSize, 0, _interactMask);
        if (interact == null)
        {
            _playerCanvas.InteractEvent(false);
            if (_interactObj != null)
            {
                _interactObj.ShowInteract(false);
                _interactObj = null;
            }
            return;
        }
        _interactObj = interact.GetComponent<IInteract>();
        if (_interactObj == null) return;
        else
        {

            _playerCanvas.InteractEvent(true);
            _interactObj.ShowInteract(true);
        }
        if (PlayerInput.interactionInput)
        {
            _interactObj.Interact();
        }
    }
    #endregion
    #region Movement
    bool hasPlayedMovement = false;

    void RightMovement()
    {
        float targetSpeed;
        running = true;
        if (onGround && !isJumping)
        {
            if (!onClimb && rb.velocity.x > .1f)
            {
                ChangeAnimationState(Player_Run);
                _runParticle.Play();
            }
            else
            {
                _runParticle.Stop();
                ChangeAnimationState(Player_Idle);
            }
        }
        else
        {
            _runParticle.Stop();
        }

        transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        faceDirection = 1;
        if (!hasPlayedMovement)
        {
            SoundManager.instance.Play(SoundManager.Types.Steps);
            hasPlayedMovement = true;
        }

        xMove = faceDirection * runAccel * Time.deltaTime;
        xMove = Mathf.Clamp(rb.velocity.x + xMove, 0, maxSpeed);;
        rb.velocity = new Vector2(xMove, rb.velocity.y);

        //targetSpeed = faceDirection * maxSpeed;
        //rb.velocity = new Vector2(targetSpeed, rb.velocity.y);
        //Debug.Log($"<Color=magenta>The rigidbody velocity is: {rb.velocity}</color>");
    }

    void LeftMovement()
    {
        float targetSpeed;
        running = true;
        if (onGround && !isJumping)
        {
            if (!onClimb && rb.velocity.x < -.1f)
            {
                ChangeAnimationState(Player_Run);
                _runParticle.Play();
            }
            else
            {
                _runParticle.Stop();
                ChangeAnimationState(Player_Idle);
            }
        }
        else
        {
            _runParticle.Stop();
        }


        transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        faceDirection = -1;
        if (!hasPlayedMovement)
        {
            SoundManager.instance.Play(SoundManager.Types.Steps);
            hasPlayedMovement = true;
        }
        xMove = faceDirection * runAccel * Time.deltaTime;
        xMove = Mathf.Clamp(rb.velocity.x + xMove, -maxSpeed, 0);
        rb.velocity = new Vector2(xMove, rb.velocity.y);
        //targetSpeed = faceDirection * maxSpeed;
        //rb.velocity = new Vector2(targetSpeed, rb.velocity.y);
        //  Debug.Log($"<Color=magenta>The rigidbody velocity is: {rb.velocity}</color>");
    }

    public void StopMovement()
    {
        //ChangeAnimationState(Player_Idle);
        running = false;
        _runParticle.Stop();
        if (hasPlayedMovement)
        {
            SoundManager.instance.Pause(SoundManager.Types.Steps);
            hasPlayedMovement = false;
        }
        if (!OnIce) rb.velocity = new Vector2(0, rb.velocity.y);
        // Debug.Log($"<Color=magenta>The rigidbody velocity is: {rb.velocity}</color>");
        _MovementState = delegate { };
    }

    public void StopMovementOnIce()
    {
        //ChangeAnimationState(Player_Idle);
        //anim.SetBool("Run", false);
        _runParticle.Stop();
        if (hasPlayedMovement)
        {
            SoundManager.instance.Pause(SoundManager.Types.Steps);
            hasPlayedMovement = false;
        }
        _MovementState = delegate { };
    }
    #endregion
    #region JUMP
    public void BoostJump(float force_arg)
    {
        jumpForce = force_arg;
        boosterFeedBack.gameObject.SetActive(true);
        transform.localScale = _smallerScale;
        groundCheckSize = _smallerGroundCheckSize;
    }

    public void RestartJumpValue()
    {
        jumpForce = _baseJump;
        boosterFeedBack.gameObject.SetActive(false);
        transform.localScale = _startScale;
        groundCheckSize = _startGroundCheckSize;
    }
    public void JumpUp(bool jumpUp) //Saltar
    {
        onJumpInput = false;
        if (!jumpUp || isJumping || !canJump) return;

        if (coyoteTimeCounter > 0f || onGround)
        {
            _climbScript.EndClimb();
            SoundManager.instance.Play(SoundManager.Types.CatJump);
            _runParticle.Stop();
            _jumpParticle.Play();
            jumping = true;
            isJumping = true;
            doJumpBuffer = false;
            canJump = false;
            ChangeAnimationState(Player_Jump);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

    }
    void JumpStop(bool jumpStop)
    {
        if (jumpStop && !onGround && !Climb.isClimbing)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Min(rb.velocity.y - stopJumpForce, -8));
        }
        else
        {
            onJumpInputReleased = false;
        }
    }
    private bool jumpClimb = false;

    public void JumpClimb2()
    {
        Debug.Log("jumping");

        rb.gravityScale = 1.0f;
        gravityForce = gravityForceDefault;
        rb.velocity = Vector2.zero;
        //_jumpParticle.Play();
        //    jumping = true;
        // isJumping = true;
        //anim.SetTrigger("Jump");
        ChangeAnimationState(Player_Jump);
        //canJump = false;
        if (!dead) ConstrainsReset();

        float angle = 45;
        angle *= Mathf.Deg2Rad;
        float xComponent = Mathf.Cos(angle) * jumpForceClimbLatitud;
        float zComponent = Mathf.Sin(angle) * jumpForceClimbHeight;
        Vector3 forceApplied = new Vector3(xComponent * faceDirection, zComponent, 0);

        jumpClimb = true;
        rb.AddForce(forceApplied);
        _climbScript._ClimbState = _climbScript.EndClimbJump;
        //  canJump = false;
        //_climbScript._ClimbState = _climbScript.EndClimbJump;
        //Climb.isClimbing = false;
        //  StartCoroutine(CoroutineWaitForEndJump(0.1f));
    }

    public IEnumerator CoroutineWaitForEndJump(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _climbScript._ClimbState = _climbScript.EndClimbJump;

    }
    #endregion

    public bool dashClimb = false;


    void TimeCounterCoyote()
    {
        if (onGround || coyoteTime <= 0) coyoteTimeCounter = coyoteTime;
        else coyoteTimeCounter -= 1 * Time.deltaTime;
    }

    void TimeCounterJumpbuffer()
    {
        jumpBufferCounterTime -= Time.deltaTime;
        if (jumpBufferCounterTime >= 0 && onGround && doJumpBuffer && jumping)
        {
            //JumpUp(true);
            doJumpBuffer = false;
            jumpBufferCounterTime = jumpBufferTime;
            _TimeCounterAction -= TimeCounterJumpbuffer;
        }
        else if (jumpBufferCounterTime <= 0)
        {
            doJumpBuffer = false;
            jumpBufferCounterTime = jumpBufferTime;
            _TimeCounterAction -= TimeCounterJumpbuffer;
        }
    }

    #region Dash
    public void StartDashFeedBack()
    {
        SoundManager.instance.Play(SoundManager.Types.CatDash);
        _dashParticleExplotion.Play();
        _dashParticleTrail.Play();
        _dashTrail.gameObject.SetActive(true);
    }

    public void EndDashFeedBack()
    {
        _dashParticleExplotion.Stop();
        _dashParticleTrail.Stop();
        _dashTrail.gameObject.SetActive(false);
    }

    void Dash()
    {
        if (Climb.isClimbing)
        {
            onDashInput = false;
            //Debug.Log("CANT DASH CAUSE IS CLIMBING");
            return;
        }

        if (dashClimb)
        {
            //  Debug.Log("CANT DASH CAUSE CLIMB DASH");
            onDashInput = false;
            return;
        }

        if (onDashInput && canDash && _energyPowerScript.EnergyDrain(10))
        {
            rb.isKinematic = false;
            rb.constraints = ~RigidbodyConstraints2D.FreezeAll;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            _DashState += DashStop;

            canDash = false;
            onDashInput = false;
            isDashing = true;
            dashStart = transform.position;
            _dashParticleExplotion.Play();
            _dashParticleTrail.Play();
            rb.velocity = Vector2.zero;
            SoundManager.instance.Play(SoundManager.Types.CatDash);
            _dashTrail.gameObject.SetActive(true);

            if (_climbScript.InSight(_climbLayerMask))
            {
                if (faceDirection == 1)
                    transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
                else if (faceDirection == -1)
                    transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.z * -1, transform.rotation.z);

                faceDirection = -faceDirection;
            }
        }
        else onDashInput = false;

        if (isDashing)
        {
            rb.velocity = Vector2.right * dashForce * faceDirection;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (Vector2.Distance(transform.position, dashStart) >= dashDistance)
            {
                if (!dead) ConstrainsReset();
                _dashParticleTrail.Stop();
                isDashing = false;
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0;
                _MovementState = StopMovement;
                EndDashFeedBack();
            }
        }
    }
    void DashStop()
    {
        var coll = Physics2D.OverlapBox(transform.position + offsetDashCheck, dashCheck, 0, groundLayer);
        if (coll)
        {
            if (!dead) ConstrainsReset();
            _dashParticleTrail.Stop();
            isDashing = false;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
            _DashState = delegate { };
            _MovementState = StopMovement;
            EndDashFeedBack();
        }
    }
    public void DashClimb()
    {

        dashClimb = true;
        Vector2 last = transform.position;
        onDashInput = false;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.gravityScale = 1.0f;
        gravityForce = gravityForceDefault;

        _dashParticleExplotion.Play();
        _dashParticleTrail.Play();
        _dashTrail.gameObject.SetActive(true);

        if (_climbScript.InSight(_climbLayerMask))
        {
            if (faceDirection == -1)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.z * -1, transform.rotation.z);
            }
            if (faceDirection == 1)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
            }
            faceDirection = -faceDirection;
        }
        else
        {
            Debug.Log("---NOT FACING WALL---");
        }

        Debug.Log("---DASH---");

        dashStart = transform.position;

        anim.SetTrigger("Dash");
        SoundManager.instance.Play(SoundManager.Types.CatDash);

        rb.velocity = Vector2.right * dashForceClimb * faceDirection;

        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // rb.isKinematic = true;

        _climbScript._ClimbState = EndDashClimb;
    }

    public void EndDashClimb()
    {
        if (_climbScript.InSight(_climbLayerMask))
        {
            rb.velocity *= 0;
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            _dashParticleTrail.Stop();
            EndDashFeedBack();
            _climbScript._ClimbState = _climbScript.Freeze;
            Debug.Log("freeze dash");
        }

        if (Vector2.Distance(transform.position, dashStart) >= (dashDistance))
        {
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            //ConstrainsReset();
            _dashParticleTrail.Stop();
            _dashTrail.gameObject.SetActive(false);
            rb.velocity *= 0;
            EndDashFeedBack();
            Debug.Log("SUTIL END");
            _climbScript._ClimbState = _climbScript.SutilEnd;
        }
    }
    public void ForceDashEnd()
    {
        _DashState = delegate { };
        Climb.isClimbing = false;

        if (_climbScript.InSight(_climbLayerMask) && _energyPowerScript.EnergyDrain(0.05f))
        {
            _climbScript.StartClimbWithFreeze();
        }
        // Debug.Log("ForceDashEnd dash end");
        isDashing = false;
        canDash = true;

        rb.velocity = Vector2.zero;
        _dashParticleTrail.Stop();
        if (!dead) ConstrainsReset();
        EndDashFeedBack();
    }
    void StartDash()
    {
        CustomMovement.isDashing = true;
        rb.velocity = new Vector2(0, 0);
        rb.angularVelocity = 0;
        rb.constraints = ~RigidbodyConstraints2D.FreezeAll;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        // _ClimbState = _customMovement.DashClimb;
        dashStart = transform.position;
        //isHorizontal = false;
        _DashState = MoveTowardsDash;
    }

    Vector3 rotationVector;

    void MoveTowardsDash()
    {
        Climb.MoveTowardsBool = true;

        rb.velocity = new Vector2(0, 0);
        rb.angularVelocity = 0;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (_climbScript.InSight(_climbLayerMask))
        {
            if (CustomMovement.faceDirection == -1)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
                rotationVector.y = 0;
            }
            if (CustomMovement.faceDirection == 1)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
                rotationVector.y = 180;
            }
            CustomMovement.faceDirection = -CustomMovement.faceDirection;
        }

        _DashState = ForceDash;
        StartDashFeedBack();
    }

    public void ForceDash()
    {
        Debug.Log("dashing");
        rb.velocity = Vector2.right * 45 * CustomMovement.faceDirection;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Climb.MoveTowardsBool = false;
        _DashState = EndDash;
    }

    public void EndDash()
    {
        Debug.Log("END DASH");

        /*  if (InSight(_climbLayerMask))
          {
              _customMovement.EndDashFeedBack();
              Debug.Log("start climb");
              StartClimbingState();
              rb.constraints = ~RigidbodyConstraints2D.FreezePositionY;
              CustomMovement.isDashing = false;
              _ClimbState = ClimbActionVertical;

          }
          else*/
        if (CustomMovement.collisionObstacle || Vector2.Distance(transform.position, dashStart) >= 3)
        {
            _DashState = delegate { };
            EndDashFeedBack();
            Debug.Log("end climb");
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
            CustomMovement.isDashing = false;
            _DashState = delegate { };
            //_ClimbState = EndClimb;
        }
    }
    #endregion

    #region MiauAttack
    void Attack()
    {
        if (attackInput && canAttack && _energyPowerScript.EnergyDrain(10))
        {
            anim.SetTrigger("Attack");
            SoundManager.instance.Play(SoundManager.Types.CatAttack);
            var attackParticle = Instantiate(_attackParticle);
            attackParticle.gameObject.transform.right = attackPoint.right;
            attackParticle.gameObject.transform.position = attackPoint.position;
            Destroy(attackParticle.gameObject, 1);

            canAttack = false;
            if (onGround)
            {
                maxSpeed *= 0.05f;
            }
            StartCoroutine(AttackCd());

            var coll = Physics2D.OverlapBox(attackPoint.position, attackRange, 1, damageable);
            if (coll == null) return;
            var obj = coll.gameObject.GetComponent<IDamageable>();
            if (obj == null) return;

            obj.GetDamage(1);
        }
    }

    IEnumerator AttackCd()
    {
        yield return new WaitForSeconds(attackCd);
        maxSpeed = defaultMaxSpeed;
        canAttack = true;
    }
    #endregion

    public void ConstrainsReset()
    {
        if (!dead) rb.constraints = constraints2D;
    }

    void GroundCheckPos()
    {
        groundColl = Physics2D.OverlapBox
            (groundCheckPos.transform.position, groundCheckSize, 0, groundLayer);

        if (ForestFlower.onFlower == true)
        {
            canHorizontalClimb = true;
            isJumping = false;
            onGround = true;
            jumpClimb = false;
            canJump = true;
            canDash = true;
        }

        if (groundColl != null) //On ground
        {
            canHorizontalClimb = true;
            isJumping = false;
            onGround = true;
            jumpClimb = false;
            canJump = true;
            canDash = true;
            if (!running && !Climb.isClimbing && !isJumping)
            {
                ChangeAnimationState(Player_Idle);
            }
        }
        else //Not on ground
        {
            onGround = false;
            if (!_climbScript.onClimb && rb.velocity.y < 0)
            {
                ChangeAnimationState(Player_OnAir);
                _runParticle.Stop();
            }
        }
    }

    public static bool collisionObstacle = false;
    [SerializeField] private LayerMask _obstacleLayers;

    public static bool canHorizontalClimb;

    private bool icyWall;
    public void ClimbState()
    {
        jumping = false;
        isJumping = false;
        doJumpBuffer = false;
        canJump = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (collision.gameObject.layer == 24)
        {
            Debug.Log($"Ignored Collision");
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
        }*/

        if (collision.gameObject.layer == 6 && !isJumping)
        {
            _fallParticle.Play();
        }

        if ((_obstacleLayers.value & (1 << collision.gameObject.layer)) > 0)
        {
            collisionObstacle = true;
            ForceDashEnd();
        }

        if (collision.gameObject.layer == 21 && !icyWall && isJumping)
        {

            if (faceDirection == -1)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.z * -1, transform.rotation.z);
            }
            if (faceDirection == 1)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
            }
            faceDirection = -faceDirection;

            float angle = 45;
            angle *= Mathf.Deg2Rad;
            float xComponent = Mathf.Cos(angle) * jumpForceClimbLatitud * modiffyIceJumpX;
            float zComponent = Mathf.Sin(angle) * jumpForceClimbHeight * modiffyIceJumpY;
            Vector2 forceApplied = new Vector2(xComponent * faceDirection, zComponent);

            rb.AddForce(forceApplied);
            icyWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((_obstacleLayers.value & (1 << collision.gameObject.layer)) > 0)
        {
            collisionObstacle = false;
        }
    }

    private bool OnIce = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 17 && canHorizontalClimb)
        {
            Climb.isClimbing = true;
            Climb.isHorizontal = true;
            ForceStopMovement();
            _climbScript.StartClimbingState();
            //_climbScript._ClimbState = _climbScript.ClimbActionHorizontal;
            //canHorizontalClimb = false;
        }

        if (collision.gameObject.layer == 20)
        {
            OnIce = true;
        }

    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 20)
        {
            OnIce = false;
            Physics2D.IgnoreLayerCollision(6, 7, false);
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheckPos.transform.position, groundCheckSize);
            Gizmos.DrawWireCube(attackPoint.transform.position, attackRange);
            Gizmos.DrawWireCube(transform.position, _interactSize);
            Gizmos.DrawWireCube(transform.position + offsetDashCheck, dashCheck);
        }
    }

    public void Stuck()
    {
        _Inputs = delegate { };
    }

    public void GetDamage(float dmg)
    {
        rb.simulated = false;
        dead = true;
        _Inputs = delegate { };
        rb.velocity = Vector2.zero;
        GameManager.Instance.PlayerDeath();
        SoundManager.instance.Play(SoundManager.Types.CatDamage);
        _playerCanvas.TrapEvent(false, 0);
        _playerCanvas.InteractEvent(false);
        ForceDashEnd();
        isJumping = false;
        onGround = true;
        canJump = true;
        canDash = true;
        _climbScript._ClimbState = _climbScript.EndClimb;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        _MovementState = StopMovement;
    }
    public void ResetPlayer()
    {
        _Inputs = Inputs;
        dead = false;
        rb.simulated = true;
    }
    public IEnumerator CoroutineWaitForRestartDistance(Vector2 last)
    {
        while (_climbScript.InSightLast(last, _distanceToRope))
        {
            yield return Climb._distanceToRope = 0;
        }
        Climb._distanceToRope = _distanceToRope;
    }

    private void TrapInputs()
    {
        if (PlayerInput.trapInput)
        {
            var trap = _currentTrap.GetComponent<ILiberate>();
            if (trap == null) return;
            trap.TryLiberate();
        }
    }
    public void Trap(bool trapState, float life, GameObject enemy)
    {
        if (trapState)
        {
            //anim.SetBool("TrapByPlant", true);
            _PlayerActions = Liberate;
            _Inputs = TrapInputs;
            _currentTrap = enemy;
            _playerCanvas.TrapEvent(trapState, life);
        }
        else
        {
            //anim.SetBool("TrapByPlant", false);
            _PlayerActions = delegate { };
            _Inputs = Inputs;
            _playerCanvas.TrapEvent(trapState, life);
        }
    }
    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        anim.Play(newState);
        currentState = newState;
    }
    public void CancelMovement()
    {
        onJumpInput = false;
        onJumpInputReleased = false;
        rb.velocity = Vector3.zero;
        Debug.Log($"<Color=magenta>The rigidbody velocity is: {rb.velocity}</color>");
    }
    public void Liberate() { }

    private void OnDisable()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void OnEnable()
    {
        ConstrainsReset();
    }
}