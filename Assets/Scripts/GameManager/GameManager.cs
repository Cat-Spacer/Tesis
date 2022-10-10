using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

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
    [Header("PickUps")]
    [SerializeField] Text _pointsText;
    [SerializeField] float _points = 0;

    [Header("PickUps")]
    [SerializeField] float _pointsPerLevel = 0;
    [SerializeField] GameObject winScreen;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GetItem();
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

    public void ButtonDie()
    {
        _player.GetDamage(1);
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
        _player.ResetPlayer();
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

    public void GetItem()
    {
        _points++;
        if (_points != _pointsPerLevel)
        {
            _pointsText.text = _points.ToString();
        }
        else //Ganaste
        {
            //Time.timeScale = 0;
            winScreen.SetActive(true);
        }
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
