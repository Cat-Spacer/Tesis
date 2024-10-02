using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teletransporter : MonoBehaviour
{
    [SerializeField] private bool canTp;
    [SerializeField] private Teletransporter connection;
    [SerializeField] private Transform catPos, hamsterPos;
    private GameObject catFilter, hamsterFilter; 

    private void Teletransport(PlayerCharacter player)
    {
        player.Teletransport();
        if (player.GetCharType() == CharacterType.Cat) connection.TpCat(player.gameObject);
        else connection.TpHamster(player.gameObject);
    }

    void TpCat(GameObject player)
    {
        catFilter = player;
        player.transform.position = catPos.position;
    }

    void TpHamster(GameObject player)
    {
        hamsterFilter = player;
        player.transform.position = hamsterPos.position;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.gameObject.GetComponent<PlayerCharacter>();
        if (player != null && canTp && col.gameObject != catFilter && col.gameObject != hamsterFilter)
        {
            Teletransport(player);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var player = other.gameObject.GetComponent<PlayerCharacter>();
        if (player.GetCharType() == CharacterType.Cat) catFilter = null;
        else hamsterFilter = null;
    }
}
