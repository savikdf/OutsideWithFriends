using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnManager : IManager
{
    GameObject Player { get; set; }

    void SpawnPlayer(int index);
}
