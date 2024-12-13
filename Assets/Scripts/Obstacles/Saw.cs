using System;
using System.Collections;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Saw : MonoBehaviour
{
    private Action SpinAction = delegate {  };
    [SerializeField] private float _acelerationSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float _decelerationSpeed;
    [SerializeField] private float currentSpeed;
    [SerializeField] private ParticleSystem[] _blood;
    [SerializeField] private SpriteRenderer[] _bloodStun;
    int dmgTimes = 0;
    private void Start()
    {
        foreach (var item in _blood)
        {
            item.Pause();
        }
        foreach (var item in _bloodStun)
        {
            item.enabled=false;
        }
    }

    private void Update()
    {
        SpinAction();
    }

    public void StartSpinning()
    {
        SpinAction = Spin;
    }
    void Spin()
    {
        currentSpeed += _acelerationSpeed * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        transform.Rotate(0, 0, currentSpeed);
    }

    public void StopSpinning()
    {
        SpinAction = StopSpin;
    }
    void StopSpin()
    {
        currentSpeed -= _decelerationSpeed * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        transform.Rotate(0, 0, currentSpeed);
        if(currentSpeed <= 0)
        {
            currentSpeed = 0;
            SpinAction = delegate { };
        }
    }

   

    private void OnTriggerEnter2D(Collider2D coll)
    {
        var player = coll.gameObject.GetComponent<PlayerCharacter>();
        if (player == null) return;
        player.GetDamage();
        if (_blood!= null && _blood.Length>dmgTimes)
            _blood[dmgTimes].Play();
        if(_bloodStun != null && _bloodStun.Length > dmgTimes)
            _bloodStun[dmgTimes].enabled = true;
        dmgTimes += 1;
    }
}
