using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lavel : MonoBehaviour
{
    [SerializeField] private Door[] _doors;
    [SerializeField] private GameObject[] _openDoors;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*Debug.Log($"Trigerrie con {collision.gameObject.name}");
        Debug.Log($"CustomMovement es {collision.gameObject.GetComponent<CustomMovement>()}");*/
        if (collision.gameObject.GetComponent<CustomMovement>())
        {
            foreach (Door door in _doors)
                door.ActivateDesactivate(false);
            foreach (GameObject go in _openDoors)
                go.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CustomMovement>())
        {
            foreach (Door door in _doors)
                door.ActivateDesactivate(true);
            foreach (GameObject go in _openDoors)
                go.SetActive(false);
        }
    }
}
