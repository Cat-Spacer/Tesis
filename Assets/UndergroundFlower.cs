using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergroundFlower : WindObstacles
{
    [SerializeField] private float _windForce;

    protected override void OnTriggerStay2D(Collider2D other)
    {
        if (other != _player.GetComponent<Collider2D>()) return;

        Wind(transform.up, _windForce);

    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }
    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
    }
}
