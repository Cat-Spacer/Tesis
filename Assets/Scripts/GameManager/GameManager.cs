using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] Transform _respawnPoint;

    CustomMovement _player;

    Action _CounterAction;
    [Header("DeathScreen")]
    [SerializeField] Animator _deathScreen;
    [SerializeField] float _resetPlayerTime;
    [SerializeField] float _deathScreenTime;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _player = FindObjectOfType<CustomMovement>();
        //_CounterAction = delegate { };
    }
    private void Update()
    {
        //_CounterAction();
    }

    public void PlayerDeath ()
    {
        _deathScreen.gameObject.SetActive(true);
        StartCoroutine(PlayerResetPositionCounter());
        StartCoroutine(DeathScreenCounter());
    }
    IEnumerator PlayerResetPositionCounter()
    {
        yield return new WaitForSeconds(_resetPlayerTime);
        _player.transform.position = new Vector3(_respawnPoint.transform.position.x, _respawnPoint.transform.position.y, 0);
    }    
    IEnumerator DeathScreenCounter()
    {
        yield return new WaitForSeconds(_deathScreenTime);
        _deathScreen.gameObject.SetActive(false);
        _player.ConstrainsReset();
    }
}
