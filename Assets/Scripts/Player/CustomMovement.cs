using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomMovement : PlayerDatas, IDamageable, ITrap
{
    private PlayerInput playerInput;
    public Rigidbody2D rb;
    private Action _TimeCounterAction;
    private Action _PlayerActions;
    private Action _Inputs;
    [SerializeField] Transform _respawnPoint;
    [SerializeField] TrailRenderer _dashTrail;
    public Climb _climbScript;
    BoxCollider2D _collider;
    public static bool isJumping = false;
    private GameObject currentTrap;
    public float jumpForceClimbHeight = 400;
    public float jumpForceClimbLatitud = 150;
    EnergyPower _energyPowerScript;
    private IInteract interactObj;
    private Action _MovementState;
    private Action _DashState;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        playerInput = GetComponent<PlayerInput>();
        _energyPowerScript = GetComponent<EnergyPower>();
        rb = GetComponent<Rigidbody2D>();

        _climbScript = new Climb();
        _climbScript.SetClimb(playerInput, _collider, rb, transform, _upSpeed, _downSpeed,
            _distanceToRope, _climbLayerMask, _impulseDirectionExitForce, _impulseExitRopeForce, anim, this, gravityForceDefault, _energyPowerScript,
             groundLayer);

    }
    private void Start()
    {
        gravityForce = gravityForceDefault;
        defaultMaxSpeed = maxSpeed;
        coyoteTimeCounter = coyoteTime;
        jumpBufferCounterTime = jumpBufferTime;
        canAttack = true;
        _TimeCounterAction += TimeCounterCoyote;
        _Inputs = Inputs;
        _PlayerActions = delegate { };
        _MovementState = delegate { };
        //_DashState = delegate { };
    }
    private void Update()
    {
       // Movement(1);
        _Inputs();
        _TimeCounterAction();
        Attack();
        Interact();
        _PlayerActions = delegate { };
        _climbScript.UpdateClimb();
        Debug.DrawRay(transform.position, transform.right * _distanceToRope, Color.red);


        //if (PlayerInput.dashInput)
          //  _DashState = StartDash;

    }
    private void FixedUpdate()
    {
        //_DashState();
        _climbScript.FixedUpdateClimb();
        rb.AddForce(Vector2.down * gravityForce);
        _MovementState();
        GroundCheckPos();
        JumpUp(onJumpInput);
        Dash();        
        if (jumpClimb && !PlayerInput.left_Input && !PlayerInput.right_Input)
            Movement( 1);

    }
    private void Inputs()
    {    
        //xMove = playerInput.xAxis;
        if (playerInput.jumpInput) onJumpInput = true;
        if (PlayerInput.dashInput) onDashInput = true;
        if (PlayerInput.up_Input) w_Input = true;
        else w_Input = false;
        if (PlayerInput.left_Input) a_Input = true;
        else a_Input = false;
        if (PlayerInput.down_Input) s_Imput = true;
        else s_Imput = false;
        if (PlayerInput.right_Input) d_Input = true;
        else d_Input = false;
        if (playerInput.attackImput) attackInput = true;
        else attackInput = false;
        if (PlayerInput.interactionInput) interactionInput = true;
        else interactionInput = false;


        if (PlayerInput.right_Input && !Climb.isHorizontal && !isDashing && !Climb.MoveTowardsBool)
            _MovementState = RightMovement;

        if (PlayerInput.left_Input && !Climb.isHorizontal && !isDashing && !Climb.MoveTowardsBool)
            _MovementState = LeftMovement;

        if (PlayerInput.right_Input_UpKey || PlayerInput.left_Input_UpKey && !Climb.isHorizontal && !isDashing && !Climb.MoveTowardsBool)
            _MovementState = StopMovement;
    }

    #region Interact
    void Interact()
    {
        var interact = Physics2D.OverlapBox(transform.position, _interactSize, 0, _interactMask);
        if (interact == null)
        {
            _playerCanvas.InteractEvent(false);
            if (interactObj != null)
            {
                interactObj.ShowInteract(false);
                interactObj = null;
            }           
            return;
        }
        interactObj = interact.GetComponent<IInteract>();
        if (interactObj == null) return;
        else
        {

            _playerCanvas.InteractEvent(true);
            interactObj.ShowInteract(true);
        }        
        if (PlayerInput.interactionInput)
        {
            interactObj.Interact();
        }
    }
    #endregion
    #region Movement
    bool hasPlayedMovement = false;

    void RightMovement()
    {

        float targetSpeed;
        float velPower;

        anim.SetBool("Run", true);
        transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        faceDirection = 1;
        velPower = accelPower;
        if (!hasPlayedMovement)
        {
            SoundManager.instance.Play(SoundManager.Types.Steps);
            hasPlayedMovement = true;
        }
        targetSpeed = faceDirection * maxSpeed;
        rb.velocity = new Vector2(targetSpeed, rb.velocity.y);
    }

    void LeftMovement()
    {

        float targetSpeed;
        float velPower;

        anim.SetBool("Run", true);
        transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        faceDirection = -1;
        velPower = accelPower;
        if (!hasPlayedMovement)
        {
            SoundManager.instance.Play(SoundManager.Types.Steps);
            hasPlayedMovement = true;
        }
        targetSpeed = faceDirection * maxSpeed;
        rb.velocity = new Vector2(targetSpeed, rb.velocity.y);
    }

    void StopMovement()
    {
        anim.SetBool("Run", false);
        if (hasPlayedMovement)
        {
            SoundManager.instance.Pause(SoundManager.Types.Steps);
            hasPlayedMovement = false;
        }
        rb.velocity = new Vector2(0, rb.velocity.y);
        _MovementState = delegate { };
    }

    private void Movement(float lerpAmount)
    {
  
        /*if (rb.velocity.y < 0 && !Climb.isClimbing)
        {
            if (hasPlayedMovement)
            {
                SoundManager.instance.Pause(SoundManager.Types.Steps);
                hasPlayedMovement = false;
            }
            anim.SetTrigger("Fall");
        }*/
    }
    #endregion
    #region JUMP
    void JumpUp(bool jumpUp) //Saltar
    {
        if (Climb.isClimbing)
        {
            onJumpInput = false;
            return;
        }
           
        /* if (Climb.isClimbing)
         {
             canJump = true;
             onGround = true;
         }*/

        if (jumpUp && canJump && (coyoteTimeCounter > 0f || onGround))
        {
            SoundManager.instance.Play(SoundManager.Types.CatJump);
            _jumpParticle.Play();
            jumping = true;
            isJumping = true;
            anim.SetTrigger("Jump");
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canJump = false;
            return;
        }
        else
        {
            if (jumpUp && !onGround)
            {
                doJumpBuffer = true;
                _TimeCounterAction += TimeCounterJumpbuffer;
            }
        }


        if (jumpUp && Climb.isClimbing && (!_climbScript.InSight(_climbLayerMask) || _climbScript.InSight(_climbLayerMask)))
        {
           /* jumping = true;
            anim.SetTrigger("Jump");
            Debug.Log("SaltoEnPared");
            rb.gravityScale = 1.0f;
            gravityForce = gravityForceDefault;
            rb.velocity = Vector2.zero;
            canJump = false;
            ConstrainsReset();
            if (_climbScript.InSight())
            {

                if (faceDirection == 1)
                    transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
                else if (faceDirection == -1)
                    transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.z * -1, transform.rotation.z);

                faceDirection = -faceDirection;
            }

            if (faceDirection == 1) //Salto a la izq
            {
               
                rb.AddForce(-Vector2.right * jumpForceClimbLatitud, ForceMode2D.Impulse);
                rb.AddForce(Vector2.up * jumpForceClimbHeight, ForceMode2D.Impulse);
            }
            else //Salto a la der
            {
                rb.AddForce(Vector2.right * jumpForceClimbLatitud, ForceMode2D.Impulse);
                rb.AddForce(Vector2.up * jumpForceClimbHeight, ForceMode2D.Impulse);
                //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            */

        }
        //Si estoy en la pared
        if (jumpUp && canJump /*&& _onWall*/)
        {
            
          /*  _onWall = false;
            _onClimb = false;*/
           
          /*  _onWall = false;
            _onClimb = false;
            _canClimb = false;
            _doClimbUp = false;
            _doClimbDown = false;
            _doClimbStaticLeft = false;
            _doClimbStaticRight = false;*/

          /*  if (faceDirection == 1) //Salto a la izq
            {
                rb.AddForce(-Vector2.right * jumpForce * .5f, ForceMode2D.Impulse);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            else //Salto a la der
            {
                rb.AddForce(Vector2.right * _wallJumpForceX, ForceMode2D.Impulse);
                rb.AddForce(Vector2.up * _wallJumpForceY, ForceMode2D.Impulse);
                //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }*/
            
        }
        onJumpInput = false;
    }

    bool jumpClimb = false;
    public void JumpClimb2()
    {
        Debug.Log("jumping");

        rb.gravityScale = 1.0f;
        gravityForce = gravityForceDefault;
        rb.velocity = Vector2.zero;
        //_jumpParticle.Play();
    //    jumping = true;
       // isJumping = true;
        anim.SetTrigger("Jump");

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
            StartCoroutine(ExampleCoroutine());
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
            StartCoroutine(ExampleCoroutine());
            Debug.Log("SUTIL END");
            _climbScript._ClimbState = _climbScript.SutilEnd;
        }
    }
    IEnumerator JumpReturnControll()
    {
        yield return new WaitForSeconds(.5f);

    }
    void JumpStop(bool jumpStop)
    {
        if (jumpStop && !onGround)
        {
            rb.AddForce(Vector2.down * stopJumpForce, ForceMode2D.Impulse);
        }
        else if (onGround) onJumpInputReleased = false;

    }
    void TimeCounterCoyote()
    {
        if (onGround || coyoteTime <= 0) coyoteTimeCounter = coyoteTime;
        else coyoteTimeCounter -= 1*Time.deltaTime;
    }
    void TimeCounterJumpbuffer()
    {
        jumpBufferCounterTime -= Time.deltaTime;
        if (jumpBufferCounterTime >= 0 && onGround && doJumpBuffer)
        {
            JumpUp(true);
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
        anim.SetTrigger("Dash");
        _dashTrail.gameObject.SetActive(true);
    }
    public void EndDashFeedBack()
    {
        _dashParticleTrail.Stop();
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

    
      
        if (onDashInput && canDash &&_energyPowerScript.EnergyDrain(10))
        {   rb.isKinematic = false;
            rb.constraints = ~RigidbodyConstraints2D.FreezeAll;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            canDash = false;
            onDashInput = false;
            isDashing = true;
            dashStart = transform.position;
            _dashParticleExplotion.Play();
            _dashParticleTrail.Play();
            rb.velocity = Vector2.zero;
            anim.SetTrigger("Dash");
            SoundManager.instance.Play(SoundManager.Types.CatDash);
            _dashTrail.gameObject.SetActive(true);
            StartCoroutine(DashStop());

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

            if (_climbScript.InSight(_climbLayerMask))
            {
                _climbScript._ClimbState = _climbScript.Freeze;
            }

            if (Vector2.Distance(transform.position, dashStart) >= dashDistance)
            {
                if (!dead) ConstrainsReset();
                _dashParticleTrail.Stop();
                isDashing = false;
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0;
                // rb.velocity *= 0.5f;
                Debug.Log("end dash");
                StartCoroutine(ExampleCoroutine());
            }
            if (rb.velocity == Vector2.zero)
            {
                Debug.Log("call ForceDashEnd");
                ForceDashEnd();
            }
        }
    }
    IEnumerator DashStop()
    {
        yield return new WaitForSeconds(1.5f);
        if (isDashing)
        {
            Debug.Log("call ForceDashEnd");
            ForceDashEnd();
        }
    }

    public void ForceDashEnd()
    {
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
        StartCoroutine(ExampleCoroutine());
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
        else*/ if (CustomMovement.collisionObstacle || Vector2.Distance(transform.position, dashStart) >= 3)
        {
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
    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        _dashTrail.gameObject.SetActive(false);
    }
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
            anim.SetBool("OnGround", true);
        }
        else //Not on ground
        {
            onGround = false;
            anim.SetBool("OnGround", false);
        }

     /*   else if (transform.rotation.z == 0)
        {
            Debug.Log("TRANSFORM IS 0");
        }*/
    }

    public static bool collisionObstacle = false;
    [SerializeField] LayerMask _obstacleLayers;

    public static bool canHorizontalClimb;

    private void OnCollisionEnter2D(Collision2D collision)
    {     
        if (onGround == true && collision.gameObject.layer == 6)
        {
            _fallParticle.Play();
        }

        if ((_obstacleLayers.value & (1 << collision.gameObject.layer)) > 0)
        { 
            collisionObstacle = true;
            ForceDashEnd();
        }


    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((_obstacleLayers.value & (1 << collision.gameObject.layer)) > 0)
        {
            collisionObstacle = false;
            //Debug.Log("end collision with obstacle");
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 17 && canHorizontalClimb)
        {
            Debug.Log("SOGA");
            _climbScript.StartClimbingState();
            Climb.isClimbing = true;
            _climbScript._ClimbState = _climbScript.ClimbActionHorizontal;
            canHorizontalClimb = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheckPos.transform.position, groundCheckSize);
            Gizmos.DrawWireCube(attackPoint.transform.position, attackRange);
            Gizmos.DrawWireCube(transform.position, _interactSize);
        }
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
        Debug.Log("call ForceDashEnd");
        ForceDashEnd();
        isJumping = false;
        onGround = true;
        canJump = true;
        canDash = true;
        _climbScript._ClimbState = _climbScript.EndClimb;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        anim.SetBool("Death1", true);
    }
    public void ResetPlayer()
    {
        anim.SetBool("Death1", false);
        anim.SetBool("TrapByPlant", false);
        _Inputs = Inputs;
        dead = false;
        rb.simulated = true;
    }
    public IEnumerator CoroutineWaitForRestartDistance(Vector2 last)
    {
        while (_climbScript.InSightLast(last, _distanceToRope))
        {
            Debug.Log("as last");
            yield return Climb._distanceToRope = 0;
        }

        Climb._distanceToRope = _distanceToRope;
       
        Debug.Log("done");
    }

    private void TrapInputs()
    {
        if (PlayerInput.trapInput)
        {
            var trap = currentTrap.GetComponent<ILiberate>();
            if (trap == null) return;
            Debug.Log("Liberar");
            trap.TryLiberate();
        }
    }
    public void Trap(bool trapState, float life, GameObject enemy)
    {
        if (trapState)
        {
            anim.SetBool("TrapByPlant", true);
            _PlayerActions = Liberate;
            _Inputs = TrapInputs;
            currentTrap = enemy;
            _playerCanvas.TrapEvent(trapState, life);
        }
        else
        {
            anim.SetBool("TrapByPlant", false);
            _PlayerActions = delegate { };
            _Inputs = Inputs;
            _playerCanvas.TrapEvent(trapState, life);
        }
    }

    public void Liberate()
    {

    }

    private void OnDisable()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void OnEnable()
    {
        ConstrainsReset();
    }
}