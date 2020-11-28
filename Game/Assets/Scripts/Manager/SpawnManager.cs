using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        Debug.Log($"Initializing {GetType().Name}.");

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


}
