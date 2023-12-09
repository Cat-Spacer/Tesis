using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionUI : MonoBehaviour
{
    //public Button _catButton, _hamsterButton;
    private MyServer _server;


    public void SelectCat()
    {
        MyServer.Instance.SelectCatPlayer();
    }

    public void SelectHamster()
    {
        MyServer.Instance.SelectHamsterPlayer();
    }
}
