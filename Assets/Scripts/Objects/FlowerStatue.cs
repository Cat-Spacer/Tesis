using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerStatue : MonoBehaviour
{
    [SerializeField] private Transform _flowerPos;
    private Item _flower;
    private BoxCollider2D coll;

    private void Start()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    public void PutFlower(Item flower)
    {
        _flower = flower;
        _flower.HasPhysics(false);
        _flower.transform.parent = _flowerPos;
        _flower.transform.position = _flowerPos.position;
        coll.enabled = false;
        EventManager.Instance.Trigger(EventType.OnChangePeace, 1);
        SoundManager.instance.Play(SoundsTypes.Collect);
    }
}
