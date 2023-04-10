using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] float jumpForce;
    [SerializeField] LayerMask mask;
    [SerializeField] Animator anim;
    [SerializeField] ParticleSystem sporesParticle;

    //[SerializeField] Sprite itemSprite;

    //public bool _cloneObject = false;

    //public void Clone(bool isClone)
    //{
    //    _cloneObject = isClone;
    //}

    //public bool GetBool()
    //{
    //    return _cloneObject;
    //}

    //public Sprite GetSprite()
    //{
    //    return itemSprite;
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((mask.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            var player = collision.gameObject.GetComponent<CustomMovement>();
            player.CancelMovement();
            var particle = Instantiate(sporesParticle);
            particle.transform.position = transform.position;
            SoundManager.instance.Play(SoundManager.Types.Mushroom);
            anim.SetTrigger("Interaction");
            var entityRb = collision.gameObject.GetComponent<Rigidbody2D>();
            entityRb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
