using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Inputs;

    public float xAxis { get; private set; }
    public bool  jumpImput { get; private set; }
    public bool  dashImput { get; private set; }
    public bool  w_Imput { get; private set; }
    public bool  a_Imput { get; private set; }
    public bool  s_Imput { get; private set; }
    public bool  d_Imput { get; private set; }
    public bool attackImput { get; private set; }

    private void Awake()
    {
        Inputs = this;
    }

    private void Update()
    {
        xAxis = Input.GetAxis("Horizontal");
        jumpImput = Input.GetKeyDown(KeyCode.Space);
        dashImput = Input.GetKeyDown(KeyCode.LeftShift);
        w_Imput = Input.GetKey(KeyCode.W);
        a_Imput = Input.GetKey(KeyCode.A);
        s_Imput = Input.GetKey(KeyCode.S);
        d_Imput = Input.GetKey(KeyCode.D);
        attackImput = Input.GetKeyDown(KeyCode.J);
    }
}