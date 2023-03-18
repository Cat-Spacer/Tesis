using UnityEngine;
using System.Collections;
using InputKey;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Inputs;

    // public float xAxis { get; private set; }
    public bool jumpInput { get; private set; }
    public static bool dashInput { get; private set; }
    public static bool dashInputStay { get; private set; }
    public bool jumpInputStay { get; private set; }
    public static bool jumpInputDown { get; private set; }
    public static bool up_Input { get; private set; }
    public static bool left_Input { get; private set; }

    public static bool left_Input_UpKey { get; private set; }
    public static bool right_Input_UpKey { get; private set; }

    public static bool left_Input_DownKey { get; private set; }
    public static bool right_Input_DownKey { get; private set; }
    public static bool down_Input { get; private set; }
    public static bool right_Input { get; private set; }
    public static bool interactionInput { get; private set; }
    public static bool trapInput { get; private set; }
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

        if (Input.GetKeyUp(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.Jump]))
        {
            Climb.canClimbJump = true;
        }
        //   xAxis = Input.GetAxis("Horizontal");
        jumpInput = Input.GetKeyDown(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.Jump]);
        dashInput = Input.GetKeyDown(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.Dash]);
        up_Input = Input.GetKey(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.ClimbUp]);
        left_Input = Input.GetKey(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.Left]);
        down_Input = Input.GetKey(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.ClimbDown]);
        right_Input = Input.GetKey(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.Right]);
        attackImput = Input.GetKeyDown(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.Attack]);
        dashInputStay = Input.GetKey(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.Dash]);
        jumpInputStay = Input.GetKey(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.Jump]);
        interactionInput = Input.GetKeyDown(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.Interact]);
        jumpInputDown = Input.GetKeyDown(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.Jump]);
        //TrapInputs
        left_Input_UpKey = Input.GetKeyUp(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.Left]);
        right_Input_UpKey = Input.GetKeyUp(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.Right]);
        left_Input_DownKey = Input.GetKeyUp(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.Left]);
        right_Input_DownKey = Input.GetKeyUp(InputDictionary.buttonKeys[InputDictionary.TypeOfKeys.Right]);
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