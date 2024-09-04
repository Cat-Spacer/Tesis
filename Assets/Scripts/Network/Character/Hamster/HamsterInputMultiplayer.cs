using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputKey;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class HamsterInputMultiplayer : NetworkBehaviour
{
    private PlayerCharacterMultiplayer _character;
    private CharacterData _data;
    private CharacterModel _model;
    public  bool left_Input { get; private set; }
    public bool down_Input { get; private set; }
    public  bool right_Input { get; private set; }
    public  bool attack_Input { get; private set; }
    public  bool interact_Input { get; private set; }
    
    private Vector2 moveDirection;    
    public InputActionReference moveInput; 
    public InputActionReference jumpInput; 
    public InputActionReference attackInput; 
    public InputActionReference specialInput; 
    public InputActionReference interactInput; 

    void Start()
    {
        _character = GetComponent<PlayerCharacterMultiplayer>();
        _data = GetComponent<CharacterData>();
        _model = GetComponent<CharacterModel>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (!IsOwner) return;
        // if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
        // {
        //     _character.JumpUp(true);
        // }
        //
        // if (Input.GetKeyUp(KeyCode.W) || Input.GetKey(KeyCode.Space))
        // {
        //     _character.StopJump();
        // }
        // down_Input = Input.GetKey(KeyCode.S);
        // left_Input = Input.GetKey(KeyCode.A);
        // right_Input = Input.GetKey(KeyCode.D);
        //
        // attack_Input = Input.GetKeyDown(KeyCode.J);
        // if (attack_Input)  _character.Punch();
        //
        // interact_Input = Input.GetKeyDown(KeyCode.E);
        // if(interact_Input) _character.Interact(interact_Input);
        // else _character.Interact(interact_Input);
        
        if (!IsOwner) return;
        moveDirection = moveInput.action.ReadValue<Vector2>();
    }
    
    private void FixedUpdate()
    {
        if (!IsOwner) return;
        _character.Movement( true, (int)moveDirection.x);
        // if (right_Input)
        // {
        //     _character.Movement( true,1);
        // }
        // else if (left_Input)
        // {
        //     _character.Movement(true,-1);
        // }
        // else
        // {
        //     _character.Movement(false,0);
        // }
    }
    private void Jump(InputAction.CallbackContext obj)
    {
        if (!IsOwner) return;
        if(obj.ReadValueAsButton()) _character.JumpUp(true);
        else  _character.StopJump();
    }
    private void Special(InputAction.CallbackContext obj)
    {
        if (!IsOwner) return;
        _character.Special();
    }

    private void Attack(InputAction.CallbackContext obj)
    {
        if (!IsOwner) return;
        _character.Punch();
    }

    private void Interact(InputAction.CallbackContext obj)
    {
        if (!IsOwner) return;
        _character.Interact(obj.ReadValueAsButton());
    }
    private void OnEnable()
    {
        jumpInput.action.started += Jump;
        jumpInput.action.canceled += Jump;
        attackInput.action.started += Attack;
        specialInput.action.started += Special;
        interactInput.action.started += Interact;
    }


    private void OnDisable()
    {
        jumpInput.action.started -= Jump;
        jumpInput.action.canceled -= Jump;
        attackInput.action.started -= Attack;
        specialInput.action.started -= Special;
        interactInput.action.started -= Interact;
    }
}
