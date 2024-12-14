using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    private Button buttonSelected = null;
    [SerializeField] private int _sceneLoaderIndex = 2;
    [SerializeField] private string _selectedLevel = null;
    [SerializeField] private Button _playButton = null;
    private string _defaultLevel = "MenuCoop";

    [SerializeField] private List<Button> allLevels = new ();
    private Dictionary<string, Button> _allLevelsBtn = new ();

    private LevelNameBtn[] _levelNameBtns = null;

    private void Start()
    {
        List<LevelNameBtn> levelNamesAux = new();
        foreach (Button button in allLevels)
        {
            _allLevelsBtn.Add(button.name, button);
            if (button.GetComponent<LevelNameBtn>()) levelNamesAux.Add(button.GetComponent<LevelNameBtn>());
        }
        _levelNameBtns = levelNamesAux.ToArray();
        _selectedLevel = EventSystem.current.firstSelectedGameObject.name;
        if (_selectedLevel != null && EventSystem.current.firstSelectedGameObject.GetComponent<LevelNameBtn>())
            StaySelected(EventSystem.current.firstSelectedGameObject.GetComponent<LevelNameBtn>());
        if (_selectedLevel != null || !_playButton) return;
        _playButton.interactable = false;
        
        
    }

    public void SelectLevel(string lvl)
    {
        if (lvl == null || lvl.Length <= 0) return;
        if (string.IsNullOrEmpty(lvl)) _selectedLevel = _defaultLevel;
        _selectedLevel = lvl;
        if(_playButton) _playButton.interactable = true;
    }

    public void PlayLevel()
    {
        if (_selectedLevel.Length == 0) return;
        SceneToLoad(DecodeString(_selectedLevel));
    }
    private void SceneToLoad(int scene)
    {
        AsyncLoadScenes.sceneToLoad = scene + SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(_sceneLoaderIndex);
    }
    public void PlayAndSelectLevel(string lvl)
    {
        SelectLevel(lvl);
        PlayLevel();
    }

    private int DecodeString(string textToSplit)
    {
        string[] stringArray = textToSplit.Split(" "[0]);//Split myString wherever there's a _ and make a String array out of it.
        int[] myNumbers = new int[stringArray.Length];
        for (int num = 0; num < stringArray.Length; num++)
        {
            int.TryParse(stringArray[num], out int res);
            myNumbers[num] = res;
        }

        return myNumbers[^1];
    }

    public void StaySelected(LevelNameBtn button)
    {
        if(!button.selected) button.SetSelected(true);

        foreach (LevelNameBtn btn in _levelNameBtns)
            if (btn.selected && btn != button)
                btn.SetSelected(false);
    }
}