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
    Vector3 rotationVector;
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(CustomMovement.isDashing);
        rotationVector = _player.transform.rotation.eulerAngles;
     

        if (collision.gameObject.layer == _player.gameObject.layer && CustomMovement.isDashing)
        {
            rotationVector.z = 90;
            _player.transform.rotation = Quaternion.Euler(rotationVector);

            _player.ForceDashEnd();
            Climb.isClimbing = true;
          /*  if (Climb.isClimbing)
            {*/
                Debug.Log("DASH AND CLIMB");
                Climb.startMirrorDash = transform.position;
                _player._climbScript._ClimbState = _player._climbScript.EndClimbForMirrorDash;
            /*}
            else
            {
                _player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 50);
            }*/
           
          
       
        }
    }

    /*public IEnumerator CoroutineWaitForRestart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        // Climb.isClimbing = false;

    }*/
}
