using UnityEngine;
using System;

public class Climb
{
    float _upSpeed = 5;
    float _downSpeed = 5 * 0.8f;
    public static float _distanceToRope = 0.4f;
    LayerMask _layerMask;
    float _impulseDirectionExitForce = 6;
    float _impulseExitRopeForce = 20;

    #region Private Variables
    private float _impulseForce;
    float _speed;
    PlayerInput playerInput;
    public Action _ClimbState = delegate { };
    Rigidbody2D _rb;
    Vector2 _vector;
    BoxCollider2D _collider;
    bool _alreadyStarted = false;
    Transform _transform;
    Animator _animator;
    CustomMovement _customMovement;
    float _worldDefaultGravity;
    private bool _dashInput => PlayerInput.dashImput;

    public static bool isClimbing = false;

    public Vector2 start;

    #endregion
    public void SetClimb(PlayerInput playerInput_arg, BoxCollider2D collider_arg, Rigidbody2D rb_arg, Transform transform_arg, float upSpeed_arg, float downSpeed_arg, float distanceToRope_arg,
        LayerMask layerMask_arg, float impulseDirectionExitForce_arg, float impulseExitRopeForce_arg, Animator animator_arg, CustomMovement customMovement_arg, float worldDefaultGravity_arg)
    {
        playerInput = playerInput_arg;
        _collider = collider_arg;
        _rb = rb_arg;
        _transform = transform_arg;
        _upSpeed = upSpeed_arg;
        _downSpeed = downSpeed_arg;
        _distanceToRope =distanceToRope_arg;
        _layerMask = layerMask_arg;
        _impulseDirectionExitForce = impulseDirectionExitForce_arg;
        _impulseExitRopeForce = impulseExitRopeForce_arg;
        _animator = animator_arg;
        _customMovement = customMovement_arg;
        _worldDefaultGravity = worldDefaultGravity_arg;
    }
    public void UpdateClimb()
    {
       
            if (playerInput.s_Imput)
                ClimbState(KeyCode.S, playerInput.s_Imput);
            if (playerInput.w_Imput)
                ClimbState(KeyCode.W, playerInput.w_Imput);
            if (PlayerInput.a_Imput)
                ClimbState(KeyCode.A, PlayerInput.a_Imput);
            if (PlayerInput.d_Imput)
                ClimbState(KeyCode.D, PlayerInput.d_Imput);
            if (PlayerInput.dashImput)
                ClimbState(KeyCode.LeftShift, PlayerInput.d_Imput);


    }

    /// <summary>
    /// Use when key is pressed down
    /// </summary>
    void ClimbState(KeyCode keyPressed_arg, bool state)
    {
        if (InSight())
        {
            if (keyPressed_arg == KeyCode.W)
            {
                _vector = Vector2.up;
                _speed = _upSpeed;
            }
            if (keyPressed_arg == KeyCode.S)
            {
                _vector = Vector2.down;
                _speed = _downSpeed;
            }
           

            if ( !_alreadyStarted)
            {
                StartClimbingState();
                isClimbing = true;


                _ClimbState = ClimbActionUp;
                _alreadyStarted = true;
            }
            else if (_alreadyStarted)
            {
                if (playerInput.jumpInputStay)
                {
                    _ClimbState = EndClimbForJump;
                }
                if (keyPressed_arg == KeyCode.LeftShift)
                {
                    //_ClimbState = delegate { };
                    _ClimbState = EndClimbForDash;
                }

              //  Debug.Log("as");
            }

           

           /* if (!state)
            {
                _impulseForce = _impulseDirectionExitForce;
                _ClimbState = EndClimb;
                return;
            }*/
           
        }
    }

