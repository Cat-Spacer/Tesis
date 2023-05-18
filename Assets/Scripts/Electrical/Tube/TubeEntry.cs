using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeEntry : MonoBehaviour
{
    [SerializeField] Sprite _open, _closed;
    SpriteRenderer _sp;
    private void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
        _sp.sprite = _closed;
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
}
