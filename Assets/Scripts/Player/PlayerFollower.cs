using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [SerializeField] private CustomMovement _customMovement;
    [SerializeField] private Vector3 _offset;
    void Start()
    {
        if (!_customMovement)
            _customMovement = FindObjectOfType<CustomMovement>();
        else
            Debug.LogWarning($"There is no player on scene for {name} to follow");
    }

    void Update()
    {
        transform.position = _customMovement.transform.position + _offset;
    }
}