    public void StartClimbingState()
    {
       SoundManager.instance.Play(SoundManager.Types.Climb);
        _animator.SetBool("Climbing", true);
        _animator.SetBool("OnWall", false);
        _customMovement.ForceDashEnd();
    }
    public void WhileClimbingState()
    {
    }
    public void EndClimbingState()
    {
        _animator.SetBool("OnWall", false);
        _animator.SetBool("Climbing", false);
        SoundManager.instance.Pause(SoundManager.Types.Climb);
    }
    public void PauseClimbingState()
    {
        _animator.SetBool("OnWall", true);
        _animator.SetBool("Climbing", false);
       SoundManager.instance.Pause(SoundManager.Types.Climb);
    }
    public void FreezeClimbingState()
    {
        _animator.SetBool("Climbing", false);
        _animator.SetBool("OnWall", true);
        
        SoundManager.instance.Pause(SoundManager.Types.Climb);
    }

    public void FixedUpdateClimb()
    {
        _ClimbState();
    }
    public bool InSight()
    {
        if (Physics2D.Raycast(_transform.position, _transform.right, _distanceToRope, ~_layerMask)) return true;
        else return false; 
        
    }
    public bool InSightLast(Vector2 last, float distance)
    {
        if (Physics2D.Raycast(_transform.position, last, distance, ~_layerMask)) return true;
        else return false;
    }
    bool FootInSight()
    {
        float point = _collider.size.y * 0.5f;
        if (Physics2D.Raycast(new Vector2(_transform.position.x, _transform.position.y - point - 0.1f), _transform.right, _distanceToRope, ~_layerMask)) return true;
        else return false;
    }
    bool LowerFootInSight()
    {
        float point = _collider.size.y * 0.5f;
        if (Physics2D.Raycast(new Vector2(_transform.position.x, _transform.position.y - point - 0.5f), _transform.right, _distanceToRope, ~_layerMask)) return true;
        else return false;
    }
    void ClimbActionUp()
    {
        Debug.Log("climb");
        if (playerInput.jumpInputStay)
        {
            _ClimbState = EndClimbForJump;
        }
       /* else if (_dashInput)
        {
            _ClimbState = EndClimbForDash;
        }*/
        else if (!InSight() && !FootInSight())
        {
            _impulseForce = _impulseDirectionExitForce;
            _ClimbState = EndClimb;
        }
        else if (!InSight() && FootInSight())
        {
            _ClimbState = EndRope;
        }

        WhileClimbingState();

        if ((PlayerInput.d_Imput && !PlayerInput.a_Imput  && !playerInput.w_Imput && !playerInput.s_Imput)||
            (PlayerInput.a_Imput && !PlayerInput.d_Imput && !playerInput.w_Imput && !playerInput.s_Imput)||
            (!PlayerInput.a_Imput && !PlayerInput.d_Imput && !playerInput.w_Imput && !playerInput.s_Imput))
        {
            _ClimbState = Freeze;
            
        }
        else if (playerInput.w_Imput || playerInput.s_Imput)
        {
            _rb.velocity = _vector * _speed;
        }

    }
    public static bool MoveTowardsBool = false;
    void MoveTowards()
    {
        MoveTowardsBool = true;
        float step = 10 * Time.deltaTime;

        if (InSight() && FootInSight())
        {
            StartClimbingState();
            _ClimbState = ClimbActionUp;
        }
         
            _transform.position = Vector2.MoveTowards(_transform.position, new Vector2(start.x + (0.5f *CustomMovement.faceDirection), _transform.position.y), step);


        if (_transform.position.x >= (start.x + 0.5f) || _transform.position.x <= (start.x - 0.5f))
        {
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0;

            _ClimbState = EndClimb;
        }
    }
    public void EndClimb()
    {
        if (playerInput.jumpInputStay)
        {
            _ClimbState = EndClimbForJump;
        }
        /*else if (_dashInput)
        {
            _ClimbState = EndClimbForDash;
        }*/

        _rb.constraints = ~RigidbodyConstraints2D.FreezeAll;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        EndClimbingState();
        Debug.Log("end");

        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0;
        _rb.AddForce(_transform.right * _impulseForce, ForceMode2D.Impulse);
       
       // GameManager.Instance.WaitForEndClimb(0.3f);
        Climb.isClimbing = false;
        _customMovement.dashClimb = false;
        MoveTowardsBool = false;
        _alreadyStarted = false;
        _ClimbState = delegate { };
       

    }

