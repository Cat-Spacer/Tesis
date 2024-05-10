using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] private Crystal[] _nextCrystal = default;
    public Crystal prevCrystal = default;
    public LineCollision line { get; set; }
    [SerializeField] private bool _forceStart = false, _rotable = true;
    [SerializeField] private int /*_crystalNumber = default,*/ _sidesForRotation = 4;
    [SerializeField] private bool _lastCrystal = default;
    [SerializeField] private GameObject _door = default;

    private bool _alreadyShowLight = default;
    //[SerializeField] private GameObject _interactLight = default;  

    public bool _activatedCrystal = false;

    private void Start()
    {
        if (!line) line = GetComponent<LineCollision>();
    }
    private void Update()
    {
        if (_forceStart)
        {
            CallCrystal(prevCrystal);
            _forceStart = false;
        }
    }

    /// <summary>
    /// Desactivates a GameObject if it's the last crystal
    /// </summary>
    /// <param name="linked"></param>
    public void CheckIfLastCrystal(bool linked)
    {
        if (_lastCrystal && _door != null && _door.GetComponent<Door>())
        {
            Debug.Log($"{gameObject.name} es last y tiene a {_door.gameObject} como puerta");
            //_door.GetComponent<Door>().ActivateDesactivate(linked);

            /*//old
             if (_nextCrystal)
                line.SetLines(_prevCrystal, _nextCrystal);
            */

            for (int i = 0; i < _nextCrystal.Length && _nextCrystal[i]; i++)
                line.SetLines(prevCrystal, _nextCrystal[i]);
        }
    }

    public void CallCrystal(Crystal prevCrystal)
    {
        if (prevCrystal == this) return;
        //Debug.Log($"{gameObject.name} call. _prevCrystal = {prevCrystal}");
        this.prevCrystal = prevCrystal;
        for (int i = 0; i < _nextCrystal.Length && _nextCrystal[i]; i++)
            line.SetLight(this.prevCrystal, this, _nextCrystal[i]);
        _activatedCrystal = true;
    }

    public void RotateCrystal()
    {
        transform.Rotate(0, 0, transform.rotation.y + (360 / _sidesForRotation));
        if (_activatedCrystal) CallCrystal(prevCrystal);
    }

    public Crystal GetPrevCrystal()
    {
        return prevCrystal;
    }
    public Crystal[] GetNextCrystal()
    {
        return _nextCrystal;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!prevCrystal) return;
        prevCrystal.CallCrystal(prevCrystal.prevCrystal);
        CallCrystal(prevCrystal);
        if (GetComponent<Rigidbody2D>()) Destroy(GetComponent<Rigidbody2D>());
        if (GetComponent<BoxCollider2D>()) GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void Interact()
    {
        if (_rotable) RotateCrystal();
    }

    public void ShowInteract(bool showInteractState)
    {
        if (showInteractState && !_alreadyShowLight)
        {
            //interactLight.SetActive(true);
            //alreadyShowLight = true;
        }
        else if (!showInteractState)
        {
            Debug.Log("Desactivado");
            //interactLight.SetActive(false);
            //alreadyShowLight = false;
        }
    }
}