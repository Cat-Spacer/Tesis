using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDatas : MonoBehaviour
{
    [Header("PlayerMovement")]
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected float runAccel;
    [SerializeField] protected float runDeccel;
    [SerializeField] protected float stopPower;
    [SerializeField] protected float turnPower;
    [SerializeField] protected float accelPower;
    [SerializeField] public int faceDirection = 1;

    [Header("PlayerAirMovement")]
    [SerializeField] protected float airRunAccel;
    [SerializeField] protected float airRunDeccel;

    [Header("PlayerJump")]
    [SerializeField] protected float jumpForce;
    [SerializeField] protected float stopJumpForce;
    [SerializeField] protected Vector2 wallJumpDir;
    [HideInInspector] protected bool onJumpPressed;
    [HideInInspector] protected bool onJumpReleased;
    [SerializeField] protected bool canJump;
    [Header("PlayerBufferJump")]
    [SerializeField] protected bool doJumpBuffer;
    [SerializeField] protected float jumpBufferCounterTime;
    [SerializeField] protected float jumpBufferTime;

    [Header("PlayerCoyoteTime")]
    [SerializeField] protected float coyoteTimeCounter;
    [SerializeField] protected float coyoteTime;

    [Header("Dash")]
    [SerializeField] protected float dashDistance;
    [SerializeField] protected float dashForce;
    [SerializeField] protected bool dashing;
    [SerializeField] protected bool doDash;
    [SerializeField] protected bool canDash;
    [SerializeField] protected Vector2 dashStart;

    [Header("GroundCheck")]
    [SerializeField] protected Transform groundCheckPos;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected Vector2 groundCheckSize;
    [HideInInspector] public Collider2D groundColl;
    [SerializeField] protected bool onGround;

    [Header("Gravity")]
    public float gravityForce;
    public float gravityForceDefault;

    [Header("Attack")]
    [SerializeField] protected Vector2 attackRange;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask damageable;
    [HideInInspector] protected float xMove, yMove;

    [Header("Animation")]
    [SerializeField] public Animator anim;
    [SerializeField] SpriteRenderer _sr;
    [SerializeField] protected bool jumping;

    [Header("Particles")]
    [SerializeField] protected ParticleSystem _jumpParticle;
    [SerializeField] protected ParticleSystem _fallParticle;
    [SerializeField] protected ParticleSystem _dashParticleExplotion;
    [SerializeField] protected ParticleSystem _dashParticleTrail;
    [SerializeField] protected ParticleSystem _attackParticle;
    [SerializeField] protected ParticleSystem _climbParticle;

    [Header("WallClimb")]
    [SerializeField] protected int _wallLayerNumber;
    [SerializeField] protected float _gravityScale;
    [SerializeField] protected bool _onWall;
    [SerializeField] protected bool _onClimb;
    [SerializeField] protected bool stopClimbing = false;
    [SerializeField] protected float _climbSpeed = 5.0f;

    [Header("Constrains")]
    [SerializeField] protected RigidbodyConstraints2D constraints2D;
}
