
using UnityEngine;

public class FloorSwitch : MonoBehaviour, IActivate
{
    private BoxCollider2D _coll;
    [SerializeField] private bool _activated;
    private SpriteRenderer _sp;
    [SerializeField] private Color activatedColor, desactivatedColor;

    [SerializeField] private float speedChange;
    
    private void Awake()
    {
        _coll = GetComponent<BoxCollider2D>();
    }
    
    private void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
        if (_activated)
        {
            _sp.color = desactivatedColor;
            _coll.enabled = false;
            _activated = false;
        }
        else
        {
            _sp.color = activatedColor;
            _coll.enabled = true;
            _activated = true;
        }
    }
    void OpenClose()
    {
        if (_activated)
        {
            _sp.color = desactivatedColor;
            _coll.enabled = false;
            _activated = false;
        }
        else
        {
            _sp.color = activatedColor;
            _coll.enabled = true;
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