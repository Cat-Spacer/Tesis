using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosBTN : MonoBehaviour
{
    [SerializeField] private Transform _respawnPoint;
    [SerializeField] private GameObject _player;

    private void Awake()
    {
        if (_player == null)
            _player = FindObjectOfType<CustomMovement>().gameObject;
        if (_player == null)
            Debug.Log($"There's no player on scene");
    }

    public void ResetPlayerPos()
    {
        _player.transform.position = new Vector3(_respawnPoint.transform.position.x, _respawnPoint.transform.position.y, 0);
    }
}