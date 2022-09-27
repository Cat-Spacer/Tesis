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

    public static bool isClimbing = false;

    #endregion

    // _climbParticle.Play();
    // rb.gravityScale = _gravityScale;
    //   gravityForce = 0.0f;
    //    ForceDashEnd();

    //on exit
    //rb.gravityScale = 1.0f;
    //gravityForce = gravityForceDefault;

    
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
            if (playerInput.a_Imput)
                ClimbState(KeyCode.A, playerInput.a_Imput);
            if (playerInput.d_Imput)
                ClimbState(KeyCode.D, playerInput.d_Imput);
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

                Debug.Log("isClimbing" + isClimbing);
                /*if (keyPressed_arg == KeyCode.A || keyPressed_arg == KeyCode.D)
                {
                    Debug.Log("as");
                    _ClimbState = Freeze;
                }
                else if
                {
                    _ClimbState = ClimbActionUp;
                }*/

                _ClimbState = ClimbActionUp;
                _alreadyStarted = true;
            }
            else if (_alreadyStarted)
            {
                if (playerInput.jumpInputStay)
                {
                    _ClimbState = EndClimbForJump;
                }
                if (playerInput.dashInputStay)
                {
                    _ClimbState = EndClimbForDash;
                }
            }

            if (!state)
            {
                _impulseForce = _impulseDirectionExitForce;
                _ClimbState = EndClimb;
                return;
            }
        }
    }

    public void StartClimbingState()
    {
       SoundManager.instance.Play(SoundManager.Types.Climb);
        _animator.SetBool("OnWall", true);
        _animator.SetBool("Climbing", true);
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
        _animator.SetBool("OnWall", true);
        _animator.SetBool("Climbing", false);
        SoundManager.instance.Pause(SoundManager.Types.Climb);
    }

    public void FixedUpdateClimb()
    {
        _ClimbState();
    }
    public bool InSight()
    {
        if ( Physics2D.Raycast(_transform.position, _transform.right, _distanceToRope, ~_layerMask)) return true;
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
        if (playerInput.jumpInputStay)
        {
            _ClimbState = EndClimbForJump;
        }
        if (playerInput.dashInputStay)
        {
            _ClimbState = EndClimbForDash;
        }

        /* if (!playerInput.climbInput)
         {
             _impulseForce = _impulseDirectionExitForce;
             _ClimbState = EndClimb;
         }**/

        if (!InSight() && !FootInSight())
        {
            _impulseForce = _impulseDirectionExitForce;
            _ClimbState = EndClimb;
        }

        if (!InSight() && FootInSight())
        {
            _ClimbState = EndRope;
        }

        WhileClimbingState();

        if (playerInput.d_Imput && !playerInput.a_Imput  && !playerInput.w_Imput && !playerInput.s_Imput)
        {
            _ClimbState = Freeze;
            
        }

        if (playerInput.a_Imput && !playerInput.d_Imput  && !playerInput.w_Imput && !playerInput.s_Imput)
        {
            _ClimbState = Freeze;
        }

        if (!playerInput.a_Imput && !playerInput.d_Imput && !playerInput.w_Imput && !playerInput.s_Imput)
        {
            _ClimbState = Freeze;
        }

        if (_rb.velocity == Vector2.zero)
        {
            _ClimbState = Freeze;
        }
        //&& _rb.velocity == Vector2.zero
       /* if (playerInput.dashImput && InSight())
        {
            _rb.velocity = -_vector;
        }*/

        if (playerInput.w_Imput || playerInput.s_Imput)
        {
            _rb.velocity = _vector * _speed;
        }

    }
    public void EndClimb()
    {
        if (playerInput.jumpInputStay)
        {
            _ClimbState = EndClimbForJump;
        }
        if (playerInput.dashInputStay)
        {
            _ClimbState = EndClimbForDash;
        }
        EndFreeze();
        EndClimbingState();

        _rb.velocity = new Vector2(0, 0);
        //_rb.velocity = new Vector2(_rb.velocity.y, 0);
        _rb.angularVelocity = 0;
        _rb.AddForce(_transform.right * _impulseForce, ForceMode2D.Impulse);
        
     
        _alreadyStarted = false;
        //_rb.gravityScale = _worldDefaultGravity;
        GameManager.Instance.WaitForEndClimb(0.3f);
        //isClimbing = false;
        _ClimbState = delegate { };
        Climb.isClimbing = false;

    }

    void EndClimbForJump()
    {
        _rb.velocity = new Vector2(0, 0);
        //_rb.velocity = new Vector2(_rb.velocity.y, 0);
        _rb.angularVelocity = 0;
        _rb.isKinematic = false;
        _rb.constraints = ~RigidbodyConstraints2D.FreezeAll;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        EndClimbingState();
        _ClimbState = Jump;
    }

    void Jump()
    {
        _customMovement.JumpClimb2();
        _alreadyStarted = false;
        _ClimbState = delegate { };
    }
    void Dash()
    {
       // _customMovement.DashClimb();
        _ClimbState = _customMovement.DashClimb;
        //_alreadyStarted = false;
        //_ClimbState = delegate { };
    }

    public void SutilEnd()
    {
        _alreadyStarted = false;
        _ClimbState = delegate { };
    }

    void EndClimbForDash()
    {
        _rb.velocity = new Vector2(0, 0);
        //_rb.velocity = new Vector2(_rb.velocity.y, 0);
        _rb.angularVelocity = 0;
        _rb.isKinematic = false;
        _rb.constraints = ~RigidbodyConstraints2D.FreezeAll;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        EndClimbingState();
        _ClimbState = Dash;
    }
    void EndRope()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.angularVelocity = 0;
        _rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        if (playerInput.w_Imput || playerInput.s_Imput)
        {
            _rb.velocity = _vector * _speed * 0.8f;
        }

        if (!InSight() && !FootInSight() && LowerFootInSight())
        {
            _impulseForce = _impulseExitRopeForce;
            _ClimbState = EndClimb;
        }
        if (!InSight() && !FootInSight() && !LowerFootInSight())
        {
            _impulseForce = 0;
            _ClimbState = EndClimb;
        }

        if (InSight() && FootInSight())
        {
            _ClimbState = ClimbActionUp;
        }
        if (playerInput.jumpInputStay)
        {
            _ClimbState = EndClimbForJump;
        }
        if (playerInput.dashInputStay)
        {
            _ClimbState = EndClimbForDash;
        }

    }
    void EndFreeze()
    {
        if (playerInput.jumpInputStay)
        {
            _ClimbState = EndClimbForJump;
        }
        if (playerInput.dashInputStay)
        {
            _ClimbState = EndClimbForDash;
        }

        _rb.isKinematic = false;
        _rb.constraints = ~RigidbodyConstraints2D.FreezeAll;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        StartClimbingState();
        _ClimbState = ClimbActionUp;

    }
    public void Freeze()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.velocity = new Vector2(_rb.velocity.y, 0);
        _rb.angularVelocity = 0;
        _rb.isKinematic = true;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        FreezeClimbingState();


        /*if (!playerInput.climbInput)
        {
            _impulseForce = _impulseDirectionExitForce;
            _ClimbState = EndClimb;
        }*/
        _ClimbState = WaitForEndFreeze;
    }

    void WaitForEndFreeze()
    {
        if (playerInput.jumpInputStay)
        {
            _ClimbState = EndClimbForJump;
        }
        if (playerInput.dashInputStay)
        {
            _ClimbState = EndClimbForDash;
        }
        if (playerInput.w_Imput)
        {
            Debug.Log("end freeze");
            _ClimbState = EndFreeze;

        }
        if (playerInput.s_Imput)
        {
            _ClimbState = EndFreeze;
        }
        if (!InSight() && !FootInSight())
        {
            _impulseForce = _impulseDirectionExitForce;
            _ClimbState = EndClimb;
        }

        if (!InSight() && FootInSight())
        {
            _ClimbState = EndRope;
        }
    }

}


