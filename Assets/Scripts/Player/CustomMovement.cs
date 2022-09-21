using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomMovement : PlayerDatas, IDamageable
{
    public Rigidbody2D rb;
    private Action _TimeCounterAction;
    private Action _InputsAction;
    private Action _ClimbAction;
    [SerializeField] Transform _respawnPoint;
    [SerializeField] TrailRenderer _dashTrail;

    private void Start()
    {
        gravityForce = gravityForceDefault;

        rb = GetComponent<Rigidbody2D>();
        coyoteTimeCounter = coyoteTime;
        jumpBufferCounterTime = jumpBufferTime;

        _TimeCounterAction += TimeCounterCoyote;

        _InputsAction = MovementInput;
        _InputsAction += JumpInput;
        _InputsAction += DashInput;
        _InputsAction += Attack;
        _InputsAction += ClimbInput;

        _ClimbAction = ClimbUp;
        _ClimbAction += ClimbDown;
        _ClimbAction += ClimbStaticRight;
        _ClimbAction += ClimbStaticLeft;
    }
    private void Update()
    {
        _InputsAction();
        _TimeCounterAction();
    }
    private void FixedUpdate()
    {
        rb.AddForce(Vector2.down * gravityForce);
        Movement(xMove, 1);
        GroundCheckPos();
        JumpUp(onJumpPressed);
        Dash();
        _ClimbAction();
    }
    #region Movement
    private void MovementInput()
    {
        xMove = Input.GetAxis("Horizontal");
    }

    bool hasPlayedMovement = false;
    private void Movement(float xDir,float lerpAmount)
    {
        if (dashing) return;


        //anim.SetFloat("Move", xDir);
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

        if (rb.velocity.y < 0)
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
    void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onJumpPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.Space) /*|| Input.GetKeyUp(KeyCode.W)*/ || Input.GetKeyUp(KeyCode.UpArrow))
        {
            onJumpReleased = true;
        }
    }
    void JumpUp(bool jumpUp) //Saltar
    {
        if (jumpUp && canJump && (coyoteTimeCounter > 0f || onGround))
        {
            _jumpParticle.Play();
            jumping = true;
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
        //Si estoy en la pared
        if (jumpUp && canJump && _onWall)
        {
            //_jumpParticle.Play();
            jumping = true;
            anim.SetTrigger("Jump");
            Debug.Log("Salto");
            _onWall = false;
            _onClimb = false;
            rb.gravityScale = 1.0f;
            gravityForce = gravityForceDefault;
            rb.velocity = Vector2.zero;
            if (faceDirection == 1) //Salto a la izq
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                rb.AddForce(-Vector2.right * jumpForce * .5f, ForceMode2D.Impulse);
            }
            else //Salto a la der
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                rb.AddForce(Vector2.right * jumpForce * .8f, ForceMode2D.Impulse);
            }
            canJump = false;
        }
        onJumpPressed = false;
    }
    void JumpStop(bool jumpStop)
    {
        if (jumpStop && !onGround)
        {
            rb.AddForce(Vector2.down * stopJumpForce, ForceMode2D.Impulse);
        }
        else if (onGround) onJumpReleased = false;

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
    #endregion
    #region Dash

    void DashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            doDash = true;
        }
    }
    void Dash()
    {
        if (doDash && canDash)
        {
            canDash = false;
            doDash = false;
            dashing = true;
            dashStart = transform.position;
            _dashParticleExplotion.Play();
            _dashParticleTrail.Play();
            rb.velocity = Vector2.zero;
            anim.SetTrigger("Dash");
            SoundManager.instance.Play(SoundManager.Types.CatDash);
            _dashTrail.gameObject.SetActive(true);
        }
        if(dashing)
        {
            rb.velocity = Vector2.right * dashForce * faceDirection;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (Vector2.Distance(transform.position, dashStart) >= dashDistance)
            {
                ConstrainsReset();
                _dashParticleTrail.Stop();
                dashing = false;
                rb.velocity *= 0.5f;
                StartCoroutine(ExampleCoroutine());
            }
            if (rb.velocity == Vector2.zero)
            {
                ForceDashEnd();
            }
        }
    }
    IEnumerator DashStop()
    {
        yield return new WaitForSeconds(1);
        dashing = false;
        rb.velocity *= 0.5f;
        ConstrainsReset();
        StartCoroutine(ExampleCoroutine());
    }

    public void ForceDashEnd()
    {
        dashing = false;
        //rb.velocity *= 0.5f;
        rb.velocity = Vector2.zero;
        canDash = true;
        _dashParticleTrail.Stop();
        ConstrainsReset();
        StartCoroutine(ExampleCoroutine());
    }

    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        _dashTrail.gameObject.SetActive(false);
    }


    #endregion
    #region Climb

    bool hasPlayedClimb = false;
    void ClimbInput()
    {
        if (_onWall && (Input.GetKey(KeyCode.W) || onGround == false))
        {
            _canClimb = true;
        }
        if (!_canClimb) return;
        if (Input.GetKeyDown(KeyCode.W) && !Input.GetKeyDown(KeyCode.A) && !Input.GetKeyDown(KeyCode.D))
        {
            _doClimbStaticLeft = false;
            _doClimbStaticRight = false;
            _doClimbDown = false;
            _doClimbUp = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _doClimbStaticLeft = false;
            _doClimbStaticRight = false;
            _doClimbUp = false;
            _doClimbDown = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            _doClimbUp = false;
            _doClimbDown = false;
            _doClimbStaticRight = false;
            _doClimbStaticLeft = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _doClimbUp = false;
            _doClimbDown = false;
            _doClimbStaticLeft = false;
            _doClimbStaticRight = true;
        }
        _climbParticle.Play();

        if (rb.velocity.y < -0.1)
        {
            anim.SetBool("OnWall", true);
            if (!hasPlayedClimb)
            {
                SoundManager.instance.Play(SoundManager.Types.Climb);
                hasPlayedClimb = true;
            }

        }
        if (rb.velocity.y > 0.1)
        {
            if (!hasPlayedClimb)
            {
                SoundManager.instance.Play(SoundManager.Types.Climb);
                hasPlayedClimb = true;
            }
        }
        if (rb.velocity.y == 0)
        {
            hasPlayedClimb = false;
            SoundManager.instance.Pause(SoundManager.Types.Climb);
        }
    }
    void ClimbUp()
    {
        if (_doClimbUp)
        {
            anim.SetBool("OnWall", true);

            _climbParticle.Stop();
            anim.SetBool("Climbing", true);
            rb.velocity = Vector2.up * _climbSpeed;
            ConstrainsReset();
        }
        else anim.SetBool("Climbing", false);
    }
    void ClimbDown()
    {
        if (_doClimbDown)
        {
            anim.SetBool("OnWall", true);
            anim.SetBool("Climbing", false);
            rb.velocity = Vector2.down * _climbSpeed * 0.8f;
            ConstrainsReset();
        }
    }
    void ClimbStaticLeft()
    {
        if (_doClimbStaticLeft)
        {
            anim.SetBool("OnWall", true);
            if (faceDirection == -1)
            {
                stopClimbing = true;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.velocity = new Vector2(rb.velocity.y, 0);
                rb.angularVelocity = 0;
                rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            }
            else
            {
                ConstrainsReset();
            }
        }       
    }
    void ClimbStaticRight()
    {
        if (_doClimbStaticRight) 
        {
            anim.SetBool("OnWall", true);

            if (faceDirection == 1)
            {
                stopClimbing = true;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.velocity = new Vector2(rb.velocity.y, 0);
                rb.angularVelocity = 0;
                rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            }
            else
            {
                ConstrainsReset();
            }
        }
    } 
    #endregion
    #region MiauAttack
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {     
            anim.SetTrigger("Attack");
            SoundManager.instance.Play(SoundManager.Types.CatAttack);
            var attackParticle = Instantiate(_attackParticle);
            attackParticle.gameObject.transform.right = attackPoint.right;
            attackParticle.gameObject.transform.position = attackPoint.position;
            Destroy(attackParticle.gameObject, 1);
            var coll = Physics2D.OverlapBox(attackPoint.position, attackRange, 1, damageable);
            if (coll == null) return;
            var obj = coll.gameObject.GetComponent<IDamageable>();
            if (obj == null) return;
            Debug.Log("attack");
            obj.GetDamage(1);
        }
    }
    #endregion
    public void ConstrainsReset()
    {
        rb.constraints = constraints2D;
    }
    void GroundCheckPos()
    {
        groundColl = Physics2D.OverlapBox
            (groundCheckPos.transform.position, groundCheckSize, 0, groundLayer);
        if (groundColl != null) //On ground
        {
            onGround = true;
            canJump = true;
            canDash = true;
            anim.SetBool("OnGround", true);
        }
        else //Not on ground
        {
            onGround = false;
            anim.SetBool("OnGround", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {     
        if (onGround == true && collision.gameObject.layer == 6)
        {
            _fallParticle.Play();
        }

        if (collision.gameObject.layer == _wallLayerNumber) //OnWall
        {
            hasPlayedClimb = false;
            _climbParticle.Play();
            canJump = true;
            rb.velocity = Vector2.zero;
            _onWall = true;
            rb.gravityScale = _gravityScale;
            gravityForce = 0.0f;
            ForceDashEnd();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _wallLayerNumber)
        {
            hasPlayedClimb = false;
            ConstrainsReset();

            _onWall = false;
            _onClimb = false;
            _canClimb = false;
            _doClimbUp = false;
            _doClimbDown = false;
            _doClimbStaticLeft = false;
            _doClimbStaticRight = false;

            rb.gravityScale = 1.0f;
            gravityForce = gravityForceDefault;
            anim.SetBool("OnWall", false);
            anim.SetBool("Climbing", false);
            _climbParticle.Stop();
            SoundManager.instance.Pause(SoundManager.Types.Climb);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (dashing && !onGround)
        {
            ForceDashEnd();
            /*dashing = false;
            rb.velocity *= 0.5f;
            ConstrainsReset();*/
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPos.transform.position, groundCheckSize);
        Gizmos.DrawWireCube(attackPoint.transform.position, attackRange);
    }
    public void GetDamage(float dmg)
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        GameManager.Instance.PlayerDeath();
        SoundManager.instance.Play(SoundManager.Types.CatDamash);
    }
}

