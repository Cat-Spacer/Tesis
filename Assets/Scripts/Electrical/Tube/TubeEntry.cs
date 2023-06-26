using System.Collections;
using UnityEngine;

public class TubeEntry : MonoBehaviour, IMouseOver
{
    [SerializeField] private Sprite _open, _closed;
    [SerializeField] private Tube _entryTube;
    [SerializeField] private float _searchRad = 1.0f;
    [SerializeField] private Material outlineMat;
    [SerializeField] private bool _isOpen, _isOutline, _allReadyIn = false;
    private SpriteRenderer _sp;
    private Material defaultMat;

    private void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
        _sp.sprite = _closed;
        defaultMat = GetComponent<SpriteRenderer>().material;
        _isOutline = false;
        if (!_entryTube && Physics2D.OverlapCircle(transform.position, _searchRad).GetComponent<Tube>())
            _entryTube = Physics2D.OverlapCircle(transform.position, _searchRad).GetComponent<Tube>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CustomMovement>())
        {
            _isOpen = true;
            _sp.sprite = _open;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<CustomMovement>())
        {
            _isOpen = false;
            _sp.sprite = _closed;
        }
    }

    public void MouseOver()
    {
        if (_isOutline || !_isOpen) return;
        _sp.material = outlineMat;
        _isOutline = true;
    }

    public void MouseExit()
    {
        if (!_isOutline) return;
        _sp.material = defaultMat;
        _isOutline = false;
    }

    public void Interact()
    {
        var hamster = FindObjectOfType<Hamster>();
        if (!(hamster && _isOpen)) return;

        if (!hamster.InTube())
            StartCoroutine(HamsterToEntry(hamster));
        else if (hamster.CurrentTube == _entryTube)
            StartCoroutine(HamsterToPlayer(hamster));
    }

    private IEnumerator HamsterToEntry(Hamster squix)
    {
        while (squix.transform.position != transform.position)
        {
            squix.GoToPosition(transform.position);
            yield return new WaitForEndOfFrame();
        }
        squix.GetInTube(_entryTube.transform.position, _entryTube);
    }

    private IEnumerator HamsterToPlayer(Hamster squix)
    {
        while (squix.transform.position != transform.position)
        {
            squix.GoToPosition(transform.position);
            yield return new WaitForEndOfFrame();
        }
        squix.ReturnToPlayer(false);
    }
}