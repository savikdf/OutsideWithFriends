using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
public class SpawnManager : NetworkBehaviour, IManager, ISpawnManager
{
    private bool isInitialized = false;
    private GameObject playerPreFab;
    public GameObject PlayerPreFab { 
        get{
            if(playerPreFab == null)
            {
                playerPreFab = (GameObject)GameManger.resourceLoader.LoadResourceObject(ResourcePaths.PlayerPrefabPath);
            }
            return playerPreFab;
        }
        set { 
            playerPreFab = value; 
        } 
    }

    private int spawnedIndex;
    private Spawn spawn;

    public void Awake()
    {
        if(!isInitialized)
            Initialize();
    }

    public bool Initialize()
    {
        DebugCol.Log(new Color(30, 215, 96, 1), $"Initializing {GetType().Name}.");

        //wipe all existing players for a fresh spawn
        List<Player> playersInScene = FindObjectsOfType<Player>().ToList();
        playersInScene?.ForEach(p => Destroy(p.gameObject));

        spawn = FindObjectOfType<Spawn>();
        GameObject temp = PlayerPreFab;

        isInitialized = true;
        return true;
    }   

    public IEnumerator Routine(){
        Debug.Log("Running Spawn Routine");
        yield return null;
    }

    public override void OnStartServer() => NetworkManagerLobby.OnServerReadied += SpawnPlayer;
    [ServerCallback]
    public void OnDestroy() => NetworkManagerLobby.OnServerReadied -= SpawnPlayer;

    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {
        if (spawn == null)
        {
            Debug.LogError("Spawnpoint must be set up in the Scene before attempting to spawn Players");
            return;
        }

        GameObject newPlayer = GameObject.Instantiate(PlayerPreFab, spawn.SpacedCircularSpawnPoint(spawnedIndex), Quaternion.identity);
        newPlayer.name = $"Player {spawnedIndex}";

        NetworkServer.Spawn(newPlayer, conn);
        spawnedIndex++;
    }


}
