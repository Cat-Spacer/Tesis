using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterCharacterInput : MonoBehaviour
{
    private PlayerCharacter _character;
    private CharacterData _data;
    private CharacterModel _model;
    public bool up_Input { get; private set; }
    public  bool left_Input { get; private set; }
    public bool down_Input { get; private set; }
    public  bool right_Input { get; private set; }
    public  bool attack_Input { get; private set; }
    public  bool impulse_Input { get; private set; }
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
        //up_Input = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space);

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _character.JumpUp(true);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            _character.StopJump();
        }
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
        // if (up_Input)
        // {
        //     _charMovement.JumpUp();
        // }
        // else
        // {
        //     _charMovement.StopJump();
        // }
        
    }
}
