using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputKey;
public class HamsterInputCoop : MonoBehaviour
{
    private PlayerCharacter _character;
    private CharacterData _data;
    private CharacterModel _model;
    public  bool left_Input { get; private set; }
    public bool down_Input { get; private set; }
    public  bool right_Input { get; private set; }
    public  bool attack_Input { get; private set; }
    public  bool interact_Input { get; private set; }

    void Start()
    {
        _character = GetComponent<PlayerCharacter>();
        _data = GetComponent<CharacterData>();
        _model = GetComponent<CharacterModel>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _character.JumpUp(true);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            _character.StopJump();
        }
        down_Input = Input.GetKey(KeyCode.DownArrow);
        left_Input = Input.GetKey(KeyCode.LeftArrow);
        right_Input = Input.GetKey(KeyCode.RightAlt);
        
        attack_Input = Input.GetKeyDown(KeyCode.Keypad1);
        if (attack_Input)  _character.Punch();

        interact_Input = Input.GetKeyDown(KeyCode.Keypad2);
        if(interact_Input) _character.Interact(interact_Input);
        else _character.Interact(interact_Input);
    }
    
    private void FixedUpdate()
    {
        if (right_Input)
        {
            _character.Movement( true,1);
        }
        else if (left_Input)
        {
            _character.Movement(true,-1);
        }
        else
        {
            _character.Movement(false,0);
        }
    }

}
