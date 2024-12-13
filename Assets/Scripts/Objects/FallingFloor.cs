using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FallingFloor : MonoBehaviour
{
    [SerializeField] LayerMask mask;
    [SerializeField] float _desactivateDelay, _activateDelay;
    private SpriteRenderer _sp;
    [SerializeField] private Color activatedColor, desactivatedColor;
    private bool _isActive;
    [SerializeField] private BoxCollider2D _coll;
    [SerializeField] private BoxCollider2D _killZone;
    [SerializeField] private ParticleSystem _onFall;
    private void Start()
    {
        _sp = GetComponentInChildren<SpriteRenderer>();
        //_killZone = GetComponentInChildren<BoxCollider2D>();
        //_coll = GetComponent<BoxCollider2D>();
        _isActive = true;
    }
    private void Activate(bool activated)
    {
        if (activated)
        {
            SoundManager.instance.Play(SoundsTypes.Drop, gameObject);
            _onFall.Play();
            _sp.color = activatedColor;
            _coll.enabled = true;
            _killZone.enabled = true;
            _isActive = true;
        }
        else
        {
            SoundManager.instance.Play(SoundsTypes.Drop, gameObject);
            _onFall.Play();
            _sp.color = desactivatedColor;
            _coll.enabled = false;
            _killZone.enabled = false;
            _isActive = false;
            StartCoroutine(ActivateFloor());
        }
    }
    IEnumerator DesactivateFloor()
    {
        yield return new WaitForSeconds(_desactivateDelay);
        Activate(false);
    }
    IEnumerator ActivateFloor()
    {
        yield return new WaitForSeconds(_activateDelay);
        Activate(true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((mask.value & (1 << collision.transform.gameObject.layer)) > 0  && _isActive)
        {
            _isActive = false;
            StartCoroutine(DesactivateFloor());
        }
    }
}