    public void EndClimbJump()
    {
        Climb.isClimbing = false;
        _customMovement.dashClimb = false;
        MoveTowardsBool = false;
        _alreadyStarted = false;
        _ClimbState = delegate { };

    }

    void EndClimbForJump()
    {
        _rb.velocity = new Vector2(0, 0);
        //_rb.velocity = new Vector2(_rb.velocity.y, 0);
        _rb.angularVelocity = 0;
        _rb.constraints = ~RigidbodyConstraints2D.FreezeAll;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        EndClimbingState();
        _ClimbState = Jump;
    }

    void Jump()
    {
        _customMovement.JumpClimb2();
        _alreadyStarted = false;
        //_ClimbState = delegate { };
    }


    public void SutilEnd()
    {
        FreezeClimbingState();
        _alreadyStarted = false;
        Climb.isClimbing = false;
        _customMovement.dashClimb = false;
        //_ClimbState = delegate { };
        _ClimbState = EndClimb;
        Debug.Log("SUTIL END");
    }

    void EndClimbForDash()
    {
        FreezeClimbingState();
        _rb.velocity = new Vector2(0, 0);
        //_rb.velocity = new Vector2(_rb.velocity.y, 0);
        _rb.angularVelocity = 0;
       // _rb.isKinematic = false;
        _rb.constraints = ~RigidbodyConstraints2D.FreezeAll;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        EndClimbingState();
        _ClimbState = _customMovement.DashClimb;
    }
    void EndRope()
    {
        _rb.velocity = new Vector2(0, 0);
        _rb.angularVelocity = 0;
        _rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        if (playerInput.w_Imput || playerInput.s_Imput)
        {
           _rb.velocity = _vector * _speed * 0.8f;
        }

        if (!InSight() && !FootInSight() && LowerFootInSight())
        {
            //_impulseForce = _impulseExitRopeForce;
            //_ClimbState = EndClimb;
            start = _transform.position;
            _ClimbState = MoveTowards;
        }
        if (!InSight() && !FootInSight() && !LowerFootInSight())
        {
            _impulseForce = 0;
            _ClimbState = EndClimb;
        }

        if (InSight() && FootInSight())
        {
            StartClimbingState();
            _ClimbState = ClimbActionUp;
        }
        if (playerInput.jumpInputStay)
        {
            _ClimbState = EndClimbForJump;
        }
        /*if (_dashInput)
        {
            _ClimbState = EndClimbForDash;
        }*/

    }
    void EndFreeze()
    {
        if (playerInput.jumpInputStay)
        {
            _ClimbState = EndClimbForJump;
        }
       /* if (_dashInput)
        {
            _ClimbState = EndClimbForDash;
        }*/

        _rb.constraints = ~RigidbodyConstraints2D.FreezeAll;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        StartClimbingState();
        _ClimbState = ClimbActionUp;

    }
    public void Freeze()
    {
        _rb.velocity = new Vector2(0, 0);
        _rb.angularVelocity = 0;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        FreezeClimbingState();
        _ClimbState = WaitForEndFreeze;
    }

    void WaitForEndFreeze()
    {
        if (playerInput.jumpInputStay)
        {
            _ClimbState = EndClimbForJump;
        }
       /* else if (_dashInput)
        {
            _ClimbState = EndClimbForDash;
        }*/
        else if (playerInput.w_Imput)
        {
            _ClimbState = EndFreeze;
        }
        else if (playerInput.s_Imput)
        {
            _ClimbState = EndFreeze;
        }
        else if (!InSight() && !FootInSight())
        {
            _impulseForce = _impulseDirectionExitForce;
            _ClimbState = EndClimb;
        }
        else if (!InSight() && FootInSight())
        {
            _ClimbState = EndRope;
        }
    }



}


