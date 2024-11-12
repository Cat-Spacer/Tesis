using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private CharacterType type;
    [SerializeField] private List<GameObject> _connectionObj;
    private List<IActivate> _connection  = new List<IActivate>();
    private Animator _anim;
    private string activateAnimation;
    private string desactivateAnimation;
    [SerializeField] private bool inverse;
    
    void Start()
    {
        _anim = GetComponent<Animator>();
        foreach (var connection in _connectionObj)
        {
            var obj = connection.GetComponent<IActivate>();
            if(obj != null) _connection.Add(obj);
        }
    }
    void Activate()
    {
        _anim.Play("Activate");
        foreach (var connection in _connection)
        {
            if(connection != null)
            {
                if(!inverse) connection.Desactivate();
                else connection.Activate();
            }
        }
    }

    void Desactivate()
    {
        _anim.Play("Desactivate");
        foreach (var connection in _connection)
        {
            if(connection != null)
            {
                if(!inverse) connection.Activate();
                else connection.Desactivate();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerCharacter>();
        if (player != null && player.GetCharType() == type)
        {
            Activate();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerCharacter>();
        if (player != null && player.GetCharType() == type)
        {
            Desactivate();
        }
    }
}
