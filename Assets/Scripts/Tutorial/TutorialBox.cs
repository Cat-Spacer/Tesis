using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialBoxType
{
    None,
    Movement,
    Jump,
    Interact,
    Special,
    Punch,
    DoubleJump,
    TubesMovement,
    CatFace,
    HamsterFace
}

public class TutorialBox : MonoBehaviour
{
    [SerializeField] private TutorialBot catBot;
    [SerializeField] private TutorialBot hamsterBot;
    [SerializeField] TutorialBoxType type;
    BoxCollider2D boxCollider;
    [SerializeField] private bool sharedTutorial = true;
    [SerializeField] CharacterType singleTutorialCharacterType;
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void OnEnable()
    {
        if(GameManager.Instance == null) return;
        EventManager.Instance.Subscribe(EventType.OnStartGame, OnStartGame);
    }
    private void OnStartGame(object[] obj)
    {
        boxCollider.enabled = true;
        if (GameManager.Instance) GameManager.Instance.EnableByBehaviour();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerCharacter>();
        if (player == null) return;
        var inputType = player.input._input;
        if (inputType == null) return;
        var playerType = player.GetCharType();

        if (sharedTutorial)
        {
            if (playerType == CharacterType.Cat) //Es Gato
            {
                if (inputType.inputType == Type.WASD) catBot.P1ChangeAnimation(type); //Es P1
                else catBot.P2ChangeAnimation(type); //Es P2
            }
            else if(playerType == CharacterType.Hamster)//Es Hamster
            {
                if (inputType.inputType == Type.WASD) hamsterBot.P1ChangeAnimation(type); //Es P1
                else hamsterBot.P2ChangeAnimation(type); //Es P2
            }
        }
        else if (playerType == singleTutorialCharacterType)
        {
            if (playerType == CharacterType.Cat) //Es Gato
            {
                if (inputType.inputType == Type.WASD) catBot.P1ChangeAnimation(type); //Es P1
                else catBot.P2ChangeAnimation(type); //Es P2
            }
            else if(playerType == CharacterType.Hamster)//Es Hamster
            {
                if (inputType.inputType == Type.WASD) hamsterBot.P1ChangeAnimation(type); //Es P1
                else hamsterBot.P2ChangeAnimation(type); //Es P2
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerCharacter>();
        if (player == null) return;
        var playerType = player.GetCharType();
        Debug.Log("FoundPlayer");
        if (sharedTutorial)
        {
            if (playerType == CharacterType.Cat) catBot.P1ChangeAnimation(TutorialBoxType.None); //Es Gato
            else if (playerType == CharacterType.Hamster) hamsterBot.P1ChangeAnimation(TutorialBoxType.None); //Es Hamster
        }
        else if(playerType == singleTutorialCharacterType)
        {
            if (playerType == CharacterType.Cat) catBot.P1ChangeAnimation(TutorialBoxType.None); //Es Gato
            else if (playerType == CharacterType.Hamster) hamsterBot.P1ChangeAnimation(TutorialBoxType.None); //Es Hamster
        }

    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe(EventType.OnStartGame, OnStartGame);
    }
}
