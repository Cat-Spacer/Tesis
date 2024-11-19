
using System;
using UnityEngine;

public class FloorSwitch : MonoBehaviour, IActivate
{
    private BoxCollider2D _coll;
    [SerializeField] private bool _activated;
    private SpriteRenderer _sp;
    [SerializeField] private Color activatedColor, desactivatedColor;

    [SerializeField] private float speedChange;
    [SerializeField] private BoxCollider2D killZone;
    private void Awake()
    {
        _sp = GetComponent<SpriteRenderer>();
        _coll = GetComponent<BoxCollider2D>();
    }
    
    private void Start()
    {

        if (_activated)
        {
            _sp.color = desactivatedColor;
            _coll.enabled = false;
            killZone.enabled = false;
            _activated = false;
        }
        else
        {
            _sp.color = activatedColor;
            _coll.enabled = true;
            killZone.enabled = true;
            _activated = true;
        }
    }
    void OpenClose()
    {
        if (_activated)
        {
            _sp.color = desactivatedColor;
            _coll.enabled = false;
            killZone.enabled = false;
            _activated = false;
        }
        else
        {
            _sp.color = activatedColor;
            _coll.enabled = true;
            killZone.enabled = true;
            _activated = true;
        }
    }
    public void Activate()
    {
        OpenClose();
    }

    public void Desactivate()
    {
        OpenClose();
    }
}