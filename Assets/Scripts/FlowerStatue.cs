using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerStatue : MonoBehaviour, IInteract
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
        PeaceSystem.instance.UpdatePeace(2);
    }

    public void Interact(params object[] param)
    {
        var player = param[0] as PlayerCharacter;
        var flower = player.GiveItem(ItemType.Flower);
        PutFlower(flower);
    }

    public void ShowInteract(bool showInteractState)
    {

    }
}