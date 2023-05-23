using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterCatch : MonoBehaviour
{
    [SerializeField] private bool _obstacle = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponent<Hamster>()) return;

        var hamster = collision.gameObject.GetComponent<Hamster>();
        if (hamster.InTube())
            if (!_obstacle)
                hamster.ReturnToCat();
            else
                hamster.MoveToNextTube(hamster.LastTube);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.GetComponent<Hamster>()) return;

        var hamster = collision.gameObject.GetComponent<Hamster>();
        if (hamster.InTube())
            if (!_obstacle)
                hamster.ReturnToCat();
            else
                hamster.MoveToNextTube(hamster.LastTube);
    }
}