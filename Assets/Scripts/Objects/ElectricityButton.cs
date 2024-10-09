using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ElectricityButton : MonoBehaviour, IInteract
{
    [SerializeField] private List<GameObject> _connectionObj;
    private List<IActivate> _connection = new List<IActivate>();
    private bool _activated;

    void Start()
    {
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
            transform.localScale = new Vector3(0.25f, .7f, 1);
            _activated = true;
        }
        else
        {
            foreach (var connection in _connection)
            {
                connection.Desactivate();
            }
            transform.localScale = new Vector3(0.5f, .7f, 1);
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

    public InteractEnum GetInteractType()
    {
        return default;
    }
}
