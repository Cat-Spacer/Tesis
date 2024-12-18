using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour 
{
    private Action InputAction = delegate { };
    public SO_Inputs _input;

    private PlayerCharacter _character;
    private CatCharacter _catCharacter;
    private HamsterChar _hamsterCharacter;
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
    private KeyCode upKeycode;
    private KeyCode downKeycode;
    private KeyCode jumpKeycode;
    private KeyCode attackKeycode;
    private KeyCode interactKeycode;
    private KeyCode specialKeycode;
    void Start()
    {
        _character = GetComponent<PlayerCharacter>();
        var hamster = _character.GetComponent<HamsterChar>();
        if (hamster != null)
        {
            _hamsterCharacter = _character.GetComponent<HamsterChar>();
            InputAction = HamsterInputs;
        }
        else
        {
            _catCharacter = _character.GetComponent<CatCharacter>();
            InputAction = CatInputs;
        }
    }
    private void OnEnable()
    {
        if(GameManager.Instance == null) return;
        EventManager.Instance.Subscribe(EventType.OnFinishGame, OnFinishGame);
        EventManager.Instance.Subscribe(EventType.OnLoseGame, OnFinishGame);
    }
    private void OnFinishGame(object[] obj)
    {
        InputAction = delegate { };
    }

    public void SetInput(SO_Inputs inputs)
    {
        _input = inputs;
        leftKeycode = inputs.left;
        rightKeycode = inputs.right;
        upKeycode = inputs.up;
        downKeycode = inputs.down;
        jumpKeycode = inputs.jump;
        attackKeycode = inputs.attack;
        interactKeycode = inputs.interact;
        specialKeycode = inputs.special;
    }
    void Update()
    {
        InputAction();
    }

    void CatInputs()
    {
        if (Input.GetKey(jumpKeycode)) _character.JumpUp(true);
        if (Input.GetKeyUp(jumpKeycode)) _character.StopJump();
        left_Input = Input.GetKey(leftKeycode);
        right_Input = Input.GetKey(rightKeycode);
        attack_Input = Input.GetKeyDown(attackKeycode);
        if (attack_Input)  _character.Punch();
        impulse_Input = Input.GetKeyDown(specialKeycode);
        if(impulse_Input) _character.Special();
        interact_Input = Input.GetKeyDown(interactKeycode);
        if(interact_Input) _character.Interact(interact_Input);
        else _character.Interact(interact_Input);
    }
    void HamsterInputs()
    {
        if (!_hamsterCharacter.InTube())
        {
            if (Input.GetKey(jumpKeycode)) _character.JumpUp(true);
            if (Input.GetKeyUp(jumpKeycode)) _character.StopJump();
            left_Input = Input.GetKey(leftKeycode);
            right_Input = Input.GetKey(rightKeycode);
            attack_Input = Input.GetKeyDown(attackKeycode);
            if (attack_Input)  _character.Punch();
            impulse_Input = Input.GetKeyDown(specialKeycode);
            if(impulse_Input) _character.Special();
            interact_Input = Input.GetKeyDown(interactKeycode);
            if(interact_Input) _character.Interact(interact_Input);
            else _character.Interact(interact_Input);
        }
        else
        {
            interact_Input = Input.GetKeyDown(interactKeycode);
            if(interact_Input) _character.Interact(interact_Input);
            else _character.Interact(interact_Input);
            if(Input.GetKeyDown(rightKeycode)) _hamsterCharacter.TubeDirection(Vector2.right);
            if(Input.GetKeyDown(leftKeycode)) _hamsterCharacter.TubeDirection(Vector2.left);
            if(Input.GetKeyDown(upKeycode)) _hamsterCharacter.TubeDirection(Vector2.up);
            if(Input.GetKeyDown(downKeycode)) _hamsterCharacter.TubeDirection(Vector2.down);
        }
    }
    private void FixedUpdate()
    {
        if (right_Input)
            _character.Movement(true,1);
        else if (left_Input)
            _character.Movement(true,-1);
        else _character.Movement(false,0);
    }

    public void GivePunchButton()
    {
        attackKeycode = _input.attack;
    }

    public void TakeAwayPunchButton()
    {
        attackKeycode = KeyCode.None;
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe(EventType.OnFinishGame, OnFinishGame);
        EventManager.Instance.Unsubscribe(EventType.OnLoseGame, OnFinishGame);
    }
}
