using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] InteractionColorsEnum interactionColors;
    [SerializeField] private CharacterType type;
    [SerializeField] private List<GameObject> _connectionObj;
    private List<IActivate> _connection  = new List<IActivate>();
    private Animator _anim;
    private string activateAnimation;
    private string desactivateAnimation;
    [SerializeField] private bool inverse;
    
    private Transform _target;
    [SerializeField] private GameObject arrow;
    
    
    void Start()
    {
        _anim = GetComponent<Animator>();
        foreach (var connection in _connectionObj)
        {
            var obj = connection.GetComponent<IActivate>();
            if(obj != null) _connection.Add(obj);
        }
        if (type == CharacterType.Cat) _target = GameManager.Instance.GetCat();
        else _target = GameManager.Instance.GetHamster();
        SetColor();
        _anim.Play(desactivateAnimation);
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

    void Activate()
    {
        _anim.Play(activateAnimation);
        SoundManager.instance.Play(SoundsTypes.PressureLever, gameObject);
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
        _anim.Play(desactivateAnimation);
        SoundManager.instance.Play(SoundsTypes.PressureLever, gameObject);
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
