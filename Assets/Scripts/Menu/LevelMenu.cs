using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
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

    LevelNameBtn[] _levelNameBtns = default;

    private void Start()
    {
        List<LevelNameBtn> levelNamesAux = new();
        foreach (Button button in allLevels)
        {
            _allLevelsBtn.Add(button.name, button);
            if (button.GetComponent<LevelNameBtn>()) levelNamesAux.Add(button.GetComponent<LevelNameBtn>());
        }
        _levelNameBtns = levelNamesAux.ToArray();
    }

    public void SelectLevel(string lvl)
    {
        if (lvl == null || lvl.Length <= 0) return;
        if (string.IsNullOrEmpty(lvl)) _selectedLevel = _defaultLevel;
        _selectedLevel = lvl;
    }

    public void PlayLevel()
    {
        //if (_selectedLevel.IsNullOrEmpty()) return;
        if (_selectedLevel.Length == 0) return;
        SceneToLoad(DecodeString(_selectedLevel));
        //SceneManager.LoadScene(_selectedLevel, LoadSceneMode.Single);
    }
    private void SceneToLoad(int scene)
    {
        AsyncLoadScenes.sceneToLoad = scene + SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(1);
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

        return myNumbers[myNumbers.Length - 1];
    }

    public void StaySelected(LevelNameBtn button)
    {
        button.SetSelected(true);

        foreach (LevelNameBtn btn in _levelNameBtns)
            if (btn.selected == true && btn != button)
            {
                btn.SetSelected(false);
                return;
            }
    }
}