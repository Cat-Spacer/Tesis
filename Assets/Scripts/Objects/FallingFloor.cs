using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFloor : MonoBehaviour
{
    [SerializeField] LayerMask mask;
    [SerializeField] float _desactivateDelay, _activateDelay;
    private SpriteRenderer _sp;
    private BoxCollider2D _coll;
    [SerializeField] private Color activatedColor, desactivatedColor;
    private bool _isActive;
    private void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
        _coll = GetComponent<BoxCollider2D>();
        _isActive = true;
    }

    private void Activate(bool activated)
    {
        //gameObject.SetActive(activated);
        if (activated)
        {
            _sp.color = activatedColor;
            _coll.enabled = true;
            _isActive = true;
        }
        else
        {
            _sp.color = desactivatedColor;
            _coll.enabled = false;
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
