using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MenuType
{
    Main,
    Levels,
    Options,
    Credits
}
public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject levelMenu;

    private Dictionary<MenuType, GameObject> _allMenus = new Dictionary<MenuType, GameObject>();

    private void Start()
    {
        _allMenus.Add(MenuType.Levels, levelMenu);
    }

    public void PlayBtn(MenuEnum type)
    {
        SelectMenu(type.type);
    }

    public void OptionsBtn(MenuEnum type)
    {
        SelectMenu(type.type);
    }
    public void CreditsBtn(MenuEnum type)
    {
        SelectMenu(type.type);
    }
    public void QuitBtn(MenuEnum type)
    {
        SelectMenu(type.type);
    }

    void SelectMenu(MenuType key)
    {
        if (_allMenus.ContainsKey(key))
        {
            foreach (var menu in _allMenus)
            {
                if(menu.Key == key) _allMenus[menu.Key].SetActive(true);
                else _allMenus[menu.Key].SetActive(false);
            }
        }
    }
}