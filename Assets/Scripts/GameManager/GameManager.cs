using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    CustomMovement _player;

    private void Start()
    {
        _player = FindObjectOfType<CustomMovement>();
    }




}
