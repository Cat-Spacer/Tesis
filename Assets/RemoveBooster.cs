using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBooster : MonoBehaviour
{
    [SerializeField] CustomMovement _player;

    private void Start()
    {
        _player = FindObjectOfType<CustomMovement>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(CustomMovement.isDashing);

        if (collision.gameObject.layer == _player.gameObject.layer)
        {
            _player.RestartJumpValue();

        }
    }
}
