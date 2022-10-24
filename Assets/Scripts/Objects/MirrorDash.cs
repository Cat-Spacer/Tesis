using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorDash : MonoBehaviour
{
    [SerializeField] CustomMovement _player;

    private void Start()
    {
        _player = FindObjectOfType<CustomMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _player.gameObject.layer)
        {
            _player.transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
            Climb.startMirrorDash = transform.position;
            Debug.Log("call ForceDashEnd");
            _player.ForceDashEnd();
           _player._climbScript._ClimbState = _player._climbScript.EndClimbForMirrorDash;
           // gameObject.SetActive(false);
        }
    }

    /*public IEnumerator CoroutineWaitForRestart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        // Climb.isClimbing = false;

    }*/
}
