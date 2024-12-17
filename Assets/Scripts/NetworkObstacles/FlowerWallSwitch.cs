using System;
using UnityEngine;

public class FlowerWallSwitch : MonoBehaviour, IActivate
{
    private BoxCollider2D _coll;
    [SerializeField] private bool _wallIsActive;
    [SerializeField] private SpriteRenderer[] _sp;
    [SerializeField] private Color activatedColor, desactivatedColor;

    //[SerializeField] private float speedChange;
    [SerializeField] private BoxCollider2D colliderZone;
    [SerializeField] private ParticleSystem _myparticle;
    private void Awake()
    {
      //  _sp = GetComponent<SpriteRenderer>();
        _coll = GetComponent<BoxCollider2D>();
    }
    
    private void Start()
    {
        if (!_wallIsActive)
        {

            foreach (var item in _sp)
            {
                item.color = desactivatedColor;
            }
          
            _coll.enabled = false;
            colliderZone.enabled = false;
            _wallIsActive = false;
        }
        else
        {
            foreach (var item in _sp)
            {
                item.color = activatedColor;
            }
            _coll.enabled = true;
            colliderZone.enabled = true;
            _wallIsActive = true;
        }
    }
    void OpenClose()
    {
        if (_wallIsActive)
        {
            foreach (var item in _sp)
            {
                item.color = desactivatedColor;
            }
            _coll.enabled = false;
            colliderZone.enabled = false;
            _wallIsActive = false;
        }
        else
        {
            foreach (var item in _sp)
            {
                item.color = activatedColor;
            }
            _coll.enabled = true;
            colliderZone.enabled = true;
            _wallIsActive = true;
        }
    }
    public void Activate()
    {
        SoundManager.instance.Play(SoundsTypes.Magic, gameObject);
        if(_myparticle) _myparticle.Play();
        OpenClose();
    }

    public void Desactivate()
    {
        SoundManager.instance.Play(SoundsTypes.Magic, gameObject);
        if (_myparticle) _myparticle.Play();
        OpenClose();
    }
}