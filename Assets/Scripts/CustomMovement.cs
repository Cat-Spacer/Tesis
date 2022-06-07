using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMovement : MonoBehaviour
{
    [SerializeField] PlayerDatas data;
    [SerializeField] SpriteRenderer sr;
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        data.coyoteTimeCounter = data.coyoteTime;
        data.jumpBufferCounterTime = data.jumpBufferTime;
    }
    private void Update()
    {
        data.xMove = Input.GetAxis("Horizontal");
        JumpInput();
        TimeCounterCoyote();
        TimeCounterJumpbuffer();
    }
    private void FixedUpdate()
    {
        rb.AddForce(Vector2.down * data.gravityForce);
        Run(data.xMove, 1);
        GroundCheckPos();
        JumpUp(data.onJumpPressed);
        JumpStop(data.onJumpReleased);
    }
    private void Run(float xDir,float lerpAmount)
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
    void JumpInput()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            data.onJumpPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            data.onJumpReleased = true;
        }
    }
    void JumpUp(bool jumpUp)
    {
        if (data.canJump && jumpUp && data.coyoteTimeCounter > 0f && data.jumpBufferCounterTime > 0f)
        {
            Debug.Log("Saltando");
            data.jumpBufferCounterTime = 0f;
            rb.AddForce(Vector2.up * data.jumpForce, ForceMode2D.Impulse);
            data.onJumpPressed = false;
            data.canJump = false;
        }
    }
    void JumpStop(bool jumpStop)
    {
        if (jumpStop && !data.onGround)
        {
            rb.AddForce(Vector2.down * data.stopJumpForce, ForceMode2D.Impulse);
            data.onJumpReleased = false;
        }
    }
    void GroundCheckPos()
    {
        data.groundColl = Physics2D.OverlapBox
            (data.groundCheckPos.transform.position, data.groundCheckSize, 0, data.groundLayer);
        if (data.groundColl == null) //Not on ground
        {
            data.onGround = false;
            data.coyoteOnGroundLeave = true;
        }
        else //On ground
        {
            data.onGround = true;
            data.canJump = true;
        }
    }
    void TimeCounterCoyote()
    {
        if (data.onGround || data.coyoteTime <= 0) data.coyoteTimeCounter = data.coyoteTime;
        else data.coyoteTimeCounter -= 1*Time.deltaTime;
    }
    void TimeCounterJumpbuffer()
    {
        if (data.onJumpPressed) data.jumpBufferCounterTime = data.jumpBufferTime;
        else data.jumpBufferCounterTime -= Time.deltaTime;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(data.groundCheckPos.transform.position, data.groundCheckSize);
    }
}

