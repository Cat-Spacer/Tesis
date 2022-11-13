using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveSpike : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] CaveSpikeBase spikeBase;
    [SerializeField] float damage;
    [SerializeField] float fallSpeed;
    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] Animator anim;

    [SerializeField] ParticleSystem brokenRockParticle;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }
    public void Activate()
    {
        Debug.Log("Me caigo");
        rb.gravityScale = fallSpeed;
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
                gameObject.SetActive(false);
            }
        }
        if ((groundLayerMask.value & (1 << collision.transform.gameObject.layer)) > 0) //Ground
        {
            SoundManager.instance.Play(SoundManager.Types.FallingDebris);
            var particle = Instantiate(brokenRockParticle);
            particle.transform.position = transform.position;
            gameObject.SetActive(false);
        }
    }
}
