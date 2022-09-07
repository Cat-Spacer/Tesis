using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomMovement : PlayerDatas, IDamageable
{
    public Rigidbody2D _rb;
    private Action _TimeCounterAction;
    private Action _InputsAction;
    [SerializeField] Transform _respawnPoint;

    private void Start()
    {
        gravityForce = gravityForceDefault;

        _rb = GetComponent<Rigidbody2D>();
        coyoteTimeCounter = coyoteTime;
        jumpBufferCounterTime = jumpBufferTime;
        _TimeCounterAction += TimeCounterCoyote;
        _InputsAction = MovementInput;
        _InputsAction += JumpInput;
        _InputsAction += DashInput;
        _InputsAction += Attack;
    }
    private void Update()
    {
        _InputsAction();
        _TimeCounterAction();
        Climb();
    }
    private void FixedUpdate()
    {
        _rb.AddForce(Vector2.down * gravityForce);
        Movement(xMove, 1);
        GroundCheckPos();
        JumpUp(onJumpPressed);
        Dash();
    }
    #region Movement
    private void MovementInput()
    {
        xMove = Input.GetAxis("Horizontal");
    }
    private void Movement(float xDir,float lerpAmount)
    {
        if (dashing) return;
        //anim.SetFloat("Move", xDir);
        if (xDir > 0.1f)
        {
            anim.SetBool("Run", true);
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
            faceDirection = 1;
        }
        else if (xDir < -0.1f)
        {
            anim.SetBool("Run", true);
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
            faceDirection = -1;
        }
        else anim.SetBool("Run", false);
        float targetSpeed = xDir * maxSpeed; 
        float speedDif = targetSpeed - _rb.velocity.x; 

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

        if ((_rb.velocity.x > targetSpeed && targetSpeed > 0.01f) || (_rb.velocity.x < targetSpeed && targetSpeed < -0.01f))
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
        else if (Mathf.Abs(_rb.velocity.x) > 0 && (Mathf.Sign(targetSpeed) != Mathf.Sign(_rb.velocity.x)))
        {
            velPower = turnPower;
        }
        else
        {
            velPower = accelPower;
        }
        #endregion    

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        movement = Mathf.Lerp(_rb.velocity.x, movement, lerpAmount); 

        _rb.AddForce(movement * Vector2.right);

        if (_rb.velocity.y < 0) anim.SetTrigger("Fall");
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
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canJump = false;
        }
        else
        {
            onJumpPressed = false;
            if (jumpUp && !onGround)
            {
                doJumpBuffer = true;
                _TimeCounterAction += TimeCounterJumpbuffer;
            }
        }
    }
    void JumpStop(bool jumpStop)
    {
        if (jumpStop && !onGround)
        {
            _rb.AddForce(Vector2.down * stopJumpForce, ForceMode2D.Impulse);
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
            _rb.velocity = Vector2.zero;
            anim.SetTrigger("Dash");
        }
        if(dashing)
        {
            _rb.velocity = Vector2.right * dashForce * faceDirection;
            _rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            if (Vector2.Distance(transform.position, dashStart) >= dashDistance)
            {
                _dashParticleTrail.Stop();
                dashing = false;
                _rb.velocity *= 0.5f;
                ConstrainsReset();
            }
        }
    }
    IEnumerator DashStop()
    {
        yield return new WaitForSeconds(1);
        dashing = false;
        _rb.velocity *= 0.5f;
        ConstrainsReset();
    }

    public void ForceDashEnd()
    {
        dashing = false;
        _rb.velocity *= 0.5f;
        canDash = true;
        ConstrainsReset();
    }
    #endregion
    #region MiauAttack
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {     
            anim.SetTrigger("Attack");
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
    #region Climb
    void Climb()
    {
        if (_onWall)
        {
            anim.SetBool("OnWall", true);
            if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                _climbParticle.Stop();
                anim.SetBool("Climbing", true);
                _rb.velocity = Vector2.up * _climbSpeed;
            }
            else anim.SetBool("Climbing", false);

            if (Input.GetKey(KeyCode.S))
            {
                anim.SetBool("Climbing", false);
                _rb.velocity = Vector2.down * _climbSpeed;
            }

            if (Input.GetKey(KeyCode.A))
            {
                if (faceDirection == -1)
                {
                    stopClimbing = true;
                    _rb.velocity = new Vector2(_rb.velocity.x, 0);
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (faceDirection == 1)
                {
                    stopClimbing = true;
                    _rb.velocity = new Vector2(_rb.velocity.x, 0);
                }
            }
            _climbParticle.Play();
        }
    }
    #endregion
    void ConstrainsReset()
    {
        _rb.constraints = RigidbodyConstraints2D.None;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
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
            Debug.Log("Onground");
            _fallParticle.Play();
        }

        if (collision.gameObject.layer == _wallLayerNumber)
        {
            _rb.velocity = Vector2.zero;
            _onWall = true;
            _rb.gravityScale = _gravityScale;
            gravityForce = 0.0f;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _wallLayerNumber)
        {
            _onWall = false;
            _onClimb = false;
            _rb.gravityScale = 1.0f;
            gravityForce = gravityForceDefault;
            anim.SetBool("OnWall", false);
            anim.SetBool("Climbing", false);
            _climbParticle.Stop();
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (dashing)
        {
            dashing = false;
            _rb.velocity *= 0.5f;
            ConstrainsReset();
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
        transform.position = new Vector3(_respawnPoint.transform.position.x, _respawnPoint.transform.position.y, 0);
        Debug.Log("Recibi daño");
    }
}

