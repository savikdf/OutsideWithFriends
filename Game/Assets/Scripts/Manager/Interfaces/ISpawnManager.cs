using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnManager : IManager
{
    GameObject PlayerPreFab { get; set; }

    void SpawnPlayer(NetworkConnection conn);
}
