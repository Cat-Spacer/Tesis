using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonsManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu, controlsMenu, optionsMenu, mainFirstBTN, controlsFirstBTN, optionsFirstBTN;

    public void OpenMainMenu()
    {
        if (!mainMenu) return;
        mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainFirstBTN);
    }
    public void CloseMainMenu()
    {
        if (!mainMenu) return;
        mainMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void OpenControlsMenu()
    {
        if (!controlsMenu) return;
        controlsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(controlsFirstBTN);
    }
    public void CloseControlsMenu()
    {
        if (!controlsMenu) return;
        controlsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void OpenOptionsMenu()
    {
        if (!optionsMenu) return;
        optionsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsFirstBTN);
    }
    public void CloseOptionsMenu()
    {
        if (!optionsMenu) return;
        optionsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
