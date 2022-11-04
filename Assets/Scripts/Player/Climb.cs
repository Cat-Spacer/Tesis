using UnityEngine;
using System;

public class Climb
{
    float _upSpeed = 5;
    float _downSpeed = 5 * 0.8f;
    public static float _distanceToRope = 0.4f;
    LayerMask _climbLayerMask;
    float _impulseDirectionExitForce = 6;
    float _impulseExitRopeForce = 20;

    #region Private Variables
    private float _impulseForce;
    float _speed;
    PlayerInput playerInput;
    public Action _ClimbState = delegate { };
    Rigidbody2D _rb;
    public Vector2 _vector;
    BoxCollider2D _collider;
    bool _alreadyStarted = false;
    Transform _transform;
    Animator _animator;
    CustomMovement _customMovement;
    float _worldDefaultGravity;
    private bool _dashInput => PlayerInput.dashImput;
    LayerMask _groundMask;

    public static bool isClimbing = false;

    public Vector2 start;
    EnergyPower _energyPowerScript;

    #endregion
    public void SetClimb(PlayerInput playerInput_arg, BoxCollider2D collider_arg, Rigidbody2D rb_arg, Transform transform_arg, float upSpeed_arg, float downSpeed_arg, float distanceToRope_arg,
        LayerMask layerMask_arg, float impulseDirectionExitForce_arg, float impulseExitRopeForce_arg, Animator animator_arg, CustomMovement customMovement_arg, float worldDefaultGravity_arg, EnergyPower energy_arg,
        LayerMask groundMask_arg)
    {
        playerInput = playerInput_arg;
        _collider = collider_arg;
        _rb = rb_arg;
        _transform = transform_arg;
        _upSpeed = upSpeed_arg;
        _downSpeed = downSpeed_arg;
        _distanceToRope = distanceToRope_arg;
        _climbLayerMask = layerMask_arg;
        _impulseDirectionExitForce = impulseDirectionExitForce_arg;
        _impulseExitRopeForce = impulseExitRopeForce_arg;
        _animator = animator_arg;
        _customMovement = customMovement_arg;
        _worldDefaultGravity = worldDefaultGravity_arg;
        _energyPowerScript = energy_arg;
        _groundMask = groundMask_arg;
    }
    public void UpdateClimb()
    {

        if (PlayerInput.s_Imput)
            ClimbState(KeyCode.S, PlayerInput.s_Imput);
        if (PlayerInput.w_Imput)
            ClimbState(KeyCode.W, PlayerInput.w_Imput);
        if (PlayerInput.a_Imput)
            ClimbState(KeyCode.A, PlayerInput.a_Imput);
        if (PlayerInput.d_Imput)
            ClimbState(KeyCode.D, PlayerInput.d_Imput);
        if (PlayerInput.dashImput)
            ClimbState(KeyCode.LeftShift, PlayerInput.d_Imput);

        if (PlayerInput.jumpInputDown && isClimbing && !isHorizontal)
        {
            _impulseForce = _impulseDirectionExitForce;
            _ClimbState = EndClimbForJump;
        }

        if (!_energyPowerScript.hasEnergy && isClimbing)
        {
            _ClimbState = EndClimb;
        }

        if (PlayerInput.s_Imput && isHorizontal)
        {
            _impulseForce = _impulseDirectionExitForce;
            _ClimbState = EndClimb;
        }


    }

