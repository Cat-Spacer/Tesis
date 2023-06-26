using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Generator : MonoBehaviour, IMouseOver, IGenerator
{
  public List<GameObject> _connection;
    [SerializeField] private bool _test = false;
    [SerializeField] private int _energyNeeded;
    [SerializeField] private float _delaySeconds = 1.0f;
    [SerializeField] private GameObject buttons = null, _batterySprite = null, _onText;
    [SerializeField] private MiniGame _miniGame;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Electricty[] _electricityParticle;
    [SerializeField] private LineRenderer feedbackLines;
    [SerializeField] private Material outlineMat;
    [SerializeField] private Sprite _powerOnSprite, _powerOffSprite;
    [SerializeField] private bool _miniGameWin, _alreadyStarded;
    [SerializeField] private SoundManager.Types _sound = SoundManager.Types.GeneratorLoop;

    private Dictionary<LineRenderer, Transform> _linesConnection = new Dictionary<LineRenderer, Transform>();
    private Hamster _hamster;
    private bool _showConnections, _isOutline;
    private SpriteRenderer _sp;
    private Material defaultMat;
    
    [SerializeField] private Sprite _buttonGreenOn, _buttonGreenOff, _buttonRedOn, _buttonRedOff;

    [SerializeField] Image _greenButton, _redButton;

    private void Start()
    {
        if (_test) StartMiniGame();
        _sp = GetComponent<SpriteRenderer>();
        defaultMat = GetComponent<SpriteRenderer>().material;
        _hamster = FindObjectOfType<Hamster>();
        //_miniGame = GetComponentInChildren<MiniGame>();
        _miniGameWin = false;
        _alreadyStarded = false;
        _onText.SetActive(false);
        if (!_miniGame) _miniGame = FindObjectOfType<MiniGame>();
        EventManager.Instance.Subscribe("PlayerDeath", ResetGenerator);
        _text.text = "0/" + _energyNeeded;
        if (_energyNeeded <= 0)
        {
            _energyNeeded = 0;
            if (_batterySprite)
                _batterySprite.SetActive(false);
        }
        CreateLines();
    }

    void CreateLines()
    {
        if (_connection.Count == 0) return;
        foreach (var connection in _connection)
        {
            var newLine = Instantiate(feedbackLines, transform);
            var currentConnection = connection.GetComponent<IElectric>();
            var connectionPos = currentConnection.ConnectionSource();
            newLine.SetPosition(0, transform.position);
            newLine.SetPosition(1, connectionPos.position);
            currentConnection.SetGenerator(GetComponent<IGenerator>() , newLine);
            _linesConnection.Add(newLine, connectionPos);
            newLine.gameObject.SetActive(false);
        }
    }

    void ShowLines()
    {
        foreach (var line in _linesConnection)
        {
            line.Key.gameObject.SetActive(true);
            line.Key.SetPosition(0, transform.position);
            line.Key.SetPosition(1, line.Value.position);
        }
    }

    public void StartGenerator(bool start = true)
    {
        if (_miniGame) _miniGame.GetSetGenerator = this;
        if (_miniGameWin)
        {
            SoundManager.instance.Play(_sound);
            /*if (_hamster)
                if (EnergyNeeded <= _hamster.Energy) StartCoroutine(Delay(start));*/
            StartCoroutine(Delay(start));
        }
        else if (!_alreadyStarded)
        {
            _alreadyStarded = true;
            StartMiniGame();
        }
        else
        {
            _alreadyStarded = false;
            StopMiniGame();
        }
    }

    public void StopMiniGame()
    {
        if (_miniGame)
        {
            _miniGame.TurnOff();
            if (_miniGame.GetSetGenerator) _miniGame.GetSetGenerator = null;
        }
    }

    public void StopGenerator()
    {
        SoundManager.instance.Pause(_sound);
        _onText.SetActive(false);
        TurnButtons(false);
        StopMiniGame();
        //StartCoroutine(Delay(false));
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
        if (_hamster) _hamster.AddEnergy(-EnergyNeeded);
        if (_batterySprite) _batterySprite.SetActive(false);
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
        if (power)
        {
            _sp.sprite = _powerOnSprite;
            _greenButton.sprite = _buttonGreenOn;
            _redButton.sprite = _buttonRedOff;
            _onText.SetActive(true);
        }
        else
        {
            _sp.sprite = _powerOffSprite;
            
            _greenButton.sprite = _buttonGreenOff;
            _redButton.sprite = _buttonRedOn;
            _onText.SetActive(false);
        }
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
        ShowLines();
        if (_isOutline) return;
        _sp.material = outlineMat;
        _isOutline = true;
        _showConnections = true;
        // foreach (var lines in _linesConnection)
        // {
        //     lines.Key.gameObject.SetActive(true);
        // }
    }

    public void MouseExit()
    {
        if (!_isOutline) return;
        _sp.material = defaultMat;
        _isOutline = false;
        _showConnections = false;
        foreach (var lines in _linesConnection)
        {
            lines.Key.gameObject.SetActive(false);
        }
    }
    public void Interact()
    {
        //if (_showConnections) _showConnections = false;
        //else _showConnections = true;
    }
    public void ShowLineConnection(LineRenderer line)
    {
        foreach (var currentLine in _linesConnection)
        {
            if (currentLine.Key == line)
            {
                currentLine.Key.gameObject.SetActive(true);
                currentLine.Key.SetPosition(0, transform.position);
                currentLine.Key.SetPosition(1, currentLine.Value.position);
            }
        }
    }

    public void NotShowLineConnection(LineRenderer line)
    {
        foreach (var currentLine in _linesConnection)
        {
            if (currentLine.Key == line)
            {
                currentLine.Key.gameObject.SetActive(false);
            }
        }
    }

    void ResetGenerator(params object[] param)
    {
        _alreadyStarded = false;
        _batterySprite.SetActive(true);
        _miniGameWin = false;
        StopGenerator();
        SetEnergyCounter(0);
    }
}