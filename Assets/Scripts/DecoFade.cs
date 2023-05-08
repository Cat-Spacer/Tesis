using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoFade : MonoBehaviour
{
    SpriteRenderer _sp;
    public float fadeOut;
    void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CustomMovement>())
        {
            _sp.color = new Color(1, 1, 1, fadeOut);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<CustomMovement>())
        {
            _sp.color = new Color(1, 1, 1, 1);
        }
    }
}
