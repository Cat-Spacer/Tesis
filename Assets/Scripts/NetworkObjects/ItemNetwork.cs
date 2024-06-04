using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ItemTypeNetwork
{
    Flower
}
public class ItemNetwork : NetworkBehaviour
{
    public ItemTypeNetwork type;
    private BoxCollider2D _boxCollider;
    protected Rigidbody2D _rb;
    public virtual void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }
    
    public virtual void PickUp(PlayerCharacterMultiplayer player, bool pickUp)
    {
        if (pickUp)
        {
            HasPhysics(false);
            player.PickUp(this);
        }
    }
    public virtual void Drop(Vector2 dir, float dropForce)
    {
        Debug.Log("Drop");
        HasPhysics(true);
        _rb.AddForce(dir * dropForce);
    }

    public void HasPhysics(bool has)
    {
        _boxCollider.enabled = has;
        //_rb.simulated = has;
    }
    public ItemTypeNetwork Type()
    {
        return type;
    }
}
