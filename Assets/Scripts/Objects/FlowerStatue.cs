using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class FlowerStatue : MonoBehaviour
{
    [SerializeField] private Transform _flowerPos;
    private Item _flower;
    private BoxCollider2D coll;
    bool _hasFlower = false;
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject arrow;
    private void Start()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (_target == null || _hasFlower) return;
        var dir = _target.position - transform.position;
        if (dir.magnitude < 2)
        {
            arrow.SetActive(false);
        }
        else
        {
            arrow.SetActive(true);
            arrow.transform.up = _target.position - transform.position;
        }
    }

    public void PutFlower(Item flower)
    {
        arrow.SetActive(false);
        _hasFlower = true;
        _flower = flower;
        _flower.HasPhysics(false);
        _flower.transform.parent = _flowerPos;
        _flower.transform.position = _flowerPos.position;
        coll.enabled = false;
        EventManager.Instance.Trigger(EventType.OnPutFlower);
        SoundManager.instance.Play(SoundsTypes.Collect, gameObject);
    }
    public bool HasFlower() {return _hasFlower;}
}
