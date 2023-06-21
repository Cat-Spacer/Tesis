using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lavel : MonoBehaviour, IMouseOver
{
    [SerializeField] private Door[] _doors;
    [SerializeField] private GameObject[] _openDoors;
    [SerializeField] private LayerMask _TubesMask;
    [SerializeField] private float _searchRad = .05f;
    SpriteRenderer _sp;
    bool _isOutline;
    bool _showConnections;
    [SerializeField] Material outlineMat;
    Material defaultMat;
    List<GameObject> _linesConnection = new List<GameObject>();
    public List<GameObject> _connection;
    [SerializeField] LineRenderer feedbackLines;

    private void Start()
    {
        _sp = GetComponentInChildren<SpriteRenderer>();
        defaultMat = _sp.material;
        SetLines();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CustomMovement>())
        {
            foreach (Door door in _doors)//Abre las puertas
                door.ActivateDesactivate(false);
            foreach (GameObject go in _openDoors)
                go.SetActive(true);
        }
    }
    void SetLines()
    {
        foreach (var connection in _connection)
        {
            var newLine = Instantiate(feedbackLines, transform);
            newLine.SetPosition(0, transform.position);
            newLine.SetPosition(1, connection.transform.position);
            _linesConnection.Add(newLine.gameObject);
            newLine.gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CustomMovement>())
        {
            foreach (Door door in _doors)
                door.ActivateDesactivate(true);
            foreach (GameObject go in _openDoors)//Cierra las puertas
                go.SetActive(false);
        }
    }
    public void Interact()
    {

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
}