using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniConsole : MonoBehaviour, IMouseOver
{
    [SerializeField, Range(0.01f, 10f)] private float _checkRadius = 1f;
    [SerializeField] private Hamster _hamster = null;
    [SerializeField] private Generator _generator = null;
    [SerializeField] private LayerMask _generatorMask;
    [SerializeField] private Transform _hamsterPos = null;
    [SerializeField] private bool _gizmos = true;
    [SerializeField] LineRenderer feedbackLines;
    bool _isOn;
    public List<GameObject> _connection;
    public float _delaySeconds;
    SpriteRenderer _sp;
    [SerializeField] Sprite _spActivated, _spDesactivated;

    bool _isOutline;
    bool _showConnections;
    [SerializeField] Material outlineMat;
    Material defaultMat;
    private Dictionary<LineRenderer, Transform> _linesConnection = new Dictionary<LineRenderer, Transform>();

    private void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
        defaultMat = GetComponent<SpriteRenderer>().material;
        if (_hamster == null)
            _hamster = FindObjectOfType<Hamster>();

        if (_generator == null && Physics2D.OverlapArea(transform.position, transform.position * _checkRadius, _generatorMask))
            _generator = Physics2D.OverlapArea(transform.position, transform.position * _checkRadius, _generatorMask).gameObject.GetComponent<Generator>();

        CreateLines();
    }
    void CreateLines()
    {
        if (_connection.Count == 0) return;
        foreach (var connection in _connection)
        {
            var newLine = Instantiate(feedbackLines, transform);
            var connectionPos = connection.GetComponent<IElectric>().ConnectionSource();
            newLine.SetPosition(0, transform.position);
            newLine.SetPosition(1, connectionPos.position);
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
    private void HamsterGetInside()
    {
        if (!_hamster) return;

        Debug.Log("ingrese");
        _hamster.MoveToPosition(_hamsterPos.position);
    }

    public void Interact()
    {
        Debug.Log("Entre");
        if (!_isOn)
        {
            _isOn = true;
            StartCoroutine(Delay(_isOn));
        }
        else
        {
            _isOn = false;
            StartCoroutine(Delay(_isOn));
        }

        //if (!(_hamster || _generator)) return;
        //if (Vector2.Distance(transform.position, _hamster.transform.position) <= _checkRadius)
        //    HamsterGetInside();
    }
    IEnumerator Delay(bool power = true)
    {
        yield return new WaitForSeconds(_delaySeconds);
        for (int i = 0; i < _connection.Count; i++)
        {
            if (power)
            {
                _sp.sprite = _spActivated;
                if (_connection[i] != null && _connection[i].GetComponent<IElectric>() != null)
                {
                    _connection[i].GetComponent<IElectric>().TurnOn();
                   
                }
                else if (_connection[i] != null && _connection[i].GetComponentInChildren<IElectric>() != null)
                {
                    _connection[i].GetComponentInChildren<IElectric>().TurnOn();
                }
            }
            else
            {
                _sp.sprite = _spDesactivated;
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
        //     lines.SetActive(true);
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

    private void OnDrawGizmos()
    {
        if (!_gizmos) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _checkRadius);
    }
}
