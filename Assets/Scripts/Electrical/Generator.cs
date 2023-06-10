using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Generator : MonoBehaviour
{
    public List<GameObject> _connection;
    [SerializeField] private bool _test = false;
    private bool _miniGameWin;
    private bool _alreadyStarded;
    [SerializeField] private int _energyNeeded;
    [SerializeField] private float _delaySeconds = 1.0f;
    public GameObject buttons = null;
    private Hamster _hamster;
    [SerializeField] MiniGame _miniGame;
    [SerializeField] TMP_Text _text;
    [SerializeField] Electricty[] _electricityParticle;

    private void Start()
    {
        if (_test) TurnButtons();

        _hamster =FindObjectOfType<Hamster>();
        //_miniGame = GetComponentInChildren<MiniGame>();
        _miniGameWin = false;
        _alreadyStarded = false;

        _text.text = "0/" + _energyNeeded;
    }

    public void ReturnButton()
    {
        _hamster.ReturnToCat();
        buttons.SetActive(false);
    }


    IEnumerator Delay(bool power = true)
    {
        yield return new WaitForSeconds(_delaySeconds);
        for (int i = 0; i < _connection.Count; i++)
        {
            if (power)
            {
                if (_connection[i] != null && _connection[i].GetComponent<IElectric>() != null)
                {
                    _connection[i].GetComponent<IElectric>().TurnOn();
                    _electricityParticle[i].Activate();
                }
                else if (_connection[i] != null && _connection[i].GetComponentInChildren<IElectric>() != null)
                {
                    _connection[i].GetComponentInChildren<IElectric>().TurnOn();
                    _electricityParticle[i].Activate();
                }
            }
            else
            {

                if (_connection[i] != null && _connection[i].GetComponent<IElectric>() != null)
                {
                    _connection[i].GetComponent<IElectric>().TurnOff();
                }
                else if (_connection[i] != null && _connection[i].GetComponentInChildren<IElectric>() != null)
                {
                    _connection[i].GetComponentInChildren<IElectric>().TurnOff();
                }
            }
        }
    }

    public int EnergyNeeded { get { return _energyNeeded; } }

    void StartMiniGame()
    {
        _miniGame.TurnOn();

    }

    public void OnWinMiniGame()
    {
        _miniGameWin = true;
        _hamster.AddEnergy(-EnergyNeeded);
        Debug.Log(-EnergyNeeded);
        StartGenerator(true);
    }

    public void TurnButtons()
    {
        buttons.SetActive(true);
    }

    public void SetEnergyCounter(int i)
    {
        _text.text = i + "/" + _energyNeeded;
    }

    public void StartGenerator(bool start = true)
    {
        if (_miniGameWin == true)
        {
            if (EnergyNeeded <= _hamster.Energy)
            {
                
                StartCoroutine(Delay(start));
            }
            StartCoroutine(Delay(start));
        }
        else if (!_alreadyStarded)
        {
            _alreadyStarded = true;
            StartMiniGame();
        }
    }
}