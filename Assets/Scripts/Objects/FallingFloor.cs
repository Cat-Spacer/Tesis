using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFloor : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] LayerMask mask;
    [SerializeField] GameObject player;
    [SerializeField] float gravityScale;
    [SerializeField] RigidbodyConstraints2D constraints2D;
    [SerializeField] float delay;
    [SerializeField] bool activated = false;
    [SerializeField] Animator anim;
    [SerializeField] ParticleSystem fallingRocksParticle;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Activate()
    {
        SoundManager.instance.Pause(SoundManager.Types.CaveSpike);
        rb.constraints = constraints2D;
        rb.gravityScale = gravityScale;
        player.transform.SetParent(transform);
        anim.SetTrigger("Stop");
    }
    IEnumerator StartFalling()
    {
        yield return new WaitForSeconds(delay);
        Activate();
        fallingRocksParticle.Stop();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(End(5));
    }

    IEnumerator End(float time)
    {
        yield return new WaitForSeconds(time);
        player.transform.parent = null;
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((mask.value & (1 << collision.transform.gameObject.layer)) > 0  && !activated)
        {
            player = collision.gameObject;
            activated = true;
            StartCoroutine(StartFalling());
            fallingRocksParticle.Play();
            anim.SetTrigger("Fall");
            SoundManager.instance.Play(SoundManager.Types.CaveSpike);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((mask.value & (1 << collision.transform.gameObject.layer)) > 0 && !activated)
        {
            player = collision.gameObject;
        }
    }
}
