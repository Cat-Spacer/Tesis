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
    private string activateAnimation;
    private string desactivateAnimation;
    [SerializeField] InteractionColorsEnum interactionColors;
    void Start()
    {
        _anim = GetComponent<Animator>();
        foreach (var connection in _connectionObj)
        {
            var obj = connection.GetComponent<IActivate>();
            if(connection != null) _connection.Add(obj);
        }
        if (type == CharacterType.Cat) _target = GameManager.Instance.GetCat();
        else _target = GameManager.Instance.GetHamster();
        SetColor();
        if(_activated)_anim.Play(activateAnimation);
        else _anim.Play(desactivateAnimation);
    }
    void SetColor()
    {
        switch (interactionColors)
        {
            case InteractionColorsEnum.Orange:
                activateAnimation = "ActivateOrange";
                desactivateAnimation = "DeactivateOrange";
                break;
            case InteractionColorsEnum.Yellow:
                activateAnimation = "ActivateYellow";
                desactivateAnimation = "DeactivateYellow";
                break;
            case InteractionColorsEnum.BlackCyan:
                activateAnimation = "ActivateBlackCyan";
                desactivateAnimation = "DeactivateBlackCyan";
                break;
            case InteractionColorsEnum.Pink:
                activateAnimation = "ActivatePink";
                desactivateAnimation = "DeactivatePink";
                break;
            case InteractionColorsEnum.Grey:
                activateAnimation = "ActivateGrey";
                desactivateAnimation = "DeactivateGrey";
                break;
        }
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
            _anim.Play(activateAnimation);
        }
        else
        {
            foreach (var connection in _connection)
            {
                if(connection != null) connection.Desactivate();
            }
            _activated = false;
            redParticles.Play();
            _anim.Play(desactivateAnimation);
        }
        SoundManager.instance.Play(SoundsTypes.Lever, gameObject);
    }

    public void Interact(params object[] param)
    {
        PressLever();
    }

    public void ShowInteract(bool showInteractState)
    {

    }

}
