using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Generator : MonoBehaviour, IMouseOver
{
    public List<GameObject> _connection;
    private List<GameObject> _linesConnection = new List<GameObject>();
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
    [SerializeField] private LineRenderer feedbackLines;
    [SerializeField] private Material outlineMat;

    private bool _showConnections, _isOutline;
    private SpriteRenderer _sp;
    private Material defaultMat;


    private void Start()
    {
        if (_test) TurnButtons();
        _sp = GetComponent<SpriteRenderer>();
        defaultMat = GetComponent<SpriteRenderer>().material;
        _hamster = FindObjectOfType<Hamster>();
        //_miniGame = GetComponentInChildren<MiniGame>();
        _miniGameWin = false;
        _alreadyStarded = false;
        if (!_miniGame) _miniGame = FindObjectOfType<MiniGame>();

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
            _linesConnection.Add(newLine.gameObject);
            newLine.gameObject.SetActive(false);
        }
    }

    public void StartGenerator(bool start = true)
    {
        if (_miniGame) if (!_miniGame.GetSetGenerator) _miniGame.GetSetGenerator = this;
        if (_miniGameWin == true)
        {
            if (EnergyNeeded <= _hamster.Energy)
            {
                StartCoroutine(Delay(start));
            }
            StartCoroutine(Delay(start));
        }
        else //if (!_alreadyStarded)
        {
            _alreadyStarded = true;
            StartMiniGame();
        }
    }

    public void StopGenerator()
    {
        TurnButtons(false);
        if (_miniGame)
        {
            _miniGame.TurnOff();
            if (_miniGame.GetSetGenerator) _miniGame.GetSetGenerator = null;
        }
        StartCoroutine(Delay(false));
    }

    public void ReturnButton()
    {
        _hamster.ReturnToPlayer();
        StopGenerator();
    }

    public void OnWinMiniGame()
    {
        TurnButtons(true);
        _miniGameWin = true;
        _hamster.AddEnergy(-EnergyNeeded);
        //Debug.Log(-EnergyNeeded);
        StartGenerator(true);
    }

    void StartMiniGame() { if (_miniGame) _miniGame.TurnOn(); }

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

    public void MouseOver()
    {
        if (_isOutline) return;
        _sp.material = outlineMat;
        _isOutline = true;
        _showConnections = true;
        foreach (var lines in _linesConnection)
        {
            lines.SetActive(true);
        }
    }

    public void MouseExit()
    {
        if (!_isOutline) return;
        _sp.material = defaultMat;
        _isOutline = false;
        _showConnections = false;
        foreach (var lines in _linesConnection)
        {
            lines.SetActive(false);
        }
    }
    public void Interact()
    {
        //if (_showConnections) _showConnections = false;
        //else _showConnections = true;
    }
}