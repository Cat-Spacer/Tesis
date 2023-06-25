using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OwlAttack : MonoBehaviour
{
    Action _SearchAction = delegate { };
    [SerializeField] Vector2 _range;
    [SerializeField] Owl _owl;
    [SerializeField] LayerMask _hamsterMask;


    void Start()
    {
        _owl = GetComponentInChildren<Owl>();
        _SearchAction = Search;
        EventManager.Instance.Subscribe("PlayerDeath", ResetPosition);
    }

    // Update is called once per frame
    void Update()
    {
        _SearchAction();
    }
    void Search()
    {
        var hamster = Physics2D.OverlapBox(transform.position, _range, 0, _hamsterMask);
        if (hamster)
        {
            Debug.Log("In Range");
            _owl.SetTarget(hamster.gameObject);
            _SearchAction = delegate { };
        }
    }
    public void ResetAttack()
    {
        _SearchAction = Search;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, _range);
    }
    void ResetPosition(params object[] param)
    {
        _SearchAction = delegate { };
        StartCoroutine(WaitForReanim());
    }

    IEnumerator WaitForReanim()
    {
        yield return new WaitForSeconds(3.5f);
        _SearchAction = Search;
    }
}
