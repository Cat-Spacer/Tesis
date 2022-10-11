using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Inputs;

    public float xAxis { get; private set; }
    public bool  jumpImput { get; private set; }
    public static bool  dashImput { get; private set; }
    public static bool dashInputStay { get; private set; }
    public bool jumpInputStay { get; private set; }
    public static bool  w_Imput { get; private set; }
    public static bool  a_Imput { get; private set; }
    public static bool  s_Imput { get; private set; }
    public static bool  d_Imput { get; private set; }
    public bool attackImput { get; private set; }

    //public bool climbInput { get; private set; }

    public KeyCode climbKey = KeyCode.C;

    private void Awake()
    {
        Inputs = this;
    }

    private void Update()
    {
        xAxis = Input.GetAxis("Horizontal");
        //dashImput = Input.GetAxis("Dash");
        jumpImput = Input.GetKeyDown(KeyCode.Space);
        dashImput = Input.GetKeyDown(KeyCode.LeftShift);
        w_Imput = Input.GetKey(KeyCode.W);
        a_Imput = Input.GetKey(KeyCode.A);
        s_Imput = Input.GetKey(KeyCode.S);
        d_Imput = Input.GetKey(KeyCode.D);
        attackImput = Input.GetKeyDown(KeyCode.J);
        dashInputStay = Input.GetKey(KeyCode.LeftShift);
        jumpInputStay = Input.GetKey(KeyCode.Space);
        //climbInput = Input.GetKey(climbKey);
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
