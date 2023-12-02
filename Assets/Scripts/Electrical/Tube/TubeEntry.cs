using System.Collections;
using UnityEngine;

public class TubeEntry : MonoBehaviour, IInteract
{
    [SerializeField] private Sprite _open, _closed;
    [SerializeField] private Tube _entryTube;
    [SerializeField] private float _searchRad = 1.0f;
    [SerializeField] private Material outlineMat;
    [SerializeField] private bool _isOpen, _isOutline, _allReadyIn = false;
    private SpriteRenderer _sp;
    private Material defaultMat;
    private HamsterChar _hamster;

    private void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
        _sp.sprite = _closed;
        defaultMat = GetComponent<SpriteRenderer>().material;
        _isOutline = false;
        if (!_entryTube && Physics2D.OverlapCircle(transform.position, _searchRad).GetComponent<Tube>())
            _entryTube = Physics2D.OverlapCircle(transform.position, _searchRad).GetComponent<Tube>();
        _hamster = GameManager.Instance.GetHamsterChar();
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.GetComponent<HamsterChar>())
    //     {
    //         _isOpen = true;
    //         _sp.sprite = _open;
    //     }
    // }
    //
    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //     if (collision.GetComponent<HamsterChar>())
    //     {
    //         _isOpen = false;
    //         _sp.sprite = _closed;
    //     }
    // }

    public void Interact()
    {
        if (!(_hamster && _isOpen)) return;
        
        Debug.Log("Puedo entrar");
        // if (!_hamster.InTube())
        //     StartCoroutine(HamsterToEntry(_hamster));
        // else if (_hamster.CurrentTube == _entryTube)
        //     StartCoroutine(HamsterToPlayer(_hamster));
    }

    public void ShowInteract(bool showInteractState)
    {
        if (showInteractState)
        {
            _isOpen = true;
            _sp.sprite = _open;
        }
        else
        {
            _isOpen = false;
            _sp.sprite = _closed;
        }
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