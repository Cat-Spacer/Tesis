using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveSpike : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float damage;
    [SerializeField] float fallSpeed;
    [SerializeField] float delay;
    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] Vector2 activationRange;
    [SerializeField] Transform activationRangeTransform;
    [SerializeField] Animator anim;

    bool activate;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }
    private void Update()
    {
        CheckPlayer();
    }
    void CheckPlayer()
    {
        Collider2D coll = Physics2D.OverlapBox(activationRangeTransform.position, activationRange, 0, playerLayerMask);
        if (coll == null) return;
        StartCoroutine(StartFalling());
        SoundManager.instance.Play(SoundManager.Types.CaveSpike);
        anim.SetTrigger("Fall");
    }
    void Activate()
    {
        anim.SetTrigger("Stop");
        SoundManager.instance.Pause(SoundManager.Types.CaveSpike);
        rb.gravityScale = fallSpeed;
    }
    IEnumerator StartFalling()
    {
        yield return new WaitForSeconds(delay);
        Activate();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((playerLayerMask.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            Debug.Log("Player");
            var damageable = collision.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.GetDamage(1);
                Debug.Log("KillPlayer");
                Destroy(gameObject);
            }
        }
        if ((groundLayerMask.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            Debug.Log("Ground");
            Destroy(gameObject);
        }     
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(activationRangeTransform.position, activationRange);
    }
}
