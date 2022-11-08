using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public struct AreaPoint
    {
        public Transform respawnPoint;
        public GameObject[] resetObjects;
    }


    public static GameManager Instance;
    [SerializeField] Transform[] _respawnPoint;
    [SerializeField] AreaPoint[] _respawnArea;

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
    [SerializeField] float _pointsPerLevel = 0;
    [SerializeField] GameObject winScreen;
    [SerializeField] MiniMenu minuMenu;

    [Header("CurrentLevel")]
    [SerializeField] private int _currentLevel = 0;
    [SerializeField] MiniMap miniMap;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _pointsText.text = _points.ToString() + "/" + _pointsPerLevel.ToString();
        _player = FindObjectOfType<CustomMovement>();
    }
    private void Update()
    {

    }
    public void GetCurrentLevel(int lvl)
    {
        _currentLevel = lvl;
        miniMap.SetPlayerInLevel(lvl);
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
        SoundManager.instance.Play(SoundManager.Types.PlayerDeath);
        StartCoroutine(PlayerResetPositionCounter(_respawnIndex));
        StartCoroutine(DeathScreenCounter());
    }
    IEnumerator PlayerResetPositionCounter(int index_arg)
    {
        yield return new WaitForSeconds(_resetPlayerTime);
        _player.transform.position = new Vector3(_respawnArea[index_arg].respawnPoint.transform.position.x, _respawnArea[index_arg].respawnPoint.transform.position.y, 0);
        _player.ResetPlayer();

        foreach (var item in _respawnArea[index_arg].resetObjects)
        {
            item.SetActive(true);

            IRespawn obj = item.GetComponent<IRespawn>();
            if (obj != null)
            {
                obj.Respawn();
            }
        }
    
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
            minuMenu.OpenCall();
            _pointsText.text = _points.ToString() + "/" + _pointsPerLevel.ToString();
            miniMap.GotItem();
        }
        else //Ganaste
        {
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
