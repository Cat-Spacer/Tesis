using System;
using System.Collections.Generic;
using Unity.Tutorials.Core.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    private Button buttonSelected;
    [SerializeField] string _selectedLevel;
    [SerializeField] private Button _playButton;
    private string _defaultLevel = "MenuCoop";

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
        if (lvl == null || lvl.Length <= 0) return;
        if (string.IsNullOrEmpty(lvl)) _selectedLevel = _defaultLevel;
        _selectedLevel = lvl;
    }

    public void PlayLevel()
    {
        if (_selectedLevel.IsNullOrEmpty()) return;
        SceneManager.LoadScene(_selectedLevel, LoadSceneMode.Single);
    }
    public void PlayAndSelectLevel(string lvl)
    {
        SelectLevel(lvl);
        PlayLevel();
    }
}