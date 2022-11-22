using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] GameObject _screen;
    bool started = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (started) return;

        var player = collision.GetComponent<CustomMovement>();
        if (player == null) return;
        _screen.SetActive(true);
        started = true;
    }
}
