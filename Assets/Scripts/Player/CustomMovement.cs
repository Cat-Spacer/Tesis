using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomMovement : PlayerDatas, IDamageable
{
    private PlayerInput playerInput;
    public Rigidbody2D rb;
    private Action _TimeCounterAction;
    private Action _ClimbAction;
    private Action _Inputs;
    [SerializeField] Transform _respawnPoint;
    [SerializeField] TrailRenderer _dashTrail;
    public Climb _climbScript;
    BoxCollider2D _collider;
    public static bool isJumping = false;

    public float jumpForceClimbHeight = 400;
    public float jumpForceClimbLatitud = 150;

    EnergyPower _energyPowerScript;


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
        /*_ClimbAction = ClimbInput;
        _ClimbAction += ClimbUp;
        _ClimbAction += ClimbDown;
        _ClimbAction += ClimbStaticRight;
        _ClimbAction += ClimbStaticLeft;*/
    }
    private void Update()
    {
        _Inputs();
        Attack();
        _TimeCounterAction();
        // _ClimbAction();
        _climbScript.UpdateClimb();

        Debug.DrawRay(transform.position, transform.right * _distanceToRope, Color.red);


        //Debug.Log(PlayerInput.dashImput);
    }
    private void FixedUpdate()
    {
        _climbScript.FixedUpdateClimb();
        rb.AddForce(Vector2.down * gravityForce);
        Movement(xMove, 1);
        GroundCheckPos();
        JumpUp(onJumpInput);
        Dash();

        if (jumpClimb && !PlayerInput.a_Imput && !PlayerInput.d_Imput)
            Movement(1 * faceDirection, 1);

    }
    private void Inputs()
    {    
        xMove = playerInput.xAxis;
        if (playerInput.jumpImput) onJumpInput = true;
       if (PlayerInput.dashImput) onDashInput = true;
        if (PlayerInput.w_Imput) w_Imput = true;
        else w_Imput = false;
        if (PlayerInput.a_Imput) a_Imput = true;
        else a_Imput = false;
        if (PlayerInput.s_Imput) s_Imput = true;
        else s_Imput = false;
        if (PlayerInput.d_Imput) d_Imput = true;
        else d_Imput = false;
        if (playerInput.attackImput) attackImput = true;
        else attackImput = false;
    }
    #region Movement
    bool hasPlayedMovement = false;
    private void Movement(float xDir,float lerpAmount)
    {
        if (Climb.isHorizontal) return;
        if (dashing && Climb.MoveTowardsBool) return;
        if (xDir > 0.1f)
        {
            anim.SetBool("Run", true);
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
            faceDirection = 1;
            if (!hasPlayedMovement)
            {
                SoundManager.instance.Play(SoundManager.Types.Steps);
                hasPlayedMovement = true;
            }
        }
        else if (xDir < -0.1f)
        {
            anim.SetBool("Run", true);
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
            faceDirection = -1;
            if (!hasPlayedMovement)
            {
                SoundManager.instance.Play(SoundManager.Types.Steps);
                hasPlayedMovement = true;
            }
        }
        else
        {
            anim.SetBool("Run", false);
            if (hasPlayedMovement)
            {
                SoundManager.instance.Pause(SoundManager.Types.Steps);
                hasPlayedMovement = false;
            }
        }
        float targetSpeed = xDir * maxSpeed; 
        float speedDif = targetSpeed - rb.velocity.x; 

        #region Acceleration Rate
        float accelRate;

        if(Mathf.Abs(targetSpeed) > 0.01f)
        {
            accelRate = runAccel;
        }
        else
        {
            accelRate = runDeccel;
        }

        if (onGround) //GroundMovement
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccel : runDeccel;
        else //AirMovement
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccel * airRunAccel : runDeccel * airRunDeccel;

        if ((rb.velocity.x > targetSpeed && targetSpeed > 0.01f) || (rb.velocity.x < targetSpeed && targetSpeed < -0.01f))
        {
            accelRate = 0;
        }
        #endregion

        #region Velocity Power
        float velPower;
        if (Mathf.Abs(targetSpeed) < 0.01f)
        {
            velPower = stopPower;
        }
        else if (Mathf.Abs(rb.velocity.x) > 0 && (Mathf.Sign(targetSpeed) != Mathf.Sign(rb.velocity.x)))
        {
            velPower = turnPower;
        }
        else
        {
            velPower = accelPower;
        }
        #endregion    

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        movement = Mathf.Lerp(rb.velocity.x, movement, lerpAmount);
       
        rb.AddForce(movement * Vector2.right);
        //Debug.Log(rb.);

        if (rb.velocity.y < 0 && !Climb.isClimbing)
        {
            if (hasPlayedMovement)
            {
                SoundManager.instance.Pause(SoundManager.Types.Steps);
                hasPlayedMovement = false;
            }
            anim.SetTrigger("Fall");
        }
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
        rb.gravityScale = 1.0f;
        gravityForce = gravityForceDefault;
        rb.velocity = Vector2.zero;
        //_jumpParticle.Play();
    //    jumping = true;
       // isJumping = true;
        anim.SetTrigger("Jump");

        //canJump = false;
        if (!dead) ConstrainsReset();
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

        float angle = 45;
        angle *= Mathf.Deg2Rad;
        float xComponent = Mathf.Cos(angle) * jumpForceClimbLatitud;
        float zComponent = Mathf.Sin(angle) * jumpForceClimbHeight;
        Vector3 forceApplied = new Vector3(xComponent * faceDirection, zComponent, 0);
        
        jumpClimb = true;
        rb.AddForce(forceApplied);

        //  canJump = false;
        //_climbScript._ClimbState = _climbScript.EndClimbJump;
        //Climb.isClimbing = false;
        StartCoroutine(CoroutineWaitForEndJump(0.1f));
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
    void Dash()
    {
        if (Climb.isClimbing)
        {
            onDashInput = false;
            Debug.Log("CANT DASH CAUSE IS CLIMBING");
            return;
        }
        
        if (dashClimb)
        {
            Debug.Log("CANT DASH CAUSE CLIMB DASH");
            onDashInput = false;
            return;
        }
      
        if (onDashInput && canDash &&_energyPowerScript.EnergyDrain(10))
        {   rb.isKinematic = false;
            rb.constraints = ~RigidbodyConstraints2D.FreezeAll;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            canDash = false;
            onDashInput = false;
            dashing = true;
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
        if (dashing)
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
                dashing = false;
                rb.velocity *= 0.5f;
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
        if (dashing)
        {
            Debug.Log("call ForceDashEnd");
            ForceDashEnd();
        }
    }

    public void ForceDashEnd()
    {
        if (_climbScript.InSight(_climbLayerMask) && _energyPowerScript.EnergyDrain(0.05f))
        {
            _climbScript.StartClimbWithFreeze();
        }
            Debug.Log("ForceDashEnd dash end");
        dashing = false;
        canDash = true;
        Climb.isClimbing = false;
        rb.velocity = Vector2.zero;
        _dashParticleTrail.Stop();
        if (!dead) ConstrainsReset();
        StartCoroutine(ExampleCoroutine());
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
        if (attackImput && canAttack && _energyPowerScript.EnergyDrain(10))
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
            isJumping = false;
            onGround = true;
            jumpClimb = false;
            canJump = true;
            canDash = true;
        }

        if (groundColl != null) //On ground
        {
   
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

        if ((groundColl != null|| ForestFlower.onFlower == true) && transform.rotation.z ==0)
        {
            canHorizontalClimb = true;
        }
    }

    public static bool collisionObstacle = false;
    [SerializeField] LayerMask _obstacleLayers;

    public static bool canHorizontalClimb;

    private void OnCollisionEnter2D(Collision2D collision)
    {     
        if (onGround == true && collision.gameObject.layer == 6)
        {
            _fallParticle.Play();
           // isJumping = false;
        }

        if ((_obstacleLayers.value & (1 << collision.gameObject.layer)) > 0)
        { 
            collisionObstacle = true;
            Debug.Log("call ForceDashEnd");
            ForceDashEnd();
        }

      /*  if (collision.gameObject.layer == _wallLayerNumber) //OnWall
        {
            hasPlayedClimb = false;
            _climbParticle.Play();
            canJump = true;
            rb.velocity = Vector2.zero;
            _onWall = true;
            rb.gravityScale = _gravityScale;
            gravityForce = 0.0f;
            ForceDashEnd();
        }*/
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((_obstacleLayers.value & (1 << collision.gameObject.layer)) > 0)
        {
            collisionObstacle = false;
            Debug.Log("end collision with obstacle");
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
      /*  if (dashing && !onGround)
        {
            Debug.Log("call ForceDashEnd");
            ForceDashEnd();
        }*/

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 17 && canHorizontalClimb)
        {
            _climbScript.StartClimbingState();
            Climb.isClimbing = true;
            _climbScript._ClimbState = _climbScript.ClimbActionHorizontal;
            canHorizontalClimb = false;
            //_climbScript._vector = transform.right;
        }
        /*if (collision.gameObject.layer == 9 && !canHorizontalClimb)
        {
            _climbScript.StartClimbingState();
            _climbScript._ClimbState = _climbScript.ClimbActionVertical;
            canHorizontalClimb = false;
            //_climbScript._vector = transform.right;
        }*/
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPos.transform.position, groundCheckSize);
        Gizmos.DrawWireCube(attackPoint.transform.position, attackRange);

    }
    public void GetDamage(float dmg)
    {
        rb.simulated = false;
        dead = true;
        _Inputs = delegate { };
        rb.velocity = Vector2.zero;
        GameManager.Instance.PlayerDeath();
        SoundManager.instance.Play(SoundManager.Types.CatDamage);
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


}