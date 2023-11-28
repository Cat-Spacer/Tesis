using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class HairBallBullet : MonoBehaviour
{
    [SerializeField] private float _speed, _timeToFall;
    private Rigidbody2D _rb;
    private BoxCollider2D _myColl;
    [SerializeField] private LayerMask _wallLayerMask;
    [SerializeField] private bool _stop = false;
    private GameObject _myFather;
    void Start()
    {
        _myColl = GetComponent<BoxCollider2D>();
        //_myColl.enabled = false;
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
        Physics2D.IgnoreCollision(_myFather.GetComponent<BoxCollider2D>(), _myColl, true);
        StartCoroutine(TimeUntilFall());
    }

    IEnumerator TimeUntilFall()
    {
        yield return new WaitForSeconds(_timeToFall);
        _rb.gravityScale = 1;
    }
    void Update()
    {
        if (_stop) return;
        _rb.velocity = new Vector2(_speed, _rb.velocity.y);
        //transform.position *= _speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == 6)
        {
            _stop = true;
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            Physics2D.IgnoreCollision(_myFather.GetComponent<BoxCollider2D>(), _myColl, false);
            //_myColl.enabled = true;
            Destroy(gameObject, 5);
            gameObject.layer = 6;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 6)
        {
            _stop = true;
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            _myColl.enabled = true;
            Destroy(gameObject, 5);
            gameObject.layer = 6;
        }
    }
    
    public HairBallBullet Set(GameObject father ,float dir)
    {
        _myFather = father;
        _speed *= dir;
        return this;
    }
}
