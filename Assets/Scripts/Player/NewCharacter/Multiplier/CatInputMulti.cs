using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputKey;
using Photon.Pun;
public class CatInputMulti : MonoBehaviourPunCallbacks
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
        if (!photonView.IsMine) return;
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
        if (attack_Input) photonView.RPC("RPCPunch", RpcTarget.All);
        
        impulse_Input = Input.GetKeyDown(KeyCode.K);
        if(impulse_Input) photonView.RPC("RPCImpulse", RpcTarget.All);
        
        // spit_Input = Input.GetKeyDown(KeyCode.L);
        // if(spit_Input) _character.Special();
        
        interact_Input = Input.GetKeyDown(KeyCode.E);
        if(interact_Input) photonView.RPC("RPCInteract", RpcTarget.All, true);
        else photonView.RPC("RPCInteract", RpcTarget.All, false);
        
        special_Input = Input.GetKeyDown(KeyCode.L);
        if(special_Input) photonView.RPC("RPCSpecial", RpcTarget.All);
        
        drop_Input = Input.GetKeyDown(KeyCode.Q);
        if(drop_Input) photonView.RPC("RPCDrop", RpcTarget.All);
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
    void RPCSpecial()
    {
        _catSpecial.Special();
    }
    [PunRPC]
    void RPCDrop()
    {
        _character.DropItem();
    }
}
