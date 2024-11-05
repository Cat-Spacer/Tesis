using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteract
{
    void Interact(params object[] param);
    void ShowInteract(bool showInteractState);

}