using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _myRigidbody;
    [SerializeField] private Animator _myAnimator;
    [SerializeField] private Transform _target, _guardPoint;
    [SerializeField] private string _chaseParameterName = "IsMoving"/*, _targetName = "Hamster"*/;
    [SerializeField] private float _movementSpeed = 2;
    [SerializeField] private bool _canChase = false;


    private void Start()
    {
        if (_myRigidbody == null)
            _myRigidbody = GetComponentInParent<Rigidbody2D>();
        if (_myAnimator == null)
            _myAnimator = GetComponentInParent<Animator>();
        /*if (target == null)
            target = GameObject.Find(targetName).transform;*/
    }

    void FixedUpdate()
    {
        if (_canChase == true)
        {
            if (_myAnimator != null)
            {
                _myAnimator.SetBool("IsMoving", true);
                _myAnimator.SetFloat("Horizontal", (_target.position.x - transform.position.x));
                _myAnimator.SetFloat("Vertical", (_target.position.y - transform.position.y));
            }

            Vector2 vectorToTarget = _target.position - transform.position;
            _myRigidbody.MovePosition(_myRigidbody.position + vectorToTarget.normalized * _movementSpeed * Time.fixedDeltaTime);
        }
        else if (_guardPoint != null)
            _myRigidbody.position = Vector2.MoveTowards(_myRigidbody.position, _guardPoint.transform.position, 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D collision) // if has line of sign attacks and chase
    {
        if (_target == null)
            _target = collision.gameObject.transform;
        //-------------- Chase condition ------------//
        if (_myAnimator != null)
            _myAnimator.SetBool(_chaseParameterName, true);

        if (collision.gameObject.GetComponent<Hamster>())
        {
            var hamster = collision.gameObject.GetComponent<Hamster>();
            if (hamster.InTube())
                _canChase = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)  // if dosnt trigger
    {
        if (_myAnimator != null)
            _myAnimator.SetBool(_chaseParameterName, false);
        _canChase = false;
        _target = null;
    }
}