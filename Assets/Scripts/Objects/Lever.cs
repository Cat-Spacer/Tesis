using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lever : MonoBehaviour, IInteract
{
    [SerializeField] private CharacterType type;
    private Animator _anim;
    [SerializeField] private List<GameObject> _connectionObj;
    private List<IActivate> _connection = new List<IActivate>();
    [SerializeField] private bool _activated = false;
    private Transform _target;
    [SerializeField] private GameObject arrow;
    [SerializeField] ParticleSystem greenParticles;
    [SerializeField] ParticleSystem redParticles;
    
    void Start()
    {
        _anim = GetComponent<Animator>();
        foreach (var connection in _connectionObj)
        {
            var obj = connection.GetComponent<IActivate>();
            if(connection != null) _connection.Add(obj);
        }
        _anim.SetBool("Activate", _activated);
        if (type == CharacterType.Cat) _target = GameManager.Instance.GetCat();
        else _target = GameManager.Instance.GetHamster();
    }
    private void Update()
    {
        if (_target == null) return;
        var dir = _target.position - transform.position;
        if (dir.magnitude < 2)
        {
            arrow.SetActive(false);
        }
        else
        {
            arrow.SetActive(true);
            arrow.transform.up = _target.position - transform.position;
        }
    }


    void PressLever()
    {
        if (!_activated)
        {
            foreach (var connection in _connection)
            {
                if(connection != null) connection.Activate();
            }
            greenParticles.Play();
            _activated = true;
        }
        else
        {
            foreach (var connection in _connection)
            {
                if(connection != null) connection.Desactivate();
            }
            _activated = false;
            redParticles.Play();
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
