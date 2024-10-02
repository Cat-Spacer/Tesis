using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDoor : MonoBehaviour
{
    [SerializeField] private CharacterType playerType;
    private BoxCollider2D _coll;
    private Animator _anim;
    [SerializeField] private Transform playerPosition;
    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void Open()
    {
        _anim.Play("Open_Door");
    }

    public void Close()
    {
        _anim.Play("Close_Door");
    }

    public Transform GetPlayerPosition()
    {
        return playerPosition;
    }
    public CharacterType GetPlayerType()
    {
        return playerType;
    }
}