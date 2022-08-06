using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomMovement : MonoBehaviour
{
    [SerializeField] PlayerDatas data;
    [SerializeField] SpriteRenderer sr;
    Rigidbody2D rb;
    private Action TimeCounterAction;
    private Action InputsAction;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        data.coyoteTimeCounter = data.coyoteTime;
        data.jumpBufferCounterTime = data.jumpBufferTime;
        TimeCounterAction += TimeCounterCoyote;
        InputsAction = MovementInput;
        InputsAction += JumpInput;
        InputsAction += DashInput;
    }
    private void Update()
    {
        InputsAction();
        TimeCounterAction();
    }
    private void FixedUpdate()
    {
        rb.AddForce(Vector2.down * data.gravityForce);
        Movement(data.xMove, 1);
        GroundCheckPos();
        JumpUp(data.onJumpPressed);
        Dash();
        //JumpStop(data.onJumpReleased);
    }
    #region Movement
    private void MovementInput()
    {
        data.xMove = Input.GetAxis("Horizontal");
    }
    private void Movement(float xDir,float lerpAmount)
    {
        if (xDir > 0.1f)
        {
            sr.flipX = false;
        }
        else if (xDir < -0.1f)
        {
            sr.flipX = true;
        }
        float targetSpeed = xDir * data.maxSpeed; 
        float speedDif = targetSpeed - rb.velocity.x; 

        #region Acceleration Rate
        float accelRate;

        if(Mathf.Abs(targetSpeed) > 0.01f)
        {
            accelRate = data.runAccel;
        }
        else
        {
            accelRate = data.runDeccel;
        }

        if (data.onGround) //GroundMovement
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccel : data.runDeccel;
        else //AirMovement
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccel * data.airRunAccel : data.runDeccel * data.airRunDeccel;

        if ((rb.velocity.x > targetSpeed && targetSpeed > 0.01f) || (rb.velocity.x < targetSpeed && targetSpeed < -0.01f))
        {
            accelRate = 0;
        }
        #endregion

        #region Velocity Power
        float velPower;
        if (Mathf.Abs(targetSpeed) < 0.01f)
        {
            velPower = data.stopPower;
        }
        else if (Mathf.Abs(rb.velocity.x) > 0 && (Mathf.Sign(targetSpeed) != Mathf.Sign(rb.velocity.x)))
        {
            velPower = data.turnPower;
        }
        else
        {
            velPower = data.accelPower;
        }
        #endregion    

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        movement = Mathf.Lerp(rb.velocity.x, movement, lerpAmount); 

        rb.AddForce(movement * Vector2.right);

    }
    #endregion
    #region JUMP
    void JumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            data.onJumpPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            data.onJumpReleased = true;
        }
    }
    void JumpUp(bool jumpUp) //Saltar
    {
        if (jumpUp && data.canJump && (data.coyoteTimeCounter > 0f || data.onGround))
        {
            //data.jumpBufferCounterTime = 0f;
            Debug.Log("Jump");
            rb.AddForce(Vector2.up * data.jumpForce, ForceMode2D.Impulse);
            data.canJump = false;
        }
        else
        {
            data.onJumpPressed = false;
            if (jumpUp && !data.onGround)
            {
                Debug.Log("RequestJumpBuffer");
                data.doJumpBuffer = true;
                TimeCounterAction += TimeCounterJumpbuffer;
            }
        }
    }
    void JumpStop(bool jumpStop)
    {
        if (jumpStop && !data.onGround)
        {
            rb.AddForce(Vector2.down * data.stopJumpForce, ForceMode2D.Impulse);
        }
        else if (data.onGround) data.onJumpReleased = false;

    }

    void TimeCounterCoyote()
    {
        if (data.onGround || data.coyoteTime <= 0) data.coyoteTimeCounter = data.coyoteTime;
        else data.coyoteTimeCounter -= 1*Time.deltaTime;
    }
    void TimeCounterJumpbuffer()
    {
        Debug.Log("JumpBuffer");
        data.jumpBufferCounterTime -= Time.deltaTime;
        if (data.jumpBufferCounterTime >= 0 && data.onGround && data.doJumpBuffer)
        {
            JumpUp(true);
            data.doJumpBuffer = false;
            data.jumpBufferCounterTime = data.jumpBufferTime;
            TimeCounterAction -= TimeCounterJumpbuffer;
        }
        else if (data.jumpBufferCounterTime <= 0)
        {
            data.doJumpBuffer = false;
            data.jumpBufferCounterTime = data.jumpBufferTime;
            TimeCounterAction -= TimeCounterJumpbuffer;
        }
    }
    #endregion
    #region Dash

    void DashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            data.doDash = true;
        }
    }
    void Dash()
    {
        if (data.doDash)
        {
            data.doDash = false;
            data.dashing = true;
            data.dashStart = transform.position;
            rb.AddForce(transform.right * data.dashForce, ForceMode2D.Impulse);
        }
        if(data.dashing)
        {
            //transform.position = new Vector2(transform.position.x, data.dashStart.y);
            if (Vector2.Distance(transform.position, data.dashStart) >= data.dashDistance)
            {
                data.dashing = false;
            }
        }
    }


    #endregion
    void GroundCheckPos()
    {
        data.groundColl = Physics2D.OverlapBox
            (data.groundCheckPos.transform.position, data.groundCheckSize, 0, data.groundLayer);
        if (data.groundColl == null) //Not on ground
        {
            data.onGround = false;
        }
        else //On ground
        {
            data.onGround = true;
            data.canJump = true;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(data.groundCheckPos.transform.position, data.groundCheckSize);
    }
}

