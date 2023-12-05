using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    [Header("Punch")] public float punchForce;
    
    [Header("Ground Movement")] 
    public float maxSpeed;
    [HideInInspector] public float defaultMaxSpeed; 
    public float runAcel; 
    public float groundFriction;
    public float turnPower;
    public int faceDirection = 1;
    public bool isRunning;
    
    [Header("Air Movement")] 
    public float airMaxSpeed;
    public float airRunAcel;
    public float airFriction;
    public bool isAirRunning;
    
    [Header("Jump")] 
    public float jumpForce;
    public bool isJumping;
    public float jumpTime;
    public float jumpCounter = 0;
    public float jumpMultiplier = 0;
    public float jumpStopForce;

    [Header("Attack")] 
    public float stunForce;
    public Transform attackPoint;
    public Vector2 attackRange;

    [Header("JumpImpulse")]
    public float jumpImpulse;
    public Vector2 jumpInpulseArea;
    
    [Header("Stun")]
    public float stunCounter;
    public bool isStun;

    [Header("Knockback")] 
    public float knockbackTime;
    public float knockbackCounter;
    public float knockbackSpeedDecel;
    public bool onKnockback;
    public Vector2 knockbackDir;
    [HideInInspector] public float knockbackForce;

    [Header("Bounce")] 
    public PhysicsMaterial2D bouncePhysicsMat;
    public PhysicsMaterial2D characterPhysicsMat;
    public Transform bounceDetectionRight;
    public Transform bounceDetectionLeft;
    public Vector2 bounceSize;
    public float bounceSpeedModifier;
    public bool doBounce;
    [HideInInspector] public Collider2D currentWall;

    [Header("Ground")] 
    public Vector2 groundCheckArea;
    public Transform groundPos;
    public bool onGround;
    
    [Header("Gravity")]
    public float fallMultiplier;
    [HideInInspector] public Vector2 gravity;
    public bool isFalling;
    [HideInInspector] public bool running = false;

    [Header("Interact")] 
    public Vector2 interactSize;
    public IInteract _interactObj;

    [Header("CatSpecial")] 
    public float specialTimmer;
    
    [Header("Canvas")] 
    public PlayerCanvas canvas;

    [Header("Inventory")] 
    public float dropForce;
    public Vector3 _dropOffset;
    public bool _hasItem;
    public Item _onHand;
    public Transform _inventoryPos;
    
    [Header("Layers")] 
    public LayerMask groundLayer;
    public LayerMask attackableLayer;
    public LayerMask playerMask;
    public LayerMask interactMask;
}
