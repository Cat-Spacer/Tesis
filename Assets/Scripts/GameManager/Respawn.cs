using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] Transform _respawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<CustomMovement>();

        if (player == null) return;

        player.transform.position = new Vector3(_respawnPoint.transform.position.x, _respawnPoint.transform.position.y, 0);
    }
}