using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HamsterCharacterInput : MonoBehaviour
{
    Action _InputAction;
    private PlayerCharacter _character;
    private HamsterChar _hamster;
    private CharacterData _data;
    private CharacterModel _model;
    public bool up_Input { get; private set; }
    public  bool left_Input { get; private set; }
    public bool down_Input { get; private set; }
    public  bool right_Input { get; private set; }
    public  bool jump_InputUp { get; private set; }
    public  bool jump_InputDown { get; private set; }


    public  bool attack_Input { get; private set; }
    public  bool impulse_Input { get; private set; }
    public  bool interact_Input { get; private set; }
    void Start()
    {
        _character = GetComponent<PlayerCharacter>();
        _hamster = GetComponent<HamsterChar>();
        _data = GetComponent<CharacterData>();
        _model = GetComponent<CharacterModel>();
        _InputAction = NormalImputs;
    }

    // Update is called once per frame
    void Update()
    {
        //up_Input = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space);

        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    _character.JumpUp(true);
        //}

        //if (Input.GetKeyUp(KeyCode.UpArrow))
        //{
        //    _character.StopJump();
        //}

        jump_InputUp = Input.GetKeyUp(KeyCode.UpArrow);
        jump_InputDown = Input.GetKeyDown(KeyCode.UpArrow);
        up_Input = Input.GetKey(KeyCode.UpArrow);
        down_Input = Input.GetKey(KeyCode.DownArrow);
        left_Input = Input.GetKey(KeyCode.LeftArrow);
        right_Input = Input.GetKey(KeyCode.RightArrow);
        
        attack_Input = Input.GetKeyDown(KeyCode.Keypad1);
        if (attack_Input) _character.Punch();
        impulse_Input = Input.GetKeyDown(KeyCode.Keypad2);
        if(impulse_Input) _character.JumpImpulse();
        
        interact_Input = Input.GetKeyDown(KeyCode.Keypad4);
        if(interact_Input) _character.Interact(true);
        else _character.Interact(false);
    }
    private void FixedUpdate()
    {
        _InputAction();
    }
    void NormalImputs()
    {
        if (right_Input)
        {
            _character.Movement(true, 1);
        }
        else if (left_Input)
        {
            _character.Movement(true, -1);
        }
        else
        {
            _character.Movement(false, 0);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _character.JumpUp(true);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            _character.StopJump();
        }
    }
    void TubesInputs()
    {
        if (right_Input)
        {
            _hamster.TubeDirection(new Vector2(1,0));
        }
        else if (left_Input)
        {
            _hamster.TubeDirection(new Vector2(-1, 0));
        }
        else if (up_Input)
        {
            _hamster.TubeDirection(new Vector2(0, 1));
        }
        else if (down_Input)
        {
            _hamster.TubeDirection(new Vector2(0,-1));
        }
    }
    public void ChangeToTubesInputs(bool change)
    {
        if (change) _InputAction = TubesInputs;
        else _InputAction = NormalImputs;
    }
}
