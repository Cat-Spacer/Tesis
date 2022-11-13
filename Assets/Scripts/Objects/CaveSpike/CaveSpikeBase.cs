using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveSpikeBase : MonoBehaviour
{
    [SerializeField] CaveSpike spike;
    [SerializeField] Vector2 activationRange;
    [SerializeField] Transform activationRangeTransform;
    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] float timeBeforeFalling;
    [SerializeField] bool alreadyFalling;
    private void Update()
    {
        CheckPlayer();
    }
    void CheckPlayer()
    {
        Collider2D coll = Physics2D.OverlapBox(activationRangeTransform.position, activationRange, 0, playerLayerMask);
        if (coll == null) return;

        if (!alreadyFalling)
        {
            StartCoroutine(StartFalling());
            alreadyFalling = true;
        }
    }
    IEnumerator StartFalling()
    {
        yield return new WaitForSeconds(timeBeforeFalling);
        spike.Activate();
    }
    public void ResetSpike()
    {
        alreadyFalling = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(activationRangeTransform.position, activationRange);
    }
}
