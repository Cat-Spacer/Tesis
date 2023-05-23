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
            if (!_obstacle)
                if (hamster.InTube())
                    hamster.ReturnToCat();
            else
                if (hamster.InTube())
                    hamster.MoveToNextTube(hamster.LastTube);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.GetComponent<Hamster>()) return;

            var hamster = collision.gameObject.GetComponent<Hamster>();
            if (!_obstacle)
                if (hamster.InTube())
                    hamster.ReturnToCat();
            else
                if (hamster.InTube())
                    hamster.MoveToNextTube(hamster.LastTube);
    }
}