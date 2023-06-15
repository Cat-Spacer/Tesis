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

    private void Start()
    {
        if (_hamster == null)
            _hamster = FindObjectOfType<Hamster>();

        if (_generator == null && Physics2D.OverlapArea(transform.position, transform.position * _checkRadius, _generatorMask))
            _generator = Physics2D.OverlapArea(transform.position, transform.position * _checkRadius, _generatorMask).gameObject.GetComponent<Generator>();

        SetLines();
    }
    void SetLines()
    {
        foreach (var connection in _connection)
        {
            var newLine = Instantiate(feedbackLines, transform);
            newLine.SetPosition(0, transform.position);
            newLine.SetPosition(1, connection.transform.position);
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
        if (_isOn)
        {
            _isOn = false;
            StartCoroutine(Delay(_isOn));
        }
        else
        {
            _isOn = true;
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
    public void MouseExit()
    {

    }

    public void MouseOver()
    {

    }

    private void OnDrawGizmos()
    {
        if (!_gizmos) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _checkRadius);
    }
}
