using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDatas : MonoBehaviour
{
    [Header("PlayerMovement")]
    public float maxSpeed;
    public float runAccel;
    public float runDeccel;
    public float stopPower;
    public float turnPower;
    public float accelPower;
    public int faceDirection = 1;

    [Header("PlayerAirMovement")]
    public float airRunAccel;
    public float airRunDeccel;

    [Header("PlayerJump")]
    public float jumpForce;
    public float stopJumpForce;
    [HideInInspector] public bool onJumpPressed;
    [HideInInspector] public bool onJumpReleased;
     public bool canJump;
    [Header("PlayerBufferJump")]
    public bool doJumpBuffer;
    public float jumpBufferCounterTime;
    public float jumpBufferTime;

    [Header("PlayerCoyoteTime")]
    public float coyoteTimeCounter;
    public float coyoteTime;

    [Header("Dash")]
    public float dashDistance;
    public float dashForce;
    public bool dashing;
    public bool doDash;
    public bool canDash;
    public Vector2 dashStart;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public LayerMask groundLayer;
    public Vector2 groundCheckSize;
    [HideInInspector] public Collider2D groundColl;
    public bool onGround;

    [Header("Gravity")]
    public float gravityForce;

    [Header("Attack")]
    public Vector2 attackRange;
    public Transform attackPoint;
    public LayerMask damageable;
    [HideInInspector] public float xMove, yMove;
}