    /// <summary>
    /// Use when key is pressed down
    /// </summary>
    void ClimbState(KeyCode keyPressed_arg, bool state)
    {
        if (InSight(_climbLayerMask) && _energyPowerScript.EnergyDrain(0.05f))
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

            if (!_alreadyStarted)
            {
                Debug.Log("!_alreadyStarted");
                _customMovement.ForceDashEnd();  // primero forceo a que frene el dash
                StartClimbingState();
                isClimbing = true;

                CustomMovement.canHorizontalClimb = true;
                _ClimbState = ClimbActionVertical;
                _alreadyStarted = true;
            }
            else if (_alreadyStarted)
            {
              //  Debug.Log("_alreadyStarted");
                /*if (playerInput.jumpInputStay)
                {
                    _ClimbState = EndClimbForJump;
                }*/
                /* if (keyPressed_arg == KeyCode.LeftShift)
                 {
                     _ClimbState = EndClimbForDash;
                 }*/

            }
            if (keyPressed_arg == KeyCode.LeftShift)
            {
                _ClimbState = EndClimbForDash;
            }
            //Debug.Log("insight");


        }


    }

    public void StartClimbWithFreeze()
    {
        isClimbing = true;
       Debug.Log("StartClimbWithFreeze");
        _ClimbState = Freeze;
    }

    public void StartClimbingState()
    {
        Debug.Log("StartClimbingState");

        SoundManager.instance.Play(SoundManager.Types.Climb);
        _animator.SetBool("Climbing", true);
        _animator.SetBool("OnWall", false);
        //_customMovement.ForceDashEnd();
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
    public bool InSight(LayerMask layerMask_arg)
    {
        if (Physics2D.Raycast(_transform.position, _transform.right, _distanceToRope, layerMask_arg)) return true;
        else return false;
    }
    public bool InSightUp(LayerMask layerMask_arg)
    {
        if (Physics2D.Raycast(_transform.position, Vector2.up, 1, layerMask_arg)) return true;
        else return false;
    }

    public bool InSightLast(Vector2 last, float distance)
    {
        if (Physics2D.Raycast(_transform.position, last, distance, _climbLayerMask)) return true;
        else return false;
    }
    bool FootInSight()
    {
        float point = _collider.size.y * 0.5f;
        if (Physics2D.Raycast(new Vector2(_transform.position.x, _transform.position.y - point - 0.1f), _transform.right, _distanceToRope, _climbLayerMask)) return true;
        else return false;
    }
    bool LowerFootInSight()
    {
        float point = _collider.size.y * 0.5f;
        if (Physics2D.Raycast(new Vector2(_transform.position.x, _transform.position.y - point - 0.5f), _transform.right, _distanceToRope, _climbLayerMask)) return true;
        else return false;
    }
    public void ClimbActionVertical()
    {
        rotationVector = _transform.rotation.eulerAngles;
        rotationVector.z = 0;

        _transform.rotation = Quaternion.Euler(rotationVector);

        if (!InSight(_climbLayerMask) && !FootInSight())
        {
            _impulseForce = _impulseDirectionExitForce;
            _ClimbState = EndClimb;
        }
        else if (!InSight(_climbLayerMask) && FootInSight())
        {
            _ClimbState = EndRope;
        }
        WhileClimbingState();

        if ((PlayerInput.d_Imput && !PlayerInput.a_Imput && !PlayerInput.w_Imput && !PlayerInput.s_Imput) ||
            (PlayerInput.a_Imput && !PlayerInput.d_Imput && !PlayerInput.w_Imput && !PlayerInput.s_Imput) ||
            (!PlayerInput.a_Imput && !PlayerInput.d_Imput && !PlayerInput.w_Imput && !PlayerInput.s_Imput))
        {
            _ClimbState = Freeze;

        }
        else if (PlayerInput.w_Imput || PlayerInput.s_Imput)
        {
            _rb.velocity = _vector * _speed;
        }

    }

    public static bool isHorizontal;


    Vector3 rotationVector;
    public void ClimbActionHorizontal()
    {
        _energyPowerScript.EnergyDrain(0.05f);
        rotationVector = _transform.rotation.eulerAngles;
        rotationVector.z = 90;

        isHorizontal = true;

        Debug.Log("climb horizontal");

        //Quaternion target = Quaternion.Euler(0, 0,90);
        //_transform.rotation = Quaternion.Slerp(_transform.rotation, target, Time.deltaTime * 5);

        _rb.constraints = RigidbodyConstraints2D.FreezePositionY;

       /* if (PlayerInput.s_Imput)
        {
            _impulseForce = _impulseDirectionExitForce;
            _ClimbState = EndClimb;
        }*/

        if (!InSightUp(_climbLayerMask))
        {
            _impulseForce = _impulseDirectionExitForce;
            _ClimbState = EndClimb;
        }

        WhileClimbingState();

        if (!PlayerInput.a_Imput && !PlayerInput.d_Imput)
        {
            _ClimbState = FreezeHorizontal;
        }

        if (PlayerInput.a_Imput && !PlayerInput.s_Imput)
        {
            rotationVector.y = 180;
            _rb.velocity = -Vector2.right * 3f;
        }

        if (PlayerInput.d_Imput && !PlayerInput.s_Imput)
        {
            rotationVector.y = 0;
            _rb.velocity = Vector2.right * 3f;
        }


        _transform.rotation = Quaternion.Euler(rotationVector);
    }

    public void FreezeHorizontal()
    {
        PauseClimbingState();
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        if (PlayerInput.s_Imput)
        {
            _impulseForce = _impulseDirectionExitForce;
            _ClimbState = EndClimb;
        }
        _ClimbState = WaitHorizontal;
    }

    public void WaitHorizontal()
    {
        _energyPowerScript.EnergyDrain(0.05f);

       /* if (PlayerInput.s_Imput)
        {
            _impulseForce = _impulseDirectionExitForce;
            _ClimbState = EndClimb;
        }*/

        if (PlayerInput.a_Imput && !PlayerInput.s_Imput)
        {
            rotationVector.y = 180;
            _rb.velocity = -Vector2.right * 3f;
            StartClimbingState();
            _ClimbState = ClimbActionHorizontal;
        }

        if (PlayerInput.d_Imput && !PlayerInput.s_Imput)
        {
            rotationVector.y = 0;
            _rb.velocity = Vector2.right * 3f;
            StartClimbingState();
            _ClimbState = ClimbActionHorizontal;
        }
    }




    public static bool MoveTowardsBool = false;
    void MoveTowards()
    {
        MoveTowardsBool = true;
        float step = 10 * Time.deltaTime;

        if (InSight(_climbLayerMask) && FootInSight())
        {
            StartClimbingState();
            MoveTowardsBool = false;
            _ClimbState = ClimbActionVertical;
        }
         
            _transform.position = Vector2.MoveTowards(_transform.position, new Vector2(start.x + (0.5f *CustomMovement.faceDirection), _transform.position.y), step);


        if (_transform.position.x >= (start.x + 0.5f) || _transform.position.x <= (start.x - 0.5f))
        {
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0;
            MoveTowardsBool = false;
            _ClimbState = EndClimb;
        }
    }

    public static Vector2 startMirrorDash;

    public void MirrorDash()
    {
        MoveTowardsBool = true;
        float step = 20 * Time.deltaTime;

        _rb.velocity = new Vector2(0, 0);
        _rb.angularVelocity = 0;
        _rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (InSight(_climbLayerMask) && !startdash)
        {
            if (CustomMovement.faceDirection == -1)
            {
                _transform.rotation = Quaternion.Euler(_transform.rotation.x, _transform.rotation.z * -1, _transform.rotation.z);
            }
            if (CustomMovement.faceDirection == 1)
            {
                _transform.rotation = Quaternion.Euler(_transform.rotation.x, 180, _transform.rotation.z);
            }
            CustomMovement.faceDirection = -CustomMovement.faceDirection;
            newFace = CustomMovement.faceDirection;
            startdash = true;
        }
        _transform.position = Vector2.MoveTowards(_transform.position, new Vector2( _transform.position.x,startMirrorDash.y + (3 * newFace)), step);


        if (_transform.position.y >= (startMirrorDash.y + 3) || _transform.position.y <= (startMirrorDash.y - 3))
        {
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0;
            startdash = false;
            _ClimbState = EndClimb;
        }
        if (InSight(_climbLayerMask) && startdash)
        {
            StartClimbingState();
            startdash = false;
            _rb.constraints = ~RigidbodyConstraints2D.FreezePositionY;
            _ClimbState = ClimbActionVertical;
        }

    }

    bool startdash = false;

    int newFace;

    Vector2 dashStart;
    void MoveTowardsDash()
    {
        MoveTowardsBool = true;

        dashStart = _transform.position;
        _rb.velocity = new Vector2(0, 0);
        _rb.angularVelocity = 0;
        _rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (InSight(_climbLayerMask) )
        {
            if (CustomMovement.faceDirection == -1)
            {
                _transform.rotation = Quaternion.Euler(_transform.rotation.x, 0, _transform.rotation.z);
                rotationVector.y = 0;
            }
            if (CustomMovement.faceDirection == 1)
            {
                _transform.rotation = Quaternion.Euler(_transform.rotation.x, 180, _transform.rotation.z);
                rotationVector.y = 180;
            }
            CustomMovement.faceDirection = -CustomMovement.faceDirection;
            newFace = CustomMovement.faceDirection;
      
        }

        _ClimbState = ForceDash;
        _customMovement.StartDashFeedBack();

       // _transform.position = Vector2.MoveTowards(_transform.position, new Vector2(start.x + (3 * newFace), _transform.position.y), step);
       //_rb.MovePosition(_transform.position + _transform.forward * step);
       //_rb.AddForce(_transform.forward * 200);


        /* if (CustomMovement.collisionObstacle)
         {
             _rb.velocity = Vector2.zero;
             _rb.angularVelocity = 0;
             startdash = false;
             _ClimbState = EndClimb;
         }*/

    }

    public void ForceDash()
    {
        Debug.Log("dashing");
        _rb.velocity = Vector2.right * 45 * CustomMovement.faceDirection;
        _rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _ClimbState = EndDash;
       
    }
    public void EndDash()
    {
        Debug.Log("END DASH");

        if (InSight(_climbLayerMask))
        {
            _customMovement.EndDashFeedBack();
            Debug.Log("start climb");
            StartClimbingState();
            _rb.constraints = ~RigidbodyConstraints2D.FreezePositionY;
            CustomMovement.isDashing = false;
            _ClimbState = ClimbActionVertical;
           
        }
        else if (CustomMovement.collisionObstacle || Vector2.Distance(_transform.position, dashStart) >= 3)
        {
            _customMovement.EndDashFeedBack();
            Debug.Log("end climb");
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0;
            CustomMovement.isDashing = false;
            _ClimbState = EndClimb;
          
        }

    }

    public void EndClimb()
    {   
        rotationVector.x = _transform.rotation.x * CustomMovement.faceDirection;
        rotationVector.z = 0;
        _transform.rotation = Quaternion.Euler(rotationVector);

        /*if (playerInput.jumpInputStay)
        {
            _ClimbState = EndClimbForJump;
        }*/
        /*else if (_dashInput)
        {
            _ClimbState = EndClimbForDash;
        }*/

        _rb.constraints = ~RigidbodyConstraints2D.FreezeAll;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        EndClimbingState();

       // _rb.velocity = Vector2.zero;
       // _rb.angularVelocity = 0;
        _rb.AddForce(_transform.right * _impulseForce, ForceMode2D.Impulse);

        // GameManager.Instance.WaitForEndClimb(0.3f);
        isHorizontal = false;
        isClimbing = false;
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
        isHorizontal = false;
        if (InSight(_climbLayerMask))
        {
            _ClimbState = StartClimbWithFreeze;
        }

        _ClimbState = delegate { };

    }

    public static bool canClimbJump = true;

   /* void TryJump()
    {
        if (canClimbJump) 
            _ClimbState = EndClimbForJump;
    }*/
    void EndClimbForJump()
    {
        canClimbJump = false;
        _rb.velocity = new Vector2(0, 0);
        //_rb.velocity = new Vector2(_rb.velocity.y, 0);
        _rb.angularVelocity = 0;
        _rb.constraints = ~RigidbodyConstraints2D.FreezeAll;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        EndClimbingState();
        isHorizontal = false;
        if (InSight(_climbLayerMask))
        {
            if (CustomMovement.faceDirection == -1)
            {
                _transform.rotation = Quaternion.Euler(_transform.rotation.x, _transform.rotation.z * -1, _transform.rotation.z);
            }
            if (CustomMovement.faceDirection == 1)
            {
                _transform.rotation = Quaternion.Euler(_transform.rotation.x, 180, _transform.rotation.z);
            }
            CustomMovement.faceDirection = -CustomMovement.faceDirection;
        }
        _animator.SetTrigger("Jump");
        _ClimbState = Jump;
    }

    void Jump()
    {
        SoundManager.instance.Play(SoundManager.Types.CatJump);
        isHorizontal = false;

        if (InSight(_climbLayerMask))
        {
            _ClimbState = StartClimbWithFreeze;
        }

        _customMovement.JumpClimb2();
        _alreadyStarted = false;

    }


    public void SutilEnd()
    {
       
        FreezeClimbingState();
        _alreadyStarted = false;
        Climb.isClimbing = false;
        _customMovement.dashClimb = false;
        //_ClimbState = delegate { };
        isHorizontal = false;
        _ClimbState = EndClimb;
        Debug.Log("SUTIL END");
    }

    void EndClimbForDash()
    {
        CustomMovement.isDashing = true;
        FreezeClimbingState();
        _rb.velocity = new Vector2(0, 0);
        _rb.angularVelocity = 0;
        _rb.constraints = ~RigidbodyConstraints2D.FreezeAll;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        EndClimbingState();
        // _ClimbState = _customMovement.DashClimb;
        start = _transform.position;
        isHorizontal = false;
        _ClimbState = MoveTowardsDash;
    }
    public void EndClimbForMirrorDash()
    {
        isHorizontal = false;
        FreezeClimbingState();
        _rb.velocity = new Vector2(0, 0);
        _rb.angularVelocity = 0;
        _rb.constraints = ~RigidbodyConstraints2D.FreezeAll;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        EndClimbingState();
        _ClimbState = MirrorDash;
    }

    void EndRope()
    {  
        isHorizontal = false;
        _rb.velocity = new Vector2(0, 0);
        _rb.angularVelocity = 0;
        _rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;


        if (PlayerInput.w_Imput || PlayerInput.s_Imput)
        {
           _rb.velocity = _vector * _speed * 0.8f;
        }

        if (!InSight(_climbLayerMask) && !FootInSight() && LowerFootInSight() && !InSight(_groundMask))
        {
            //_impulseForce = _impulseExitRopeForce;
            //_ClimbState = EndClimb;
            start = _transform.position;
            _ClimbState = MoveTowards;
        }
        if (!InSight(_climbLayerMask) && !FootInSight() && !LowerFootInSight())
        {
            _impulseForce = 0;
            _ClimbState = EndClimb;
        }

        if (InSight(_climbLayerMask) && FootInSight())
        {
            StartClimbingState();
            _ClimbState = ClimbActionVertical;
        }
    }
    void EndFreeze()
    {
        _rb.constraints = ~RigidbodyConstraints2D.FreezeAll;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        StartClimbingState();
        _ClimbState = ClimbActionVertical;

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
        _energyPowerScript.EnergyDrain(0.05f);

        if (PlayerInput.w_Imput)
        {
            _ClimbState = EndFreeze;
        }
        else if (PlayerInput.s_Imput)
        {
            _ClimbState = EndFreeze;
        }
        else if (!InSight(_climbLayerMask) && !FootInSight())
        {
            _impulseForce = _impulseDirectionExitForce;
            _ClimbState = EndClimb;
        }
        else if (!InSight(_climbLayerMask) && FootInSight())
        {
            _ClimbState = EndRope;
        }
    }



}


