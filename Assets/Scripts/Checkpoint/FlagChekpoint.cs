using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FlagChekpoint : MonoBehaviour
{
    [SerializeField] private Vector2 _boxArea;    
    [SerializeField] private LayerMask _players;
    private Animator _anim;
    private bool _isOn = false;
    void Start()
    {
        _anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (_isOn) return;
        var coll = Physics2D.OverlapBox(transform.position, _boxArea, 0, _players);
        if (coll != null)
        {
            Debug.Log("FlagOn");
            _isOn = true;
            _anim.SetTrigger("ON");
            GameManager.Instance.SetRespawnPoint(transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, _boxArea);
    }

}
