using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class HamsterCharacterInput : MonoBehaviourPunCallbacks
{
    Action _InputAction = delegate {  };
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
    public  bool special_Input { get; private set; }
    public  bool drop_Input { get; private set; }
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
        if (!photonView.IsMine) return;
        //up_Input = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space);

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) && !_hamster.InTube())
        {
            _character.JumpUp(true);
        }

        if ((Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space)) && !_hamster.InTube())
        {
            _character.StopJump();
        }

        up_Input = Input.GetKey(KeyCode.W);
        down_Input = Input.GetKey(KeyCode.S);
        left_Input = Input.GetKey(KeyCode.A);
        right_Input = Input.GetKey(KeyCode.D);
        
        attack_Input = Input.GetKeyDown(KeyCode.J);
        if (attack_Input) photonView.RPC("RPCPunch", RpcTarget.All);
        impulse_Input = Input.GetKeyDown(KeyCode.K);
        if(impulse_Input) photonView.RPC("RPCImpulse", RpcTarget.All);
        
        interact_Input = Input.GetKeyDown(KeyCode.E);
        if(interact_Input) photonView.RPC("RPCInteract", RpcTarget.All, true);
        else photonView.RPC("RPCInteract", RpcTarget.All, false);
        
        drop_Input = Input.GetKeyDown(KeyCode.Q);
        if(drop_Input) photonView.RPC("RPCDrop", RpcTarget.All);
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

        // if (Input.GetKeyDown(KeyCode.UpArrow))
        // {
        //     _character.JumpUp(true);
        // }
        //
        // if (Input.GetKeyUp(KeyCode.UpArrow))
        // {
        //     _character.StopJump();
        // }
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
    
    [PunRPC]
    void RPCPunch()
    {
        _character.Punch();
    }
    [PunRPC]
    void RPCImpulse()
    {
        _character.JumpImpulse();
    }
    [PunRPC]
    void RPCInteract(bool _do)
    {
        _character.Interact(_do);
    }
    [PunRPC]
    void RPCDrop(bool _do)
    {
        _character.DropItem();
    }
}
