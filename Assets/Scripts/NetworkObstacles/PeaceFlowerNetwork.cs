using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
public class PeaceFlowerNetwork : ItemNetwork
{
    private bool pickedUp = false;
    private Transform inventory;
    private void Update()
    {
        if (pickedUp)
        {
            Vector3 desiredPos = Vector3.Lerp(transform.position, inventory.position, .4f);
            _rb.MovePosition(desiredPos);
        }
    }

    public override void PickUp(PlayerCharacterMultiplayer player, bool pickUp)
    {
        if (pickUp)
        {
            HasPhysics(false);
            player.PickUp(this);
            inventory = player.GetInventoryTransform();
            pickedUp = pickUp;
        }
    }

    public void PutFlower()
    {
        HasPhysics(false);
        _rb.simulated = false;
        pickedUp = false;
    }
    public override void Drop(Vector2 dir, float dropForce)
    {
        Debug.Log("Drop");
        HasPhysics(true);
        _rb.AddForce(dir * dropForce);
        pickedUp = false;
    }
}
