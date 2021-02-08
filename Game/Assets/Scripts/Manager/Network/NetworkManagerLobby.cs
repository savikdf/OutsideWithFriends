using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerLobby : NetworkManager
{
    [SerializeField] private int minPlayers = 1;
    [Scene] [SerializeField] private string menuSceneName = "MainMenu";

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;
    public List<NetworkRoomPlayerLobby> RoomPlayers { get; } = new List<NetworkRoomPlayerLobby>();

    [Header("Game")]
    public Action<string> OnSceneChanged;
    [SerializeField] private NetworkGamePlayerLobby gamePlayerPrefab = null;
    public List<NetworkGamePlayerLobby> GamePlayers { get; } = new List<NetworkGamePlayerLobby>();
    public GameObject spawnManagerObject = null;

    private bool IsAllPlayersReady
    {
        get
        {
            return RoomPlayers.All(p => p.IsReady);
        }
    }

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;

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
        ////do base logic then raise event
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
        RoomPlayers.ForEach(p => { p.HandleReadyToStart(IsAllPlayersReady); });
    }

    public void StartGame()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (!IsAllPlayersReady)
            {
                return;
            }
            ServerChangeScene("Level_Woods"); //only one level for now
        }
    }
        
    public override void OnServerChangeScene(string newSceneName)
    {
        string temp = SceneManager.GetActiveScene().name;
        if (SceneManager.GetActiveScene().name.Contains(menuSceneName) && newSceneName.StartsWith("Level_"))
        {
            for (int i = 0; i < RoomPlayers.Count; i++)
            {
                NetworkConnection conn = RoomPlayers[i].connectionToClient;
                NetworkGamePlayerLobby gamePlayerInstance = Instantiate(gamePlayerPrefab);
                gamePlayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);

                //remove from menu lobby
                NetworkServer.Destroy(conn.identity.gameObject);

                NetworkServer.ReplacePlayerForConnection(conn, gamePlayerInstance.gameObject);
            }
            base.OnServerChangeScene(newSceneName);
        }
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);
        OnServerReadied?.Invoke(conn);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName.StartsWith("Level_"))
        {
            GameObject spawner = Instantiate(spawnManagerObject);
            NetworkServer.Spawn(spawner);
            GameManger.singleton.spawnManager = spawner.GetComponent<ISpawnManager>();
        }
        base.OnServerSceneChanged(sceneName);
    }
}
