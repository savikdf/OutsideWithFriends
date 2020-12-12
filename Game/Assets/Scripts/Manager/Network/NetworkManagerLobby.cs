using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerLobby : NetworkManager
{
    [SerializeField] private int minPlayers = 2;
    [Scene] [SerializeField] private string menuScene = string.Empty;

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;
    [HideInInspector] public List<NetworkRoomPlayerLobby> RoomPlayers { get; } = new List<NetworkRoomPlayerLobby>();

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;

    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>(ResourcePaths.SpawnablePrefabsPath).ToList();

    public override void OnStartClient()
    {
        Resources.LoadAll<GameObject>(ResourcePaths.SpawnablePrefabsPath).ToList().ForEach(p =>
        {
            ClientScene.RegisterPrefab(p);
        });
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        OnClientConnected?.Invoke();
    }
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        //do base logic then raise event
        base.OnClientDisconnect(conn);
        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        ////this doesnt work, the scene name compare
        //if (SceneManager.GetActiveScene().name != menuScene)
        //{
        //    //this stops midsession joining
        //    conn.Disconnect();
        //    return;
        //}

        base.OnServerConnect(conn);
    }
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab, Vector3.zero, Quaternion.identity);
        roomPlayerInstance.IsLeader = RoomPlayers.Count == 0;
        NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        RoomPlayers.ForEach(p => p.UpdateDisplay());
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (conn.identity != null)
        {
            NetworkRoomPlayerLobby player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();
            RoomPlayers.Remove(player);

            NotifyPlayersOfReadyState();
        }

        base.OnServerDisconnect(conn);
    }
    public override void OnStopServer()
    {
        RoomPlayers.Clear();
        base.OnStopServer();
    }

    public void NotifyPlayersOfReadyState()
    {
        bool roomReady = RoomPlayers.Count < minPlayers && RoomPlayers.All(pl => pl.IsReady);
        RoomPlayers.ForEach(p => { p.HandleReadyToStart(roomReady); });
    }
}
