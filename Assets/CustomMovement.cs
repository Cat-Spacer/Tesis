using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMovement : MonoBehaviour
{
    [Header("PlayerSpeed")]
    [SerializeField] float maxSpeed;
    [SerializeField] float runAccel;
    [SerializeField] float runDeccel;
    [SerializeField] float stopPower;
    [SerializeField] float turnPower;
    [SerializeField] float accelPower;

    [Header("PlayerJump")]
    [SerializeField] float jumpForce;
    [SerializeField] float stopJumpForce;
    bool onJumpPressed;
    bool onJumpReleased;
    bool onJump;

    [Header("GroundCheck")]
    [SerializeField] Transform groundCheckPos;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector2 groundCheckSize;
    Collider2D groundColl;
    bool onGround;

    [Header("CoyoteJump")]
    [SerializeField] float coyoteLeftGroundTime;
    [SerializeField] float coyoteTreshhold;
    [SerializeField] bool coyoteOnGroundLeave;
    [SerializeField] bool canCoyoteJump;

    [Header("Gravity")]
    [SerializeField] float gravityForce;

    public bool doKeepRunMomentum;
    Rigidbody2D rb;
    float xMove, yMove;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        xMove = Input.GetAxis("Horizontal");
        JumpInput();
        //yMove = Input.GetAxis("Vertical");
    }
    private void FixedUpdate()
    {
        rb.AddForce(Vector2.down * gravityForce);
        Run(xMove, 1);
        GroundCheckPos();
        JumpUp(onJumpPressed);
        JumpStop(onJumpReleased);
    }
    private void Run(float xDir,float lerpAmount)
    {
        float targetSpeed = xDir * maxSpeed; 
        float speedDif = targetSpeed - rb.velocity.x; 

        #region Acceleration Rate
        float accelRate;

        if(Mathf.Abs(targetSpeed) > 0.01f)
        {
            //accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccel : runDeccel;
            accelRate = runAccel;
            //Debug.Log("Accel");
        }
        else
        {
            accelRate = runDeccel;
            //Debug.Log("Deccel");
        }
        //if (LastOnGroundTime > 0)
        //    accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccel : data.runDeccel;
        //else
        //    accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccel * data.accelInAir : data.runDeccel * data.deccelInAir;

        //if we want to run but are already going faster than max run speed

        if (((rb.velocity.x > targetSpeed && targetSpeed > 0.01f) || (rb.velocity.x < targetSpeed && targetSpeed < -0.01f)) && doKeepRunMomentum)
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

        //if (InputHandler.instance.MoveInput.x != 0)
        //    CheckDirectionToFace(InputHandler.instance.MoveInput.x > 0);
    }
    void JumpInput()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            onJumpPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            onJumpReleased = true;
        }
    }
    void JumpUp(bool jumpUp)
    {
        //if (!jumpUp && ! ) return;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        onJumpPressed = false;
        onJump = true;
    }
    void JumpStop(bool jumpStop)
    {
        if (!jumpStop && onGround) return;
        rb.AddForce(Vector2.down * stopJumpForce, ForceMode2D.Impulse);
        onJumpReleased = false;
    }
    void GroundCheckPos()
    {
        groundColl = Physics2D.OverlapBox
            (groundCheckPos.transform.position, groundCheckSize, 0, groundLayer);
        if (groundColl == null) //Not on ground
        {
            //if (coyoteOnGroundLeave == true)
            //{
            //    coyoteLeftGroundTime = Time.time;
            //    coyoteOnGroundLeave = false;
            //}
            //if (coyoteLeftGroundTime + coyoteTreshhold < Time.time) canCoyoteJump = true;
            //else canCoyoteJump = false;
            if (!onJumpPressed && !onJump)
            {
                Debug.Log("CanCoyoteJump");
                canCoyoteJump = true;
                StartCoroutine(CoyoteTime());
            }
            onGround = false;
            coyoteOnGroundLeave = true;
        }
        else //On ground
        {
            onGround = true;
            onJump = false;
        }

    }
    IEnumerator CoyoteTime()
    {
        yield return new WaitForSeconds(coyoteTreshhold);
        canCoyoteJump = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPos.transform.position, groundCheckSize);
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == 6)
    //    {
    //        onJump = false;
    //    }
    //}
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == 6)
    //    {
    //        onJump = true;
    //    }
    //}
}

