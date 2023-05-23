using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Hamster>())
        {
            var hamster = collision.gameObject.GetComponent<Hamster>();
            if (hamster == null)
            {
                Debug.Log("Hamster is null");
                return;
            }
            hamster.AddEnergy(1);
            gameObject.SetActive(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Hamster>())
        {
            var hamster = collision.gameObject.GetComponent<Hamster>();
            if (hamster == null)
            {
                Debug.Log("Hamster is null");
                return;
            }
            hamster.AddEnergy(1);
            gameObject.SetActive(false);
        }
    }
}