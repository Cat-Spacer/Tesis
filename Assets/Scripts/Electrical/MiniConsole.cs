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

    private void Start()
    {
        if (_hamster == null)
            _hamster = FindObjectOfType<Hamster>();

        if (_generator == null && Physics2D.OverlapArea(transform.position, transform.position * _checkRadius, _generatorMask))
            _generator = Physics2D.OverlapArea(transform.position, transform.position * _checkRadius, _generatorMask).gameObject.GetComponent<Generator>();
    }

    private void HamsterGetInside()
    {
        if (!_hamster) return;

        Debug.Log("ingrese");
        _hamster.MoveToPosition(_hamsterPos.position);
    }

    public void Interact()
    {
        if (!(_hamster || _generator)) return;
        if (Vector2.Distance(transform.position, _hamster.transform.position) <= _checkRadius)
            HamsterGetInside();
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
