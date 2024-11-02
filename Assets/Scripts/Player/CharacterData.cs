using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    [Header("Ground Movement")]
    public float runAcel; 
    public float groundFriction;
    public int faceDirection = 1;
    public bool isRunning;
    public bool canMove = true;
    
    [Header("Air Movement")]
    public float airRunAcel;

    [Header("Jump")] 
    public float jumpForce;
    public float doubleJumpForce;
    public bool isJumping;
    public float jumpTime;
    public float jumpCounter = 0;
    public float jumpMultiplier = 0;
    public float jumpStopForce;
    public bool canJump = true;
    public bool canDoubleJump = true;

    [Header("Attack")] 
    public float punchForce;
    public float stunForce;
    public Transform attackPoint;
    public Vector2 attackRange;
    public float punchCd;
    public bool canPunch = true;
    public bool isPunching = false;

    [Header("JumpImpulse")]
    public float jumpImpulse;
    public Vector2 jumpInpulseArea;
    
    [Header("Stun")]
    public float stunCounter;
    public bool isStun = false;

    [Header("Knockback")] 
    public float knockbackTime;
    public float knockbackCounter;
    public float knockbackSpeedDecel;
    public bool onKnockback;
    public Vector2 knockbackDir;
    [HideInInspector] public float knockbackForce;

    [Header("Ground")] 
    public Vector2 groundCheckArea;
    public Transform groundPos;
    public bool onGround;
    
    [Header("Gravity")]
    public float fallMultiplier;
    [HideInInspector] public Vector2 gravity;
    public bool isFalling;

    [Header("Interact")] 
    public Vector2 interactSize;
    public IInteract _interactObj;
    public bool isInteracting;

    [Header("Canvas")] 
    public PlayerCanvas canvas;

    [Header("Inventory")] 
    public float dropForce;
    public Vector3 _dropOffset;
    public bool _hasItem;
    public Item _onHand;
    public ItemNetwork _onHandNetwork;
    public Transform _inventoryPos;
    
    [Header("Layers")] 
    public LayerMask groundLayer;
    public LayerMask attackableLayer;
    public LayerMask playerMask;
    public LayerMask interactMask;

    [Header("Animation")]
    public bool onAnimation;
    
    [Header("Camera")] 
    public float _fallSpeedYDampingChangeTreshold;
}
