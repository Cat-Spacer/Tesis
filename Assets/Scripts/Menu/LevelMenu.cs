using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : NetworkBehaviour
{
    [SerializeField] string _selectedLevel;
    [SerializeField] private Button _playButton;
    private string _defaultLevel = "MultiplayerTesting";

    public void SelectLevel(string lvl)
    {
        if (!ServerIsHost) return;
        if (string.IsNullOrEmpty(lvl)) _selectedLevel = _defaultLevel;
        _selectedLevel = lvl;
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