using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoText : MonoBehaviour
{
    [SerializeField] GameObject _text;
    void Start()
    {
        _text.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CustomMovement>())
        {
            _text.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CustomMovement>())
        {
            _text.SetActive(false);
        }
    }
}
