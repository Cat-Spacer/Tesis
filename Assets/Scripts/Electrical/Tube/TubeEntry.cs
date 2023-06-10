using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeEntry : MonoBehaviour, IMouseOver
{
    [SerializeField] private Sprite _open, _closed;
    [SerializeField] private Tube _entryTube;
    [SerializeField] private float _searchRad = 1.0f;
    bool _isOpen;
    bool _isOutline;
    SpriteRenderer _sp;
    [SerializeField] Material outlineMat;
    Material defaultMat;

    private void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
        _sp.sprite = _closed;
        defaultMat = GetComponent<SpriteRenderer>().material;
        _isOutline = false;
        /*if (!_entryTube && Physics2D.OverlapCircle(transform.position, _searchRad).GetComponent<Tube>())
            _entryTube = Physics2D.OverlapCircle(transform.position, _searchRad).GetComponent<Tube>();*/
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CustomMovement>())
        {
            _isOpen = true;
            _sp.sprite = _open;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<CustomMovement>())
        {
            _isOpen = false;
            _sp.sprite = _closed;
        }
    }
    //private void OnMouseOver()
    //{
    //    if (Input.GetKeyDown(KeyCode.Mouse0))
    //    {
    //        FindObjectOfType<Hamster>().GetInTube(_entryTube.transform.position, _entryTube);
    //    }
    //}

    public void MouseOver()
    {
        if (_isOutline || !_isOpen) return;
        _sp.material = outlineMat;
        _isOutline = true;
    }

    public void MouseExit()
    {
        if (!_isOutline) return;
        _sp.material = defaultMat;
        _isOutline = false;
    }

    public void Interact()
    {
        FindObjectOfType<Hamster>().GetInTube(_entryTube.transform.position, _entryTube);
    }
}