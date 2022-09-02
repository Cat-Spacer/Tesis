using UnityEngine;

public class WallClimb : MonoBehaviour
{
    [SerializeField] private float _climbSpeed = 5.0f;
    [SerializeField] private float _gravityScale = 0.25f;
    private float _gravityForce = 0.0f;
    [SerializeField] private string _wallTag = "WallClimb";
    private bool _onClimb = false;
    [SerializeField] private Rigidbody2D _myRB;
    [SerializeField] private CustomMovement _customMovement;

    private void Start()
    {
        if (_myRB == null)
            _myRB = GetComponent<Rigidbody2D>();
        if (_customMovement == null)
            _customMovement = GetComponent<CustomMovement>();
        _gravityForce = _customMovement.gravityForce;
    }

    private void Update()
    {
        if (_onClimb)
        {
            if (Input.GetKey(KeyCode.W))
                _myRB.velocity = Vector2.up * _climbSpeed;
                //_myRB.AddForce(Vector2.up * _climbSpeed);

            if (Input.GetKey(KeyCode.S))
                _myRB.velocity = Vector2.down * _climbSpeed;
                //_myRB.AddForce(Vector2.down * _climbSpeed);

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
                _customMovement.doDash = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == _wallTag)
        {
            _onClimb = true;
            _myRB.gravityScale = _gravityScale;
            _customMovement.gravityForce = 0.0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "WallClimb")
        {
            _onClimb = false;
            _myRB.gravityScale = 1.0f;
            _customMovement.gravityForce = _gravityForce;
        }
    }
}
