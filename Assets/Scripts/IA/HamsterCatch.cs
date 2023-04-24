using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterCatch : MonoBehaviour
{//agregar opcion de raycast
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Hamster>())
        {
            var hamster = collision.gameObject.GetComponent<Hamster>();
            if (hamster.InTube())
                hamster.HamsterCatched();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Hamster>())
        {
            var hamster = collision.gameObject.GetComponent<Hamster>();
            if (hamster.InTube())
                hamster.HamsterCatched();
        }
    }
}
