using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkRoomPlayerLobby : NetworkBehaviour
{
    public GameObject localLobbyMenu;
    public PlayerLobbyDisplay[] playerDisplays = new PlayerLobbyDisplay[4];
    private bool isLeader;
    public bool IsLeader {
        get { return isLeader; }
        set {
            isLeader = value;
            startGameButton.enabled = value;
            menuManager.isLobbyHost = value;
        }
    }
    [HideInInspector] public MenuManager menuManager;

    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;
    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Searching...";

    [SerializeField] private List<Tuple<string, bool>> playerInfos = new List<Tuple<string, bool>>();
    [SerializeField] public Button startGameButton;

    private NetworkManagerLobby room;
    public NetworkManagerLobby Room {
        get {
            return (room == null) ? room = NetworkManager.singleton as NetworkManagerLobby : room;
        }
    }

    private void Awake()
    {
        menuManager = FindObjectOfType<MenuManager>().GetComponent<MenuManager>();
        localLobbyMenu.SetActive(false); //turn off, only enables if instance has authority
    }

    public override void OnStartServer()
    {
        UpdateDisplay();
        base.OnStartServer();
    }
    public override void OnStartAuthority()
    {
        CmdSetPlayerDisplayName(menuManager.playerDisplayName);
        localLobbyMenu.SetActive(true);
        UpdateDisplay();
    }
    public override void OnStartClient()
    {
        Room.RoomPlayers.Add(this); //add us to the lobby
        UpdateDisplay();
    }
    public override void OnStopClient()
    {
        Room.RoomPlayers.Remove(this);
        UpdateDisplay();
    }   

    private void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
    private void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();
    
    public void UpdateDisplay() {
        if (!hasAuthority)
        {
            foreach(NetworkRoomPlayerLobby p in Room.RoomPlayers)
            {
                if (p.hasAuthority)
                {
                    p.UpdateDisplay();
                    break;
                }
            }
            return;
        }
        playerInfos = new List<Tuple<string, bool>>();
        for (int i = 0; i < Room.RoomPlayers.Count && i < playerDisplays.Length; i++)
        {
            playerDisplays[i].displayName.text = Room.RoomPlayers[i].DisplayName;
            playerDisplays[i].readyStatus.text = Room.RoomPlayers[i].IsReady ? "<color=green>READY</color>" : "<color=red>WAITING</color>";
        }
    }
    public void HandleReadyToStart(bool isReadyToStart)
    {
        if (IsLeader)
        {
            startGameButton.enabled = isReadyToStart;
        }        
    }

    [Command]
    public void CmdUpdatePlayerName(string val)
    {
        DisplayName = val;
    }

    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;
        Room.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdSetPlayerDisplayName(string newDisplayName)
    {
        DisplayName = newDisplayName;
    }

    [Command]
    public void CmdStartGame()
    {
        if(!Room.RoomPlayers[0].isLeader) { return; }
        Debug.Log("Starting Game");
        Room.StartGame();
    }

    [Command]
    public void CmdLobbyCancel() {
        menuManager.LobbyCancel();
    }

}
