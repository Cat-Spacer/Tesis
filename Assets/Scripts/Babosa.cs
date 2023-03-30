using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Babosa : MonoBehaviour
{
    CustomMovement _player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject.GetComponent<CustomMovement>();

        if (player == null) return;

        _player = player;

        _player.transform.parent = transform;

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _player.transform.parent = null;

    }
}