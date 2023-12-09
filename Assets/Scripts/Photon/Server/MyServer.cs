using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MyServer : MonoBehaviourPun
{
    public static MyServer Instance;

    Player _server;
    private Player playerLocal;

    public GameObject catPrefab, hamsterPrefab;
    public bool _catSelected, _hamsterSelected;
    public bool _selectCat, _selectHamster;

    // Dictionary<Player, TankMovement> _dictMovement = new Dictionary<Player, TankMovement>();
    // Dictionary<Player, TankCannon> _dictTankCannon = new Dictionary<Player, TankCannon>();
    List<PlayerFA> players = new List<PlayerFA>();

    private Player[] playerId;

    public int PackagePerSecond { get; private set; }

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance == null)
        {
            if (photonView.IsMine)
            {
                //Este RPC va en direccion a todos los Avatares que se crean
                //cada vez que un cliente nuevo entra a la sala
                photonView.RPC("SetServer", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer, 1);
            }
        }
    }

    [PunRPC]
    void SetServer(Player serverPlayer, int sceneIndex = 1)
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _server = serverPlayer;

        PackagePerSecond = 60;

        PhotonNetwork.LoadLevel(sceneIndex);
        playerLocal = PhotonNetwork.LocalPlayer;
    }
    public void SelectCatPlayer()
    {
        if (playerLocal != _server)
        {
            Debug.Log("AddCat");
            //Este RPC lo ejecuta cada servidor avatar en direccion al server original
            _selectCat = true;
            photonView.RPC("AddPlayer", _server, playerLocal);
        }
    }
    public void SelectHamsterPlayer()
    {
        if (playerLocal != _server)
        {
            //Este RPC lo ejecuta cada servidor avatar en direccion al server original
            _selectHamster = true;
            photonView.RPC("AddPlayer", _server, playerLocal);
        }
    }
    
    [PunRPC]
    void AddPlayer(Player player, GameObject character)
    {
        Debug.Log("AddPlayer");
        if (_selectCat) _catSelected = true;
        else if (_selectHamster) _hamsterSelected = true;
        StartCoroutine(WaitForLevel(player));
    }
    
    IEnumerator WaitForLevel(Player player)
    {
         while(PhotonNetwork.LevelLoadingProgress > 0.9f)
         {
             yield return new WaitForEndOfFrame();
         }
        
         playerId = PhotonNetwork.PlayerListOthers;
         if (_catSelected)
         {
             PlayerFA newCharacter = PhotonNetwork.Instantiate(catPrefab.name, Vector3.zero, Quaternion.identity)
                 .GetComponentInChildren<PlayerFA>()
                 .SetInitialParameters(player, playerId.Length);
             players.Add(newCharacter);
         }
         else if(_hamsterSelected)
         {
             PlayerFA newCharacter = PhotonNetwork.Instantiate(hamsterPrefab.name, Vector3.zero, Quaternion.identity)
                 .GetComponentInChildren<PlayerFA>()
                 .SetInitialParameters(player, playerId.Length);
             players.Add(newCharacter);
         }
         // _dictMovement.Add(player, newCharacter.GetComponentInChildren<TankMovement>());
         // _dictTankCannon.Add(player, newCharacter.GetComponentInChildren<TankCannon>());
    }

    public List<PlayerFA> GetPlayers()
    {
        return players;
    }
    public float GetPlayerId()
    {
        return playerId.Length;
    }
    
    
    #region REQUESTES QUE RECIBEN LOS SERVIDORES AVATARES

    //Esto lo recibe del Controller y va a llamar por RPC a la funcion Move del host real

    public void RequestMove(Player player, float dir)
    {
        photonView.RPC("RPC_ForwardMove", _server, player, dir);
    }    
    public void RequestTurn(Player player, float dir)
    {
        photonView.RPC("RPC_TurnMove", _server, player, dir);
    }
    public void RequestShoot(Player player)
    {
        photonView.RPC("RPC_Shoot", _server, player);
    }
    public void RequestPlaceMine(Player player)
    {
        photonView.RPC("RPC_PlaceMine", _server, player);
    }
    public void RequestAim(Player player, Vector3 mousePos)
    {
        photonView.RPC("RPC_Aim", _server, player, mousePos);
    }

    public void RequestAimWithKeys(Player player, float turnDir)
    {
        photonView.RPC("RPC_AimWithKeys", _server, player, turnDir);
    }

    public void RequestDisconnection(Player player)
    {
        Debug.LogWarning("ENVIO RPC");

        //PhotonNetwork.SendAllOutgoingCommands();
        //photonView.RPC("RPC_PlayerDisconnect", _server, player);
        //PhotonNetwork.SendAllOutgoingCommands();
    }

    #endregion

    #region SERVER ORIGINAL
    [PunRPC]
    private void RPC_ForwardMove(Player playerRequested, float dir)
    {
        // if (_dictMovement.ContainsKey(playerRequested))
        // {
        //     _dictMovement[playerRequested].Move(dir);
        // }
    }
    [PunRPC]
    private void RPC_TurnMove(Player playerRequested, float dir)
    {
        // if (_dictMovement.ContainsKey(playerRequested))
        // {
        //     _dictMovement[playerRequested].Turn(dir);
        // }
    }
    [PunRPC]
    void RPC_Shoot(Player playerRequested)
    {
        // if (_dictTankCannon.ContainsKey(playerRequested))
        // {
        //     _dictTankCannon[playerRequested].Shoot();
        // }
    }
    [PunRPC]
    void RPC_PlaceMine(Player playerRequested)
    {
        // if (_dictTankCannon.ContainsKey(playerRequested))
        // {
        //     _dictTankCannon[playerRequested].PlaceMine();
        // }
    }
    [PunRPC]
    void RPC_Aim(Player playerRequested, Vector3 mousePos)
    {
        // if (_dictTankCannon.ContainsKey(playerRequested))
        // {
        //     _dictTankCannon[playerRequested].Aim(mousePos);
        // }
    }
    [PunRPC]
    void RPC_AimWithKeys(Player playerRequested, float turnDir)
    {
        // if (_dictTankCannon.ContainsKey(playerRequested))
        // {
        //     _dictTankCannon[playerRequested].AimWithKeys(turnDir);
        // }
    }
    [PunRPC]
    public void RPC_PlayerDisconnect(Player player)
    {
        // PhotonNetwork.Destroy(_dictModels[player].gameObject);
        // _dictModels.Remove(player);
    }
    #endregion

}
