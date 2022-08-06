using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDatas : MonoBehaviour
{
    [Header("PlayerMovement")]
    [SerializeField] public float maxSpeed;
    [SerializeField] public float runAccel;
    [SerializeField] public float runDeccel;
    [SerializeField] public float stopPower;
    [SerializeField] public float turnPower;
    [SerializeField] public float accelPower;

    [Header("PlayerAirMovement")]
    [SerializeField] public float airRunAccel;
    [SerializeField] public float airRunDeccel;

    [Header("PlayerJump")]
    [SerializeField] public float jumpForce;
    [SerializeField] public float stopJumpForce;
    [HideInInspector] public bool onJumpPressed;
    [HideInInspector] public bool onJumpReleased;
    [SerializeField] public bool canJump;
    [Header("PlayerBufferJump")]
    [SerializeField] public bool doJumpBuffer;
    [SerializeField] public float jumpBufferCounterTime;
    [SerializeField] public float jumpBufferTime;

    [Header("PlayerCoyoteTime")]
    [SerializeField] public float coyoteTimeCounter;
    [SerializeField] public float coyoteTime;

    [Header("Dash")]
    [SerializeField] public float dashDistance;
    [SerializeField] public float dashForce;
    [SerializeField] public bool dashing;
    [SerializeField] public bool doDash;
    public Vector2 dashStart;

    [Header("GroundCheck")]
    [SerializeField] public Transform groundCheckPos;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public Vector2 groundCheckSize;
    [HideInInspector] public Collider2D groundColl;
    [HideInInspector] public bool onGround;



    [Header("Gravity")]
    [SerializeField] public float gravityForce;


    [HideInInspector] public float xMove, yMove;
}
