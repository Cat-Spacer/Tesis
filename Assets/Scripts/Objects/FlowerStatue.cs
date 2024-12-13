using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class FlowerStatue : MonoBehaviour
{
    [SerializeField] private Transform _flowerPos;
    private Item _flower;
    private BoxCollider2D coll;
    bool _hasFlower = false;
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject arrow;
    [SerializeField] private ParticleSystem _onPutFlower;
    [SerializeField] private ParticleSystem[] _duringFlower;
    [SerializeField] private GameObject _duringFlowerFeedback;
    [SerializeField] private ParticleSystem[] _withoutFlower;
    [SerializeField] private GameObject[] _withoutFlowerLight;
    [SerializeField] private SpriteRenderer _statue;
    [SerializeField] private Color _sadColor;
    [SerializeField] private Color _happyColor;
    private void Start()
    {
        _duringFlowerFeedback.gameObject.SetActive(false);
        coll = GetComponent<BoxCollider2D>();
        foreach (var item in _withoutFlower)
        {
            item.Play();
        }
        foreach (var item in _withoutFlowerLight)
        {
            item.SetActive(true);
        }
        _statue= gameObject.GetComponent<SpriteRenderer>();
        _statue.color = _sadColor;
    }

    private void Update()
    {
        if (_target == null || _hasFlower) return;
        var dir = _target.position - transform.position;
        if (dir.magnitude < 2)
        {
            arrow.SetActive(false);
        }
        else
        {
            arrow.SetActive(true);
            arrow.transform.up = _target.position - transform.position;
        }
    }

    public void PutFlower(Item flower)
    {
        arrow.SetActive(false);
        _hasFlower = true;
        _flower = flower;
        _flower.HasPhysics(false);
        _flower.transform.parent = _flowerPos;
        _flower.transform.position = _flowerPos.position;
        coll.enabled = false;
        EventManager.Instance.Trigger(EventType.OnPutFlower);
        SoundManager.instance.Play(SoundsTypes.Collect, gameObject);
        _onPutFlower.Play();
        _duringFlowerFeedback.gameObject.SetActive(true);
        _statue.color = _happyColor;
        foreach (var item in _duringFlower)
        {
            item.Play();
        }
        foreach (var item in _withoutFlower)
        {
            item.Pause();
        }
        foreach (var item in _withoutFlowerLight)
        {
            item.SetActive(false);
        }
    }
    public bool HasFlower() {return _hasFlower;}
}
