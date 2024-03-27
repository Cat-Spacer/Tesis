using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputKey;
using Photon.Pun;
public class CatInputCoop : MonoBehaviourPunCallbacks
{
    private PlayerCharacter _character;
    private CatSpecial _catSpecial;
    private CharacterData _data;
    private CharacterModel _model;
    public bool up_Input { get; private set; }
    public  bool left_Input { get; private set; }
    public bool down_Input { get; private set; }
    public  bool right_Input { get; private set; }
    public  bool attack_Input { get; private set; }
    public  bool impulse_Input { get; private set; }
    public  bool spit_Input { get; private set; }
    public  bool interact_Input { get; private set; }
    public  bool special_Input { get; private set; }
    public  bool drop_Input { get; private set; }
    
    void Start()
    {
        _character = GetComponent<PlayerCharacter>();
        _data = GetComponent<CharacterData>();
        _model = GetComponent<CharacterModel>();
        _catSpecial = GetComponent<CatSpecial>();
    }

    // Update is called once per frame
    void Update()
    {
        //up_Input = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
        {
            _character.JumpUp(true);
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space))
        {
            _character.StopJump();
        }
        down_Input = Input.GetKey(KeyCode.S);
        left_Input = Input.GetKey(KeyCode.A);
        right_Input = Input.GetKey(KeyCode.D);
        
        attack_Input = Input.GetKeyDown(KeyCode.J);
        if (attack_Input)  _character.Punch();
        
        impulse_Input = Input.GetKeyDown(KeyCode.K);
        if(impulse_Input) _character.JumpImpulse();
        
        // spit_Input = Input.GetKeyDown(KeyCode.L);
        // if(spit_Input) _character.Special();
        
        interact_Input = Input.GetKeyDown(KeyCode.E);
        if(interact_Input) _character.Interact(interact_Input);
        else _character.Interact(interact_Input);
        
        special_Input = Input.GetKeyDown(KeyCode.L);
        if(special_Input) _catSpecial.Special();
        
        drop_Input = Input.GetKeyDown(KeyCode.Q);
        if(drop_Input) _character.DropItem();
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
