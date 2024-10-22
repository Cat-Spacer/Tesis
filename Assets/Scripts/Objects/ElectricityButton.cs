using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ElectricityButton : MonoBehaviour, IInteract
{
    private Animator _anim;
    [SerializeField] private List<GameObject> _connectionObj;
    private List<IActivate> _connection = new List<IActivate>();
    private bool _activated;

    void Start()
    {
        _anim = GetComponent<Animator>();
        foreach (var connection in _connectionObj)
        {
            var obj = connection.GetComponent<IActivate>();
            _connection.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PressButton()
    {
        if (!_activated)
        {
            foreach (var connection in _connection)
            {
                connection.Activate();
            }
            _activated = true;
        }
        else
        {
            foreach (var connection in _connection)
            {
                connection.Desactivate();
            }
            _activated = false;
        }
        _anim.SetTrigger("Press");
    }

    public void Interact(params object[] param)
    {
        PressButton();
    }

    public void ShowInteract(bool showInteractState)
    {

    }

    public InteractEnum GetInteractType()
    {
        return default;
    }
}
