using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLevelMenu : NetworkBehaviour
{
    [SerializeField] private Button _resumeBtn;
    [SerializeField] private Button _optionsBtn;
    [SerializeField] private Button _levelMenuBtn;
    [SerializeField] private Button _menuBtn;
    
    [SerializeField] private GameObject _pauseMenu;

    private bool _onPause = false;
    
    private void Awake()
    {
        _resumeBtn.onClick.AddListener(Resume);
        _optionsBtn.onClick.AddListener(Options);
        _levelMenuBtn.onClick.AddListener(LevelMenu);
        _menuBtn.onClick.AddListener(MainMenu);
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallBack;
    }

    private void NetworkManager_OnClientDisconnectCallBack(ulong clientId)
    {
        ShutDownRpc();
        MainMenuRpc();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_onPause)
            {
                PauseMenu();
                PauseMenuRpc();
            }
            else
            {
                Resume();
                ResumeRpc();
            }
        }
    }
    void Options()
    {
        
    }

    public void LevelMenu()
    {
        Resume();
        ResumeRpc();
        LevelMenuRpc();
    }

    [Rpc(SendTo.Server)]
    void LevelMenuRpc()
    {
        NetworkManager.SceneManager.LoadScene("LevelSelector", LoadSceneMode.Single);
    }
    
    void MainMenu()
    {
        EventManager.Instance.Trigger("OnDisconnectedPlayer");
        ShutDownRpc();
    }
    
    
    [Rpc(SendTo.Everyone)]
    void MainMenuRpc()
    {
        Time.timeScale = 1;
        //Destroy(NetworkManager.Singleton);
        SceneManager.LoadScene("MenuMultiplayer", LoadSceneMode.Single);
    }

    [Rpc(SendTo.Server)]
    void ShutDownRpc()
    {
        NetworkManager.Shutdown();
    }
    [Rpc(SendTo.NotMe)]                           
    void ResumeRpc()                              
    {                                             
        Time.timeScale = 1;                       
        _onPause = false;                         
        _pauseMenu.SetActive(false);              
        ResetMenu();                              
    }                                             
    void Resume()                                 
    {                                             
        Time.timeScale = 1;                       
        _onPause = false;                         
        _pauseMenu.SetActive(false);              
        ResetMenu();                              
    }                                             
    [Rpc(SendTo.NotMe)]
    void PauseMenuRpc()
    {
        Time.timeScale = 0;    
        _onPause = true;
        _pauseMenu.SetActive(true);
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
}
