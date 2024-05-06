using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("saw");
        var player = coll.gameObject.GetComponent<PlayerCharacter>();
        if (player == null) return;
        
        player.GetDamage();
    }
}
