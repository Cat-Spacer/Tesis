using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D trig)
    {
        CustomMovement player = trig.GetComponent<CustomMovement>();

        if (player == null) return;
        SoundManager.instance.Play(SoundManager.Types.Item);
        gameObject.SetActive(false);
    }
}
