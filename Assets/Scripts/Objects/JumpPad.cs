using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour, IElectric
{
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private bool _isOn;
    [SerializeField] GameObject _connectionSource;
    private LineRenderer _myLineConnection;
    private IGenerator _myGen;
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

    public Transform ConnectionSource()
    {
        return _connectionSource.transform;
    }

    public void SetGenerator(IGenerator gen, LineRenderer line)
    {
        _myGen = gen;
        _myLineConnection = line;
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
        if (!_isOn) return;
        if ((_mask.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            var player = collision.gameObject.GetComponent<CustomMovement>();
            player.ForceStopMovement();
            player.RestartJumpValue();
            player.ResetDash();
            SoundManager.instance.Play(SoundManager.Types.Mushroom, false);
            var entityRb = collision.gameObject.GetComponent<Rigidbody2D>();
            entityRb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

}