using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : Obstacle
{
    [SerializeField] private GameObject target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        target.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
