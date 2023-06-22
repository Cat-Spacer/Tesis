using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IElectric
{
    void TurnOn();
    void TurnOff();

    Transform ConnectionSource();

}
