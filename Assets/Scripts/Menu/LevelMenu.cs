using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : NetworkBehaviour
{
    [SerializeField] string _selectedLevel;
    [SerializeField] private Button _playButton;

    public void SelectLevel(string lvl)
    {
        if (!ServerIsHost) return;
        _selectedLevel = lvl;
    }

    public void PlayLevel()
    {
        if (!ServerIsHost) return;
        OpenLevelRpc();
    }
    [Rpc(SendTo.Server)]
    void OpenLevelRpc()
    {
        NetworkManager.SceneManager.LoadScene(_selectedLevel, LoadSceneMode.Single);
    }
}
