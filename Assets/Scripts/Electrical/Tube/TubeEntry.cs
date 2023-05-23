using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeEntry : MonoBehaviour
{
    [SerializeField] private Sprite _open, _closed;
    [SerializeField] private Tube _entryTube;
    [SerializeField] private float _searchRad = 1.0f;
    SpriteRenderer _sp;

    private void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
        _sp.sprite = _closed;

        /*if (!_entryTube && Physics2D.OverlapCircle(transform.position, _searchRad).GetComponent<Tube>())
            _entryTube = Physics2D.OverlapCircle(transform.position, _searchRad).GetComponent<Tube>();*/
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CustomMovement>())
        {
            _sp.sprite = _open;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<CustomMovement>())
        {
            _sp.sprite = _closed;
        }
    }
    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FindObjectOfType<Hamster>().GetInTube(_entryTube.transform.position, _entryTube);
        }
    }
}