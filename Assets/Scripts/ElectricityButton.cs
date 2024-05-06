using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ElectricityButton : MonoBehaviour, IInteract
{
    [SerializeField] private GameObject _connectionObj;
    private IActivate _connection;
    private bool _activated;
    
    [SerializeField] SpriteRenderer _sp;
    [SerializeField] private Color _pressedColor, _normalColor;
    
    void Start()
    {
        _connection = _connectionObj.GetComponent<IActivate>();
        //_sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PressButton()
    {
        if (!_activated)
        {
            _connection.Activate();
            transform.localScale = new Vector3(0.25f, .7f, 1);
            _sp.material.color = _pressedColor;
            _activated = true;
        }
        else
        {
            _connection.Desactivate();
            transform.localScale = new Vector3(0.5f, .7f, 1);
            _sp.material.color = _normalColor;
            _activated = false;
        }
    }

    public void Interact(params object[] param)
    {
        PressButton();
    }

    public void ShowInteract(bool showInteractState)
    {

    }
}
