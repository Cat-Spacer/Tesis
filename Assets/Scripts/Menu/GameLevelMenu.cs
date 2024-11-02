using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameLevelMenu : MonoBehaviour
{
    [SerializeField] private Button _resumeBtn;
    [SerializeField] private Button _optionsBtn;
    [SerializeField] private Button _levelMenuBtn;
    [SerializeField] private Button _menuBtn;
    
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _winMenu;
    [SerializeField] private GameObject _loseMenu;

    private bool _onPause = false;
    
    private void Awake()
    {
        _resumeBtn.onClick.AddListener(Resume);
        _optionsBtn.onClick.AddListener(Options);
        _levelMenuBtn.onClick.AddListener(LevelMenu);
    }

    private void Start()
    {
        EventManager.Instance.Subscribe(EventType.OnLoseGame, OnLoseGame);
    }

    private void OnLoseGame(object[] obj)
    {
        Time.timeScale = 0; 
        _loseMenu.SetActive(true);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_onPause)
            {
                PauseMenu();
            }
            else
            {
                Resume();
            }
        }
    }
    void Options()
    {
        
    }

    public void LevelMenu()
    {
        Resume();
    }

    public void WinMenu()
    {
        _winMenu.SetActive(true);
        var points = EgoSystem.instance.GetEgoPoints();
        var winMenu = _winMenu.GetComponent<WinMenu>();
        if (winMenu != null)
        {
            winMenu.SetText(points.Item1.ToString(), points.Item2.ToString());
        }
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(GameManager.Instance.GetNextLevelName(), LoadSceneMode.Single);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(GameManager.Instance.GetCurrentLevelName(), LoadSceneMode.Single);
    }
    public void LevelSelectorMenu()
    {
        SceneManager.LoadScene("LevelSelector", LoadSceneMode.Single);
    }
    void Resume()                                 
    {                                             
        Time.timeScale = 1;                       
        _onPause = false;                         
        _pauseMenu.SetActive(false);              
        ResetMenu();                              
    }
    void PauseMenu()
    {
        Time.timeScale = 0;
        _onPause = true;
        _pauseMenu.SetActive(true);
    }
    void ResetMenu()
    {
        
    }
    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuCoop", LoadSceneMode.Single);
    }
}
