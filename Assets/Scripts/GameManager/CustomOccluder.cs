using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomOccluder : MonoBehaviour
{
    [SerializeField] private bool _firstLevel;
    [SerializeField] private GameObject[] _neightborLvls, _restOfLvls;

    private void Awake()
    {
        
    }
    private void Start()
    {
        Invoke("ArtificialStart", 2);
    }
    private void ArtificialStart()
    {
        if (_firstLevel)
            for (int i = 0; i < _restOfLvls.Length; i++)
                _restOfLvls[i].SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CustomMovement>())// is player collision
            for (int i = 0; i < _neightborLvls.Length; i++)
                _neightborLvls[i].SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<CustomMovement>())// is player collision
            for (int i = 0; i < _restOfLvls.Length; i++)
                _restOfLvls[i].SetActive(false);
    }
}
