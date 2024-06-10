using System.Collections;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SawNetwork : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("saw");
        var player = coll.gameObject.GetComponent<PlayerCharacterMultiplayer>();
        if (player == null) return;
        
        player.GetDamage();
    }
}
