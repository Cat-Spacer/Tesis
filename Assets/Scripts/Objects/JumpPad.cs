using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private bool _isOn;
    private LineRenderer _myLineConnection;
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
        //_particle.Stop();
        _isOn = false;
    }

    public void TurnOn()
    {
        //_particle.Play();
        _isOn = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isOn) return;
        if ((_mask.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            var player = collision.gameObject.GetComponent<PlayerCharacter>();
            player.StopMovement();
            SoundManager.instance.Play(SoundsTypes.Mushroom, false);
            var entityRb = collision.gameObject.GetComponent<Rigidbody2D>();
            entityRb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}