using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lavel : MonoBehaviour
{
    [SerializeField] private Door[] _doors;
    [SerializeField] private GameObject[] _openDoors;
    [SerializeField] private LayerMask _TubesMask;
    [SerializeField] private float _searchRad = .05f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CustomMovement>())
        {
            foreach (Door door in _doors)
            {
                if (door.Obstacle)
                {
                    if (Physics2D.OverlapCircle(door.transform.position, _searchRad, _TubesMask).GetComponent<Tube>())
                    {
                        var tube = Physics2D.OverlapCircle(door.transform.position, _searchRad, _TubesMask).GetComponent<Tube>();
                        tube.CheckNeighborTubes();
                    }
                }
                door.ActivateDesactivate(false);
            }
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
            {
                if (Physics2D.OverlapCircle(go.transform.position, _searchRad, _TubesMask).GetComponent<Tube>())
                {
                    var tube = Physics2D.OverlapCircle(go.transform.position, _searchRad, _TubesMask).GetComponent<Tube>();
                    tube.CantPass();
                }
                go.SetActive(false);
            }
        }
    }
}
