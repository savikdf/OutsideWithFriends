using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
public class SpawnManager : MonoBehaviour, IManager, ISpawnManager
{
    private GameObject player;
    public GameObject Player { 
        get{
            if(player == null)
            {
                player = (GameObject)GameManger.resourceLoader.LoadResourceObject(ResourcePaths.PlayerPrefabPath);
            }
            return player;
        }
        set { 
            player = value; 
        } 
    }

    private Spawn spawn;

    public bool Initialize()
    {
        DebugCol.Log(new Color(30, 215, 96, 1), $"Initializing {GetType().Name}.");

        //wipe all existing players for a fresh spawn
        List<Player> playersInScene = FindObjectsOfType<Player>().ToList();
        playersInScene?.ForEach(p => Destroy(p.gameObject));

        spawn = FindObjectOfType<Spawn>();
        GameObject temp = Player;
        SpawnPlayer(0);

        return true;
    }

    public void SpawnPlayer(int index)
    {
        GameObject newPlayer = GameObject.Instantiate(Player, spawn.randomSpawnPoint, Quaternion.identity);
        newPlayer.name = $"Player{index}";
    }

    public IEnumerator Routine(){
        Debug.Log("Running Spawn Routine");
        yield return null;
    }

}
