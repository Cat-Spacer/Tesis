using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : NetworkBehaviour
{
    private Button buttonSelected;
    [SerializeField] string _selectedLevel;
    [SerializeField] private Button _playButton;
    private string _defaultLevel = "MultiplayerTesting";


    [SerializeField] private List<Button> allLevels = new List<Button>();
    private Dictionary<string, Button> _allLevelsBtn = new Dictionary<string, Button>();

    private void Start()
    {
        foreach (var button in allLevels)
        {
            _allLevelsBtn.Add(button.name, button);
        }
    }

    public void SelectLevel(string lvl)
    {
        if (!ServerIsHost) return;
        if (lvl == null || lvl.Length <= 0) return;
        if (string.IsNullOrEmpty(lvl)) _selectedLevel = _defaultLevel;
        _selectedLevel = lvl;
        ButtonSelectRpc(_selectedLevel);
    }
    [Rpc(SendTo.NotMe)]
    void ButtonSelectRpc(string lvl)
    {
        if (_allLevelsBtn.ContainsKey(lvl)) _allLevelsBtn[lvl].Select();
    }

    public void PlayLevel()
    {
        if (!ServerIsHost) return;
        OpenLevelRpc();
    }

    public void PlayAndSelectLevel(string lvl)
    {
        SelectLevel(lvl);
        PlayLevel();
    }

    [Rpc(SendTo.Server)]
    void OpenLevelRpc()
    {
        NetworkManager.SceneManager.LoadScene(_selectedLevel, LoadSceneMode.Single);
    }
}