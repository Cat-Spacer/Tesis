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
    [SerializeField] private List<GameObject> _connectionObj;
    private List<IActivate> _connection = new List<IActivate>();
    [SerializeField] private bool _activated = false;
    [SerializeField] FlowerType _flowerType;
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
        foreach (var connection in _connectionObj)
        {
            var obj = connection.GetComponent<IActivate>();
            if (obj != null) _connection.Add(obj);
        }
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

        switch (_flowerType)
        {
            case FlowerType.Green:
                EventManager.Instance.Trigger(EventType.OnPutGreenFlower);
                break;
            case FlowerType.Yellow:
                EventManager.Instance.Trigger(EventType.OnPutYellowFlower);
                break;
            case FlowerType.Purple:
                EventManager.Instance.Trigger(EventType.OnPutPurpleFlower);
                break;
            default:
                EventManager.Instance.Trigger(EventType.OnPutFlower);
                Debug.LogError("flower type not specified in statue");
                break;
        }

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
        foreach (var item in _connection)
        {
            item.Desactivate();
        }
    }
    public bool HasFlower() {return _hasFlower;}
}
