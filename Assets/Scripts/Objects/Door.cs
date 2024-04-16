using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IActivate
{
    [SerializeField] private Animator _anim;
    [SerializeField] private SpriteMask _mask;
    [SerializeField] private bool _noAnim = false;
    bool currentState;
    private BoxCollider2D _coll;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _coll = GetComponent<BoxCollider2D>();
    }

    public void ActivateDesactivate(bool active)
    {
        if (_noAnim)
        {
            gameObject.SetActive(active);
            return;
        }
        if (currentState == active) return;

        if (active)
        {
            _anim.SetTrigger("Open");
        }
        else
        {
            _anim.SetTrigger("Close");
        }
        currentState = active;
    }

    public void Activate()
    {
        ActivateDesactivate(true);
        _coll.gameObject.SetActive(false);
    }

    public void Desactivate()
    {
        ActivateDesactivate(false);
        _coll.gameObject.SetActive(true);
    }
}