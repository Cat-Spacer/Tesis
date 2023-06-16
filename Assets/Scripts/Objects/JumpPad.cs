using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour, IElectric
{
    [SerializeField] float jumpForce;
    [SerializeField] LayerMask mask;
    [SerializeField] ParticleSystem _particle;
    [SerializeField] bool _isOn;

    private void Start()
    {
        if (_isOn)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }

    public void TurnOff()
    {
        _particle.Stop();
        _isOn = false;
    }

    public void TurnOn()
    {
        _particle.Play();
        _isOn = true;
    }

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

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isOn) return;
        if ((mask.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            var player = collision.gameObject.GetComponent<CustomMovement>();
            player.CancelMovement();
            player.ResetDash();
            SoundManager.instance.Play(SoundManager.Types.Mushroom);
            var entityRb = collision.gameObject.GetComponent<Rigidbody2D>();
            entityRb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
