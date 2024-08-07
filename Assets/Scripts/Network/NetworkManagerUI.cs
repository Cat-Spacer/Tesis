using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine.SceneManagement;

[Serializable]
public enum MenuType
{
    MainMenu,
    ConnectionTypeMenu,
    HostWaitingMenu,
    ClientCodeMenu,
    LevelSelectionMenu
}
public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField] private Button _serverButton;
    [SerializeField] private Button _hostButton;
    
    [SerializeField] private TMP_InputField _hostCodeText;
    [SerializeField] private Button _copyCode;
    [SerializeField] private TMP_InputField _clientCodeText;
    [SerializeField] private Button _pasteCode;


    // [SerializeField] private GameObject _mainMenu;
    // [SerializeField] private GameObject _connectionMenu;


    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject connectionTypeMenu;
    [SerializeField] private GameObject hostWaitingMenu;
    [SerializeField] private GameObject clientCodeMenu;
    [SerializeField] private GameObject levelSelectionMenu;

    private Dictionary<MenuType, GameObject> _allMenu = new Dictionary<MenuType, GameObject>();

    private Action _StartGameAction;
    private void Awake()
    {
        _hostButton.onClick.AddListener(CreateRelay);

        _copyCode.onClick.AddListener(CopyCodeBtn);
        _pasteCode.onClick.AddListener(PasteCodeBtn);
        _StartGameAction = delegate {  };
        if (NetworkManager.Singleton != null)
        {
            
        }
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log($"Signed in {AuthenticationService.Instance.PlayerId}");
        };
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallBack;
        
        _allMenu.Add(MenuType.MainMenu,mainMenu);
        _allMenu.Add(MenuType.ConnectionTypeMenu,connectionTypeMenu);
        _allMenu.Add(MenuType.HostWaitingMenu,hostWaitingMenu);
        _allMenu.Add(MenuType.ClientCodeMenu,clientCodeMenu);
        _allMenu.Add(MenuType.LevelSelectionMenu,levelSelectionMenu);
    }
    
    private void NetworkManager_OnClientDisconnectCallBack(ulong clientId)
    {
        //OpenMenu(MenuType.MainMenu);
        // if (clientId == NetworkManager.ServerClientId)
        // {
        //     MainMenu();
        // }
    }
    private void Update()
    {
        _StartGameAction();
    }
    private async void CreateRelay()
    {
        if (NetworkManager.ServerIsHost) return;
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            _hostCodeText.text = joinCode;
            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
    private async void JoinRelay(string joinCode)
    {
        if(joinCode.Length == 0) return;
        try
        {
            Debug.Log($"Joining Relay with {joinCode}");
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            
            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        }
        catch(RelayServiceException e){Debug.Log(e);}
    }
    

    public void OpenMenu(MenuEnum key)
    {
        foreach (var menu in _allMenu)
        {
            menu.Value.SetActive(menu.Key == key.type);
        }   
    }

    public void WaitingClient()
    {
        _StartGameAction = CheckPlayers;
    }
    void CheckPlayers()
    {
        if (!NetworkManager.Singleton.ServerIsHost) return;
        if (NetworkManager.Singleton.ConnectedClients.Count == 2)
        {
            OpenLevelSelectionRpc();
        }
    }
    
    void CopyCodeBtn()
    {
        if (String.IsNullOrEmpty(_hostCodeText.text)) return;
        TextEditor textEditor = new TextEditor();
        textEditor.text = _hostCodeText.text;
        textEditor.SelectAll();
        textEditor.Copy();
    }
    private void PasteCodeBtn()
    {
        TextEditor textEditor = new TextEditor();
        textEditor.multiline = true;
        textEditor.Paste();
        _clientCodeText.text = textEditor.text;
    }
    public void ConnectClient()
    {
        JoinRelay(_clientCodeText.text);
    }
    [Rpc(SendTo.Everyone)]
     void OpenLevelSelectionRpc()
     {
         foreach (var menu in _allMenu)
         {
             menu.Value.SetActive(menu.Key == MenuType.LevelSelectionMenu);
         }   
     }
    public void ShutDownHost()
    {
        NetworkManager.Shutdown();
    }
}
