using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] Transform[] _respawnPoint;

    CustomMovement _player;

    Action _CounterAction;
    [Header("DeathScreen")]
    [SerializeField] Animator _deathScreen;
    [SerializeField] float _resetPlayerTime;
    [SerializeField] float _deathScreenTime;
    int _respawnIndex = 0;
    float saveDistance;

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
    public void SaveDistance(float distance)
    {
        saveDistance = distance;
    }

    public float ReturnBaseDistanceClimb()
    {
        return saveDistance;
    }    
    public void PlayerDeath ()
    {
        _deathScreen.gameObject.SetActive(true);
        StartCoroutine(PlayerResetPositionCounter(_respawnIndex));
        StartCoroutine(DeathScreenCounter());
    }
    IEnumerator PlayerResetPositionCounter(int index_arg)
    {
        yield return new WaitForSeconds(_resetPlayerTime);
        _player.transform.position = new Vector3(_respawnPoint[index_arg].transform.position.x, _respawnPoint[index_arg].transform.position.y, 0);
    }    
    IEnumerator DeathScreenCounter()
    {
        yield return new WaitForSeconds(_deathScreenTime);
        _deathScreen.gameObject.SetActive(false);
        _player.ConstrainsReset();
    }

    public void SetRespawnPoint(int index_arg)
    {
        _respawnIndex = index_arg;
    }

    public void WaitForEndClimb(float waitTime)
    {
        StartCoroutine(CoroutineWaitForEndClimb(waitTime));
    }

    public IEnumerator CoroutineWaitForEndClimb(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
      // Climb.isClimbing = false;
        
    }
}
