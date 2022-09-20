using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSides : MonoBehaviour
{
    Transform _cam;
    Transform _player;
    [SerializeField]bool isInY;
    [SerializeField] float _moveInX = 16.5f;
    [SerializeField] float _moveInY = 10.5f;
    [SerializeField] int indexB;
    [SerializeField] int indexA;
    private void Start()
    {
        _cam = Camera.main.GetComponent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D trig)
    {
        var player = trig.gameObject.GetComponent<CustomMovement>();

        if (player == null) return;

        _player = player.transform;

    }

    private void OnTriggerExit2D(Collider2D trig)
    {
        var player = trig.gameObject.GetComponent<CustomMovement>();

        if (player == null) return;

        if (_player.position.x - gameObject.transform.position.x > 0 && !isInY)
        {
            _cam.position = new Vector3(gameObject.transform.position.x + _moveInX, _cam.position.y, _cam.position.z);
            GameManager.Instance.SetRespawnPoint(indexB);
        }
        else if (_player.position.x - gameObject.transform.position.x < 0 && !isInY)
        {
            _cam.position = new Vector3(gameObject.transform.position.x - _moveInX, _cam.position.y, _cam.position.z);
            GameManager.Instance.SetRespawnPoint(indexA);
        }
        else if (_player.position.y - gameObject.transform.position.y > 0 && isInY)
        {
            _cam.position = new Vector3(_cam.position.x, gameObject.transform.position.y + _moveInY, _cam.position.z);
            GameManager.Instance.SetRespawnPoint(indexB);
        }
        else if (_player.position.y - gameObject.transform.position.y < 0 && isInY)
        {
            _cam.position = new Vector3(_cam.position.x, gameObject.transform.position.y - _moveInY, _cam.position.z);
            GameManager.Instance.SetRespawnPoint(indexA);
        }
    }
}
