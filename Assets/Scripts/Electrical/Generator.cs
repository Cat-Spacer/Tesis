using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Generator : MonoBehaviour
{
    public List<GameObject> _connection;
    [SerializeField] private bool _test = false;
    private bool _miniGameWin, _alreadyStarded;
    [SerializeField] private int _energyNeeded;
    [SerializeField] private float _delaySeconds = 1.0f;
    public GameObject buttons = null;
    [SerializeField] private GameObject _batterySprite = null;
    private Hamster _hamster;
    [SerializeField] private MiniGame _miniGame;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Electricty[] _electricityParticle;
    [SerializeField] LineRenderer feedbackLines;

    private void Start()
    {
        if (_test) TurnButtons();

        _hamster = FindObjectOfType<Hamster>();
        //_miniGame = GetComponentInChildren<MiniGame>();
        _miniGameWin = false;
        _alreadyStarded = false;

        _text.text = "0/" + _energyNeeded;
        if (_energyNeeded <= 0 && _batterySprite)
            _batterySprite.SetActive(false);
        SetLines();
    }
    void SetLines()
    {
        if (_connection.Count == 0) return;
        foreach (var connection in _connection)
        {
            var newLine = Instantiate(feedbackLines, transform);
            newLine.SetPosition(0, transform.position);
            newLine.SetPosition(1, connection.transform.position);
        }
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

    public void StopGenerator()
    {
        TurnButtons(false);
        _miniGame.TurnOff();
        StartCoroutine(Delay(false));
    }

    public void ReturnButton()
    {
        _hamster.ReturnToCat();
        StopGenerator();
    }

    public void OnWinMiniGame()
    {
        _miniGameWin = true;
        _hamster.AddEnergy(-EnergyNeeded);
        //Debug.Log(-EnergyNeeded);
        StartGenerator(true);
    }

    void StartMiniGame() { _miniGame.TurnOn(); }

    public void TurnButtons(bool active = true) { buttons.SetActive(active); }

    public void SetEnergyCounter(int i) { _text.text = i + "/" + _energyNeeded; }

    public int EnergyNeeded { get { return _energyNeeded; } }

    public bool IsAlreadyStarded { get { return _alreadyStarded; } }

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
                    //if (_electricityParticle[i])
                    //    _electricityParticle[i].Activate();
                }
                else if (_connection[i] != null && _connection[i].GetComponentInChildren<IElectric>() != null)
                {
                    _connection[i].GetComponentInChildren<IElectric>().TurnOn();
                    //if (_electricityParticle[i])
                    //    _electricityParticle[i].Activate();
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
}