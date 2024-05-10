using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IActivate
{
    private BoxCollider2D _coll;
    [SerializeField] private bool _open;

    private void Start()
    {
        _coll = GetComponent<BoxCollider2D>();
        OpenClose();
    }

    void OpenClose()
    {
        if (!_open)
        {
            gameObject.SetActive(false);
            _coll.gameObject.SetActive(false);
            _open = true;
        }
        else
        {
            gameObject.SetActive(true);
            _coll.gameObject.SetActive(true);
            _open = false;
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