using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private SpriteMask _mask;
    [SerializeField] private bool _noAnim = false;
    bool currentState;
    private void Start()
    {
        _anim = GetComponent<Animator>();
    }
    public void ActivateDesactivate(bool active)
    {
        if (_noAnim)
        {
            gameObject.SetActive(false);
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
}
