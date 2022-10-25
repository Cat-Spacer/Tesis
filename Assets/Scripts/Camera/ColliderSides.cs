using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ColliderSides : MonoBehaviour
{
    Transform _cam;
    Transform _player;
    [SerializeField]bool isInY;
    [SerializeField] float _moveInX = 16;
    [SerializeField] float _moveInY = 9;
    [SerializeField] int indexB;
    [SerializeField] int indexA;

    Vector3 defaultpos;
    public bool followPlayerInY = false;

    public Action _FollowPlayer = delegate { };

    public ColliderSides nextFollow;
    private void Start()
    {
        _cam = Camera.main.GetComponent<Transform>();
    }

    private void Update()
    {
        _FollowPlayer();
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
            defaultpos = _cam.position;

            _cam.position = new Vector3(gameObject.transform.position.x + _moveInX, _cam.position.y, _cam.position.z);
            GameManager.Instance.SetRespawnPoint(indexB);
            CheatManager.Instance.SetCurrentLevel(indexB);

            if (followPlayerInY)
                _FollowPlayer = StartFollowPlayer;
        }
        else if (_player.position.x - gameObject.transform.position.x < 0 && !isInY)
        {
          
             _cam.position = new Vector3(gameObject.transform.position.x - _moveInX, _cam.position.y, _cam.position.z);
           
                
            GameManager.Instance.SetRespawnPoint(indexA);
            CheatManager.Instance.SetCurrentLevel(indexA);

            if (followPlayerInY)
                _FollowPlayer = EndFollowPlayer;
        }
        else if (_player.position.y - gameObject.transform.position.y > 0 && isInY)
        {
            _cam.position = new Vector3(_cam.position.x, gameObject.transform.position.y + _moveInY, _cam.position.z);
            Debug.Log(indexA);
            GameManager.Instance.SetRespawnPoint(indexA);
            CheatManager.Instance.SetCurrentLevel(indexA);
        }
        else if (_player.position.y - gameObject.transform.position.y < 0 && isInY)
        {
            _cam.position = new Vector3(_cam.position.x, gameObject.transform.position.y - _moveInY, _cam.position.z);
            Debug.Log(indexB);
            GameManager.Instance.SetRespawnPoint(indexB);
            CheatManager.Instance.SetCurrentLevel(indexB);
        }

   
    }

    public void StartFollowPlayer()
    {
        Vector3 newYPos = new Vector3(_cam.position.x, _player.position.y + _moveInY, _cam.position.z);

        var step = 2 * Time.deltaTime; // calculate distance to move
        _cam.position = Vector3.MoveTowards(_cam.position, newYPos, step);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(_cam.position, newYPos) < 0.001f)
        {
            // Swap the position of the cylinder.
            _FollowPlayer = FollowPlayer;
        }
     
    }

    public void EndFollowPlayer()
    {
        Vector3 newYPos = defaultpos;
        var step = 2 * Time.deltaTime; // calculate distance to move
        _cam.position = Vector3.MoveTowards(_cam.position, newYPos, step);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(_cam.position, newYPos) < 0.001f)
        {
            // Swap the position of the cylinder.
            _FollowPlayer = delegate { };
        }

    }

    public void FollowPlayer()
    {
        Debug.Log("follow player");
        _cam.position = new Vector3(_cam.position.x, _player.position.y + _moveInY, _cam.position.z);
    }
}
