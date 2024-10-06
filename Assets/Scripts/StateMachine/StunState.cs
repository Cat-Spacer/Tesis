using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{
    public override void Enter()
    {
        model.GetStun(true);
        Debug.Log("GetStun");
    }

    public override void Do()
    {
        if (!data.isStun)
        {
            isComplete = true;
        }
    }

    public override void Exit()
    {
        model.GetStun(false);
    }
}
