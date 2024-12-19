using System;
using UnityEngine;

public class FallingFloor : MonoBehaviour
{
    private Action activateAction = delegate { };
    [SerializeField] LayerMask mask;
    [SerializeField] float _desactivateDelay, _activateDelay;
    [SerializeField]private SpriteRenderer[] _sp;
    [SerializeField] private Color activatedColor, desactivatedColor;
    private bool _isActive;
    [SerializeField] private BoxCollider2D _coll;
    [SerializeField] private BoxCollider2D _killZone;
    [SerializeField] private ParticleSystem _onFall;
    private float currentTime;
    private void Start()
    {
        _isActive = true;
    }

    private void Update()
    {
        activateAction();
    }

    private void Activate(bool activated)
    {
        if (activated)
        {
            SoundManager.instance.Play(SoundsTypes.Drop, gameObject);
            _onFall.Play();
            foreach (var item in _sp)
            {
                item.color = activatedColor;
            }
            _coll.enabled = true;
            _killZone.enabled = true;
            _isActive = true;
        }
        else
        {
            SoundManager.instance.Play(SoundsTypes.Drop, gameObject);
            _onFall.Play();
            foreach (var item in _sp)
            {
                item.color = desactivatedColor;
            }
            _coll.enabled = false;
            _killZone.enabled = false;
            _isActive = false;
        }
    }

    void CountdownDeactivateFloor()
    {
        currentTime += Time.deltaTime;
        if (!(currentTime >= _desactivateDelay)) return;
        Activate(false);
        currentTime = 0;
        activateAction = CountdownActivateFloor;
    }

    void CountdownActivateFloor()
    {
        currentTime += Time.deltaTime;
        if (!(currentTime >= _desactivateDelay)) return;
        Activate(true);
        currentTime = 0;
        activateAction = delegate { };
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((mask.value & (1 << collision.transform.gameObject.layer)) <= 0 || !_isActive) return;
        _isActive = false;
        activateAction = CountdownDeactivateFloor;
    }
}
