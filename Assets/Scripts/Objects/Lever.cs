using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lever : MonoBehaviour, IInteract
{
    private Animator _anim;
    [SerializeField] private List<GameObject> _connectionObj;
    private List<IActivate> _connection = new List<IActivate>();
    [SerializeField] private bool _activated = false;
    
    void Start()
    {
        _anim = GetComponent<Animator>();
        foreach (var connection in _connectionObj)
        {
            var obj = connection.GetComponent<IActivate>();
            if(connection != null) _connection.Add(obj);
        }
        _anim.SetBool("Activate", _activated);
    }

    void PressLever()
    {
        if (!_activated)
        {
            foreach (var connection in _connection)
            {
                if(connection != null) connection.Activate();
            }
            _activated = true;
        }
        else
        {
            foreach (var connection in _connection)
            {
                if(connection != null) connection.Desactivate();
            }
            _activated = false;
        }
        SoundManager.instance.Play(SoundsTypes.Lever, gameObject);
        _anim.SetBool("Activate", _activated);
    }

    public void Interact(params object[] param)
    {
        PressLever();
    }

    public void ShowInteract(bool showInteractState)
    {

    }

}
