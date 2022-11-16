using UnityEngine;
using System.Collections;
using InputKey;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Inputs;

    public float xAxis { get; private set; }
    public bool  jumpImput { get; private set; }
    public static bool  dashImput { get; private set; }
    public static bool dashInputStay { get; private set; }
    public bool jumpInputStay { get; private set; }
    public static bool jumpInputDown { get; private set; }
    public static bool  w_Imput { get; private set; }
    public static bool  a_Imput { get; private set; }
    public static bool  s_Imput { get; private set; }
    public static bool  d_Imput { get; private set; }
    public static bool  interactionInput { get; private set; }
    public static bool  trapInput  { get; private set; }
    public bool attackImput { get; private set; }

    //public bool climbInput { get; private set; }

    public KeyCode climbKey = KeyCode.C;

    private void Awake()
    {
        Inputs = this;
    }

    private void Update()
    {
        //Normal Inputs

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Climb.canClimbJump = true;
        }
        xAxis = Input.GetAxis("Horizontal");
        jumpImput = Input.GetKeyDown(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.Jump]);
        dashImput = Input.GetKeyDown(KeyCode.LeftShift);
        w_Imput = Input.GetKey(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.ClimbUp]);
        a_Imput = Input.GetKey(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.Left]);
        s_Imput = Input.GetKey(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.ClimbDown]);
        d_Imput = Input.GetKey(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.Right]);
        attackImput = Input.GetKeyDown(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.Attack]);
        dashInputStay = Input.GetKey(KeyCode.LeftShift);
        jumpInputStay = Input.GetKey(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.Jump]);
        interactionInput = Input.GetKeyDown(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.Interact]);
        jumpInputDown = Input.GetKeyDown(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.Jump]);
        //TrapInputs
        trapInput = Input.GetKeyDown(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.Left]) 
            || Input.GetKeyDown(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.Right]);
        //rightTrapInput = Input.GetKey(KeyCode.A);
    }


    IEnumerator ExampleCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

}
