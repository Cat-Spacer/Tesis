using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveSpike : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float damage;
    [SerializeField] float fallSpeed;
    [SerializeField] float delay;
    [SerializeField] bool firstTouch = false;
    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] Vector2 activationRange;
    [SerializeField] Transform activationRangeTransform;
    [SerializeField] Animator anim;

    [SerializeField] ParticleSystem brokenRockParticle;

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

        anim.SetTrigger("Fall");
    }
    void Activate()
    {
        anim.SetTrigger("Stop");
        rb.gravityScale = fallSpeed;
    }
    IEnumerator StartFalling()
    {
        yield return new WaitForSeconds(delay);
        Activate();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((playerLayerMask.value & (1 << collision.transform.gameObject.layer)) > 0) //Player
        {
            var damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.GetDamage(1);
                var particle = Instantiate(brokenRockParticle);
                particle.transform.position = transform.position;
                Destroy(gameObject);
            }
        }
        if ((groundLayerMask.value & (1 << collision.transform.gameObject.layer)) > 0) //Ground
        {
            SoundManager.instance.Play(SoundManager.Types.FallingDebris);
            var particle = Instantiate(brokenRockParticle);
            particle.transform.position = transform.position;
            Destroy(gameObject);
            Debug.Log("Piso");
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(activationRangeTransform.position, activationRange);
    }
}
