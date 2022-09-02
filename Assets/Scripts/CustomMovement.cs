using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomMovement : PlayerDatas 
{
    //[SerializeField] PlayerDatas _data;
    [SerializeField] SpriteRenderer _sr;
    Rigidbody2D _rb;
    private Action _TimeCounterAction;
    private Action _InputsAction;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        coyoteTimeCounter = coyoteTime;
        jumpBufferCounterTime = jumpBufferTime;
        _TimeCounterAction += TimeCounterCoyote;
        _InputsAction = MovementInput;
        _InputsAction += JumpInput;
        _InputsAction += DashInput;
    }
    private void Update()
    {
        _InputsAction();
        _TimeCounterAction();
    }
    private void FixedUpdate()
    {
        _rb.AddForce(Vector2.down * gravityForce);
        Movement(xMove, 1);
        GroundCheckPos();
        JumpUp(onJumpPressed);
        Dash();
        //JumpStop(data.onJumpReleased);
    }
    #region Movement
    private void MovementInput()
    {
        xMove = Input.GetAxis("Horizontal");
    }
    private void Movement(float xDir,float lerpAmount)
    {
        if (xDir > 0.1f)
        {
            _sr.flipX = false;
            faceDirection = 1;
        }
        else if (xDir < -0.1f)
        {
            _sr.flipX = true;
            faceDirection = -1;
        }
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

    }
    #endregion
    #region JUMP
    void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) /*|| Input.GetKeyDown(KeyCode.W)*/ || Input.GetKeyDown(KeyCode.UpArrow))
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
            //data.jumpBufferCounterTime = 0f;
            Debug.Log("Jump");
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canJump = false;
        }
        else
        {
            onJumpPressed = false;
            if (jumpUp && !onGround)
            {
                Debug.Log("RequestJumpBuffer");
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
        Debug.Log("JumpBuffer");
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
        if (doDash)
        {
            doDash = false;
            dashing = true;
            dashStart = transform.position;
        }
        if(dashing)
        {
            _rb.AddForce(transform.right * dashForce * faceDirection, ForceMode2D.Impulse);
            //transform.position = new Vector2(transform.position.x, data.dashStart.y);
            if (Vector2.Distance(transform.position, dashStart) >= dashDistance)
            {
                dashing = false;
            }
        }
    }


    #endregion
    void GroundCheckPos()
    {
        groundColl = Physics2D.OverlapBox
            (groundCheckPos.transform.position, groundCheckSize, 0, groundLayer);
        if (groundColl == null) //Not on ground
        {
            onGround = false;
        }
        else //On ground
        {
            onGround = true;
            canJump = true;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPos.transform.position, groundCheckSize);
    }
}

