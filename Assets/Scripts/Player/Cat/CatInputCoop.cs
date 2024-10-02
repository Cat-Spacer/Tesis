using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputKey;
using UnityEngine.InputSystem;

public class CatInputCoop : MonoBehaviour
{
    private SO_Inputs _input;
    
    private PlayerCharacter _character;
    public bool up_Input { get; private set; }
    public  bool left_Input { get; private set; }
    public bool down_Input { get; private set; }
    public  bool right_Input { get; private set; }
    public  bool attack_Input { get; private set; }
    public  bool impulse_Input { get; private set; }
    public  bool interact_Input { get; private set; }
    public  bool special_Input { get; private set; }
    public  bool drop_Input { get; private set; }

    private KeyCode leftKeycode;
    private KeyCode rightKeycode;
    private KeyCode jumpKeycode;
    private KeyCode attackKeycode;
    private KeyCode interactKeycode;
    private KeyCode specialKeycode;
    void Start()
    {
        _character = GetComponent<PlayerCharacter>();
    }

    public void SetInput(SO_Inputs inputs)
    {
        leftKeycode = inputs.left;
        rightKeycode = inputs.right;
        jumpKeycode = inputs.jump;
        attackKeycode = inputs.attack;
        interactKeycode = inputs.interact;
        specialKeycode = inputs.special;
    }
    void Update()
    {
        //up_Input = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space);
        if (Input.GetKey(jumpKeycode))
        {
            _character.JumpUp(true);
        }
        
        if (Input.GetKeyUp(jumpKeycode))
        {
            _character.StopJump();
        }
        left_Input = Input.GetKey(leftKeycode);
        right_Input = Input.GetKey(rightKeycode);
        
        attack_Input = Input.GetKeyDown(attackKeycode);
        if (attack_Input)  _character.Punch();
        
        impulse_Input = Input.GetKeyDown(specialKeycode);
        //if(impulse_Input) _character.JumpImpulse();
        
        // spit_Input = Input.GetKeyDown(KeyCode.L);
        // if(spit_Input) _character.Special();
        
        interact_Input = Input.GetKeyDown(interactKeycode);
        if(interact_Input) _character.Interact(interact_Input);
        else _character.Interact(interact_Input);

        // drop_Input = Input.GetKeyDown(KeyCode.Q);
        // if(drop_Input) _character.DropItem();
    } 
    private void FixedUpdate()
    {
        // if (right_Input)
        //     _character.Movement(1);
        // else if (left_Input)
        //     _character.Movement(-1);
    }
}
