using UnityEngine;

public class WallClimb : MonoBehaviour
{
    [SerializeField] private float _climbSpeed = 5.0f;
    [SerializeField] private float _gravityScale = 0.25f;
    private float _gravityForce = 0.0f;
    [SerializeField] private int _wallLayerNumber = 8;
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
            {
                _customMovement.anim.SetBool("Climbing", true);
                _myRB.velocity = Vector2.up * _climbSpeed;
                //_myRB.AddForce(Vector2.up * _climbSpeed);
            }
            else
            {
                _customMovement.anim.SetBool("Climbing", false);
            }
           
            if (Input.GetKey(KeyCode.S))
            {
                _customMovement.anim.SetBool("Climbing", false);
                _myRB.velocity = Vector2.down * _climbSpeed;
                //_myRB.AddForce(Vector2.down * _climbSpeed);
            }


            if (Input.GetKey(KeyCode.A))
            {
                if (_customMovement.faceDirection == -1)
                {
                    _customMovement.rb.velocity = new Vector2(_customMovement.rb.velocity.x, 0);
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (_customMovement.faceDirection == 1)
                {
                    _customMovement.rb.velocity = new Vector2(_customMovement.rb.velocity.x, 0);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       
    }
    //Hacer mini salto al dejar la pared
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _wallLayerNumber)
        {
            _customMovement.rb.velocity = Vector2.zero;
            _onClimb = true;
            _myRB.gravityScale = _gravityScale;
            _customMovement.gravityForce = 0.0f;
            _customMovement.anim.SetBool("OnWall", true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _wallLayerNumber)
        {
            _onClimb = false;
            _myRB.gravityScale = 1.0f;
            _customMovement.gravityForce = _gravityForce;
            _customMovement.anim.SetBool("OnWall", false);
            _customMovement.anim.SetBool("Climbing", false);
        }
    }
}
