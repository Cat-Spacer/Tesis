using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDatas : MonoBehaviour
{
    [Header("PlayerMovement")]
    [SerializeField] protected float maxSpeed;
    [HideInInspector] protected float defaultMaxSpeed;
    [SerializeField] protected float runAccel;
    [SerializeField] protected float runDeccel;
    [SerializeField] protected float stopPower;
    [SerializeField] protected float turnPower;
    [SerializeField] protected float accelPower;
    [SerializeField] public static int faceDirection = 1;

    [Header("PlayerAirMovement")]
    [SerializeField] protected float airRunAccel;
    [SerializeField] protected float airRunDeccel;

    [Header("PlayerJump")]
    [SerializeField] protected float jumpForce;
    [SerializeField] protected float stopJumpForce;
    [SerializeField] protected Vector2 wallJumpDir;
    [SerializeField] protected bool canJump;
    [Header("PlayerBufferJump")]
    [SerializeField] protected bool doJumpBuffer;
    [SerializeField] protected float jumpBufferCounterTime;
    [SerializeField] protected float jumpBufferTime;

    [Header("PlayerCoyoteTime")]
    [SerializeField] protected float coyoteTimeCounter;
    [SerializeField] protected float coyoteTime;

    [Header("Dash")]
    [SerializeField] public float dashDistance;
    [SerializeField] protected float dashForce;
    [SerializeField] public static bool isDashing;
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
    [SerializeField]protected float attackCd = 0;
    [SerializeField]protected bool canAttack = true;
    [SerializeField] protected Vector2 attackRange;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask damageable;

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

    [Header("Attributes Wall Climb")]
    [SerializeField] protected float _upSpeed = 5;
    [SerializeField] protected float _downSpeed = 5 * 0.8f;
    [SerializeField] protected float _distanceToRope = 0.4f;
    [SerializeField] protected LayerMask _climbLayerMask;
    [SerializeField] protected float _impulseDirectionExitForce = 6;
    [SerializeField] protected float _impulseExitRopeForce = 20;
    [SerializeField] protected float dashForceClimb;

    [Header("Interact")]
    [SerializeField] protected Vector2 _interactSize; 
    [SerializeField] protected LayerMask _interactMask; 
    /* [Header("WallClimb")]
     [SerializeField] protected int _wallLayerNumber;
     [SerializeField] protected float _gravityScale;
     [SerializeField] protected bool _onWall;
     [SerializeField] protected bool _onClimb;
     [SerializeField] protected bool _canClimb;
     [HideInInspector] protected bool _doClimbUp;
     [HideInInspector] protected bool _doClimbDown;
     [HideInInspector] protected bool _doClimbStaticLeft;
     [HideInInspector] protected bool _doClimbStaticRight;
     [SerializeField] protected bool stopClimbing = false;
     [SerializeField] protected float _climbSpeed = 5.0f;
     [SerializeField] protected float _wallJumpForceX = 5.0f;
     [SerializeField] protected float _wallJumpForceY = 5.0f;*/

    [Header("Inputs")]
    [HideInInspector] protected float xMove, yMove;
    [HideInInspector] protected bool onJumpInput;
    [HideInInspector] protected bool onJumpInputReleased;
    [SerializeField] protected bool onDashInput;
    [HideInInspector] protected bool w_Input;
    [HideInInspector] protected bool a_Input;
    [HideInInspector] protected bool s_Imput;
    [HideInInspector] protected bool d_Input;
    [HideInInspector] protected bool attackInput;
    [HideInInspector] protected bool interactionInput;


    [Header("Constrains")]
    [SerializeField] protected RigidbodyConstraints2D constraints2D;

    [Header("Canvas")]
    [SerializeField] protected PlayerCanvas _playerCanvas;

    [Header("Other")]
    protected bool dead; 
    [SerializeField] protected bool showGizmos;
}
