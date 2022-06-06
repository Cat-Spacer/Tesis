using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //[SerializeField] private PlayerData data;

    //[SerializeField] float runMaxSpeed, jumpForce, jumpCutMultiplier;

    //#region COMPONENTS
    //private Rigidbody2D rb;
    //#endregion

    //#region STATE PARAMETERS
    //private bool _isFacingRight;
    //private bool _isJumping;
    //private bool _isWallJumping;
    //private bool _isDashing;

    //private int _lastWallJumpDir;
    //private float _wallJumpStartTime;

    //private int _dashesLeft;
    //private float _dashStartTime;
    //private Vector2 _lastDashDir;

    //private float _lastOnGroundTime;
    //private float _lastOnWallTime;
    //private float _lastOnWallRightTime;
    //private float _lastOnWallLeftTime;
    //#endregion

    //#region INPUT PARAMETERS
    //private Vector2 _moveInput;
    //private float _lastPressedJumpTime;
    //private float _lastPressedDashTime;
    //#endregion

    //#region CHECK PARAMETERS
    //[Header("Checks")]
    //[SerializeField] private Transform _groundCheckPoint;
    //[SerializeField] private Vector2 _groundCheckSize;
    //[Space(5)]
    //[SerializeField] private Transform _frontWallCheckPoint;
    //[SerializeField] private Transform _backWallCheckPoint;
    //[SerializeField] private Vector2 _wallCheckSize;
    //#endregion

    //#region Layers & Tags
    //[Header("Layers & Tags")]
    //[SerializeField] private LayerMask _groundLayer;
    //#endregion

    //private void Start()
    //{
    //    rb = GetComponent<Rigidbody2D>();
    //    SetGravityScale(data.gravityScale);
    //}

    //private void Update()
    //{
    //    #region INPUT HANDLER
    //    _moveInput.x = Input.GetAxisRaw("Horizontal");
    //    _moveInput.y = Input.GetAxisRaw("Vertical");

    //    if (Input.GetKeyDown(KeyCode.C))
    //    {
    //        _lastPressedJumpTime = data.jumpBufferTime;
    //    }

    //    if (Input.GetKeyDown(KeyCode.X))
    //    {
    //        _lastPressedDashTime = data.dashBufferTime;
    //    }
    //    #endregion

    //    #region TIMERS
    //    _lastOnGroundTime -= Time.deltaTime;
    //    _lastOnWallTime -= Time.deltaTime;
    //    _lastOnWallRightTime -= Time.deltaTime;
    //    _lastOnWallLeftTime -= Time.deltaTime;

    //    _lastPressedJumpTime -= Time.deltaTime;
    //    _lastPressedDashTime -= Time.deltaTime;
    //    #endregion

    //    #region GENERAL CHECKS
    //    if (_moveInput.x != 0)
    //        CheckDirectionToFace(_moveInput.x > 0);
    //    #endregion

    //    #region PHYSICS CHECKS
    //    //Ground Check

    //    if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer)) //checks if set box overlaps with ground
    //        _lastOnGroundTime = 0; //if so sets the lastGrounded to coyoteTime

    //    //Right Wall Check

    //    if ((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && _isFacingRight)
    //            || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !_isFacingRight))
    //        _lastOnWallRightTime = 0;

    //    //Right Wall Check

    //    if ((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !_isFacingRight)
    //        || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && _isFacingRight))
    //        _lastOnWallLeftTime = 0;

    //    _lastOnWallTime = Mathf.Max(_lastOnWallLeftTime, _lastOnWallRightTime);
    //    //Two checks needed for both left and right walls since whenever the play turns the wall checkPoints swap sides
    //    #endregion

    //        #region JUMP CHECKS
    //        if (_isJumping && rb.velocity.y < 0)
    //            _isJumping = false;

    //    if (_isWallJumping && rb.velocity.y < 0)
    //        _isWallJumping = false;

    //    if (_lastPressedJumpTime > 0 && !_isDashing)
    //    {
    //        if (_lastOnGroundTime > 0)
    //        {
    //            _isJumping = true;
    //            _isWallJumping = false;
    //            Jump();
    //        }
    //        else if (_lastOnWallTime > 0)
    //        {
    //            _isJumping = false;
    //            _isWallJumping = true;
    //            WallJump((_lastOnWallRightTime > 0) ? -1 : 1);
    //        }
    //    }
    //    #endregion
    //}

    //private void FixedUpdate()
    //{
    //    if (_isWallJumping)
    //        Run(0);
    //    else
    //        Run(1);
    //}

    //#region MOVEMENT METHODS
    //public void SetGravityScale(float scale)
    //{
    //    rb.gravityScale = scale;
    //}

    //private void Run(float lerpAmount)
    //{
    //    float targetSpeed = InputHandler.instance.MoveInput.x * data.runMaxSpeed; //calculate the direction we want to move in and our desired velocity
    //    float speedDif = targetSpeed - RB.velocity.x; //calculate difference between current velocity and desired velocity

    //    #region Acceleration Rate
    //    float accelRate;

    //    //gets an acceleration value based on if we are accelerating(includes turning) or trying to decelerate(stop). As well as applying a multiplier if we're air borne

    //    if (LastOnGroundTime > 0)
    //        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccel : data.runDeccel;
    //    else
    //        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccel * data.accelInAir : data.runDeccel * data.deccelInAir;

    //    //if we want to run but are already going faster than max run speed

    //    if (((RB.velocity.x > targetSpeed && targetSpeed > 0.01f) || (RB.velocity.x < targetSpeed && targetSpeed < -0.01f)) && data.doKeepRunMomentum)
    //    {
    //        accelRate = 0; //prevent any deceleration from happening, or in other words conserve are current momentum
    //    }
    //    #endregion

    //    #region Velocity Power
    //    float velPower;
    //    if (Mathf.Abs(targetSpeed) < 0.01f)
    //    {
    //        velPower = data.stopPower;
    //    }
    //    else if (Mathf.Abs(RB.velocity.x) > 0 && (Mathf.Sign(targetSpeed) != Mathf.Sign(RB.velocity.x)))
    //    {
    //        velPower = data.turnPower;
    //    }
    //    else
    //    {
    //        velPower = data.accelPower;
    //    }
    //    #endregion

    //    //applies acceleration to speed difference, then is raised to a set power so the acceleration increases with higher speeds, finally multiplies by sign to preserve direction

    //        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
    //    movement = Mathf.Lerp(RB.velocity.x, movement, lerpAmount); // lerp so that we can prevent the Run from immediately slowing the player down, in some situations eg wall jump, dash 

    //    RB.AddForce(movement * Vector2.right); // applies force force to rigidbody, multiplying by Vector2.right so that it only affects X axis 

    //    if (InputHandler.instance.MoveInput.x != 0)
    //        CheckDirectionToFace(InputHandler.instance.MoveInput.x > 0);
    //    }
    //    private void Turn()
    //    {
    //        Vector3 scale = transform.localScale; //stores scale and flips x axis, "flipping" the entire gameObject around. (could rotate the player instead)
    //        scale.x *= -1;
    //        transform.localScale = scale;

    //        _isFacingRight = !_isFacingRight; //flip bool
    //    }

    //    private void Jump()
    //    {
    //        //ensures we can't call a jump multiple times from one press

    //        _lastPressedJumpTime = 0;
    //        _lastOnGroundTime = 0;

    //        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    //    }

    //    private void WallJump(int dir)
    //    {
    //        //ensures we can't call a jump multiple times from one press

    //        _lastPressedJumpTime = 0;
    //        _lastOnGroundTime = 0;
    //        _lastOnWallRightTime = 0;
    //        _lastOnWallLeftTime = 0;

    //        Vector2 force = new Vector2(data.wallJumpForce.x, data.wallJumpForce.y);
    //        force.x *= -dir; //apply force in opposite direction of wall
    //        rb.velocity = force;
    //    }

    //    private void JumpCut()
    //    {
    //        //applies force downward when the jump button is released.Allowing the player to control jump height

    //        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCutMultiplier);
    //    }

    //    private void Slide()
    //    {
    //        //works the same as the Run but only in the y-axis

    //        rb.velocity = new Vector2(rb.velocity.x, -data.slideAccel);
    //    }

    //    private void StartDash(Vector2 dir)
    //    {
    //        _lastOnGroundTime = 0;
    //        _lastPressedDashTime = 0;

    //        rb.velocity = dir.normalized * data.dashSpeed;

    //        _isDashing = true;
    //        SetGravityScale(0);
    //    }

    //    private void StopDash(Vector2 dir)
    //    {
    //        _isDashing = false;
    //        SetGravityScale(data.gravityScale);
    //    }
    //    #endregion

    //    #region CHECK METHODS
    //    public void CheckDirectionToFace(bool isMovingRight)
    //    {
    //        if (isMovingRight != _isFacingRight)
    //            Turn();
    //    }
    //    #endregion
    //}
}
