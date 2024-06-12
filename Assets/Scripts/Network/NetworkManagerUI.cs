using System;
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

public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField] private GameObject _server;
    [SerializeField] private GameObject _host;
    [SerializeField] private GameObject _client;
    [SerializeField] private Button _serverButton;
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _clientButton;
    [SerializeField] private Button _backButton;
    
    [SerializeField] private TMP_Text _waitingPartner;
    [SerializeField] private Button _connectClient;
    [SerializeField] private TMP_InputField _hostCodeText;
    [SerializeField] private Button _copyCode;
    [SerializeField] private TMP_InputField _clientCodeText;
    [SerializeField] private Button _pasteCode;

    [SerializeField] private GameObject _levelSelectionMenu;

    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _connectionMenu;
    [SerializeField] private GameObject _backMainMenu;
    private Action _StartGameAction;
    private void Awake()
    {
        _serverButton.onClick.AddListener(() =>
        {
            // _hostButton.gameObject.SetActive(false);
            // _clientButton.gameObject.SetActive(false);
            // NetworkManager.Singleton.StartServer();
        });
        _hostButton.onClick.AddListener(() =>
        {
            CreateRelay();
            HostMenu();
            _StartGameAction = CheckPlayers;
        });
        _clientButton.onClick.AddListener(ClientMenu);
        _connectClient.onClick.AddListener(ConnectClient);
        _backButton.onClick.AddListener(BackButton);
        _copyCode.onClick.AddListener(CopyCodeBtn);
        _pasteCode.onClick.AddListener(PasteCodeBtn);
        _StartGameAction = delegate {  };
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log($"Signed in {AuthenticationService.Instance.PlayerId}");
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Update()
    {
        _StartGameAction();
    }
    private async void CreateRelay()
    {
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
    void CheckPlayers()
    {
        if (!NetworkManager.Singleton.ServerIsHost) return;
        if (NetworkManager.Singleton.ConnectedClients.Count == 2)
        {
            OpenLevelSelectionRpc();
        }
    }

    public void OpenConnectionModeMenu()
    {
        _mainMenu.gameObject.SetActive(false);
        _connectionMenu.gameObject.SetActive(true);
        _backMainMenu.gameObject.SetActive(true);
    }
    void HostMenu()
    {
        //_server.gameObject.SetActive(false);
        _client.gameObject.SetActive(false);
        _hostButton.gameObject.SetActive(false);
        _backButton.gameObject.SetActive(true);
        _waitingPartner.gameObject.SetActive(true);
    } 
    void ClientMenu()
    {
        //_server.gameObject.SetActive(false);
        _host.gameObject.SetActive(false);
        _clientButton.gameObject.SetActive(false);
        _connectClient.gameObject.SetActive(true);
        _backButton.gameObject.SetActive(true);
    }

    void MainMenu()
    {
        //_server.gameObject.SetActive(true);
        _host.gameObject.SetActive(true);
        _client.gameObject.SetActive(true);
        _serverButton.gameObject.SetActive(true);
        _hostButton.gameObject.SetActive(true);
        _clientButton.gameObject.SetActive(true);
        _waitingPartner.gameObject.SetActive(false);
        _connectClient.gameObject.SetActive(false);
        _backButton.gameObject.SetActive(false);
    }
    void BackButton()
    {
        if (ServerIsHost)
        {
            Debug.Log("shutdown");
            _StartGameAction = delegate {  };
            NetworkManager.Singleton.Shutdown();
            MainMenu();
        }
        else
        {
            Debug.Log("to menu");
            MainMenu();
        }
    }

    public void BackToMenu()
    {
        _mainMenu.gameObject.SetActive(true);
        _connectionMenu.gameObject.SetActive(false);
        _backMainMenu.gameObject.SetActive(false);
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
    void ConnectClient()
    {
        JoinRelay(_clientCodeText.text);
    }
    [Rpc(SendTo.Everyone)]
    void OpenLevelSelectionRpc()
    {
        //_server.gameObject.SetActive(false);
        _host.gameObject.SetActive(false);
        _client.gameObject.SetActive(false);
        _serverButton.gameObject.SetActive(false);
        _hostButton.gameObject.SetActive(false);
        _clientButton.gameObject.SetActive(false);
        _waitingPartner.gameObject.SetActive(false);
        _connectClient.gameObject.SetActive(false);
        _backButton.gameObject.SetActive(true);
        _levelSelectionMenu.gameObject.SetActive(true);
        //NetworkManager.SceneManager.LoadScene("MultiplayerTesting", LoadSceneMode.Single);
    }
}
