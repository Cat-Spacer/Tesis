using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FlowerStatueNetwork : NetworkBehaviour, IInteract
{
    [SerializeField] private Transform _flowerPos;
    private ItemNetwork _flower;
    private BoxCollider2D coll;

    private void Start()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    public void PutFlower(ItemNetwork flower)
    {
        if (flower == null) return;
        _flower = flower;
        var flowerObj = _flower.GetComponent<PeaceFlowerNetwork>();
        if (flowerObj == null) return;
        flowerObj.PutFlower();
        _flower.transform.position = _flowerPos.position;
        coll.enabled = false;
        //PeaceSystem.instance.UpdatePeace(2);
    }

    public void Interact(params object[] param)
    {
        var obj = (GameObject)param[0];
        var player = obj.GetComponent<PlayerCharacterMultiplayer>();
        var flower = player.GiveItem(ItemTypeNetwork.Flower);
        if(flower != null) PutFlower(flower);
    }

    public void ShowInteract(bool showInteractState)
    {

    }
}
