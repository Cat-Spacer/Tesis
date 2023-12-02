using System;
using UnityEngine;

public class ColliderSides : MonoBehaviour
{
    // Transform _cam;
    // Transform _player;
    // [SerializeField] bool isInY;
    // // [SerializeField] float _moveInX = 16;
    // [SerializeField] float _addInY = 2;
    // [SerializeField] int indexB;
    // [SerializeField] int indexA;
    // [SerializeField] Transform camPosA;
    // [SerializeField] Transform camPosB;
    //
    // Vector3 defaultpos;
    // public bool followPlayerInY = false;
    //
    // public Action _FollowPlayer = delegate { };
    //
    // public ColliderSides nextFollow;
    // private void Start()
    // {
    //     _cam = Camera.main.GetComponent<Transform>();
    // }
    //
    // public void ForceMove()
    // {
    //     var player = FindObjectOfType<CustomMovement>();
    //     if (player == null)
    //     {
    //         return;
    //     }
    //     _player = player.transform;
    //     Exit();
    //     _FollowPlayer = delegate { };
    //     GameManager.Instance.SetRespawnPoint(indexA);
    //     CheatManager.Instance.SetCurrentLevel(indexA);
    //     GameManager.Instance.GetCurrentLevel(indexA);
    //     _cam.position = camPosA.position;
    //     GameManager.Instance.PlayerDeath();
    // }
    // private void Update()
    // {
    //     _FollowPlayer();
    // }
    //
    // private void OnTriggerEnter2D(Collider2D trig)
    // {
    //     var player = trig.gameObject.GetComponent<CustomMovement>();
    //
    //     if (player == null) return;
    //
    //     _player = player.transform;
    //
    // }
    //
    // private void OnTriggerExit2D(Collider2D trig)
    // {
    //     var player = trig.gameObject.GetComponent<CustomMovement>();
    //
    //     if (player == null) return;
    //
    //     Exit();
    // }
    //
    // private void Exit()
    // {
    //     if (_player.position.x - gameObject.transform.position.x > 0 && !isInY)
    //     {
    //         defaultpos = _cam.position;
    //
    //         // _cam.position = new Vector3(gameObject.transform.position.x + _moveInX, _cam.position.y, _cam.position.z);
    //         _cam.position = camPosB.position;
    //         GameManager.Instance.SetRespawnPoint(indexB);
    //         CheatManager.Instance.SetCurrentLevel(indexB);
    //         GameManager.Instance.GetCurrentLevel(indexB);
    //
    //         if (followPlayerInY)
    //             _FollowPlayer = StartFollowPlayer;
    //     }
    //     else if (_player.position.x - gameObject.transform.position.x < 0 && !isInY)
    //     {
    //         //  _cam.position = new Vector3(gameObject.transform.position.x - _moveInX, _cam.position.y, _cam.position.z);
    //
    //         _cam.position = camPosA.position;
    //         GameManager.Instance.SetRespawnPoint(indexA);
    //         CheatManager.Instance.SetCurrentLevel(indexA);
    //         GameManager.Instance.GetCurrentLevel(indexA);
    //
    //         if (followPlayerInY)
    //             _FollowPlayer = EndFollowPlayer;
    //     }
    //     else if (_player.position.y - gameObject.transform.position.y > 0 && isInY)
    //     {
    //         // _cam.position = new Vector3(_cam.position.x, gameObject.transform.position.y + _moveInY, _cam.position.z);
    //         _cam.position = camPosA.position;
    //         Debug.Log(indexA);
    //         GameManager.Instance.SetRespawnPoint(indexA);
    //         CheatManager.Instance.SetCurrentLevel(indexA);
    //         GameManager.Instance.GetCurrentLevel(indexA);
    //     }
    //     else if (_player.position.y - gameObject.transform.position.y < 0 && isInY)
    //     {
    //         //_cam.position = new Vector3(_cam.position.x, gameObject.transform.position.y - _moveInY, _cam.position.z);
    //         // _cam.position = new Vector3(_cam.position.x, camPosB.position.y - _moveInY, _cam.position.z);
    //         _cam.position = camPosB.position;
    //         Debug.Log(indexB);
    //         GameManager.Instance.SetRespawnPoint(indexB);
    //         CheatManager.Instance.SetCurrentLevel(indexB);
    //         GameManager.Instance.GetCurrentLevel(indexB);
    //     }
    // }
    //
    // public void StartFollowPlayer()
    // {
    //     Vector3 newYPos = new Vector3(_cam.position.x, _player.position.y + _addInY, _cam.position.z);
    //
    //     var step = 2 * Time.deltaTime; // calculate distance to move
    //     _cam.position = Vector3.MoveTowards(_cam.position, newYPos, step);
    //
    //     // Check if the position of the cube and sphere are approximately equal.
    //     if (Vector3.Distance(_cam.position, newYPos) < 0.001f)
    //     {
    //         // Swap the position of the cylinder.
    //         _FollowPlayer = FollowPlayer;
    //     }
    // }
    //
    // public void EndFollowPlayer()
    // {
    //     //Vector3 newYPos = defaultpos;
    //     Vector3 newYPos = camPosA.position;
    //     var step = 2 * Time.deltaTime; // calculate distance to move
    //     _cam.position = Vector3.MoveTowards(_cam.position, newYPos, step);
    //
    //     // Check if the position of the cube and sphere are approximately equal.
    //     if (Vector3.Distance(_cam.position, newYPos) < 0.001f)
    //     {
    //         // Swap the position of the cylinder.
    //         _FollowPlayer = delegate { };
    //     }
    // }
    //
    // public void FollowPlayer()
    // {
    //     _cam.position = new Vector3(_cam.position.x, _player.position.y + _addInY, _cam.position.z);
    // }
}
