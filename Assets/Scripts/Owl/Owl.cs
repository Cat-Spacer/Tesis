using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Owl : MonoBehaviour
{
    Action _AttackAction = delegate { };
    [SerializeField] float _speed;
    [SerializeField] GameObject target;
    [SerializeField] Transform _claw, _resetPos;
    OwlAttack _parent;
    private SpriteRenderer _sp;
    void Start()
    {
        _parent = GetComponentInParent<OwlAttack>();
        transform.position = _resetPos.position;
        _sp = GetComponent<SpriteRenderer>();
        EventManager.Instance.Subscribe("PlayerDeath", ResetPosition);
    }

    // Update is called once per frame
    void Update()
    {
        _AttackAction();
    }
    void Attack()
    {
        var newDir = target.transform.position - _claw.transform.position;
        // if (newDir.x > 0) _sp.flipX = true;
        // else _sp.flipX = true;
        if (newDir.x > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
        else transform.rotation = Quaternion.Euler(0, 180, 0);;
        transform.position += newDir.normalized * _speed * Time.deltaTime;
        if (Vector3.Distance(_claw.position, target.transform.position) < 0.1f)
        {
            _AttackAction = FlyAway;
            target.GetComponent<Hamster>().OwlCatch(1);
            StartCoroutine(ResetAttack());
        }   
    }
    void FlyAway()
    {
        target.transform.position = _claw.position;
        transform.position += Vector3.up * _speed * Time.deltaTime;       
    }
    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(3.5f);
        transform.position = _resetPos.position;
        _AttackAction = delegate { };
        _parent.ResetAttack();
    }
    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
        _AttackAction = Attack;
    }

    void ResetPosition(params object[] param)
    {
        transform.position = _resetPos.position;
        _parent.ResetAttack();
        _AttackAction = delegate { };
    }
}
