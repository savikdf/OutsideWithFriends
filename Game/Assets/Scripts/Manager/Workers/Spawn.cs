using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public int spawnRadius = 3;
    private Vector3 spawnPos {
        get { return transform.position; }
    }
    public Vector3 randomSpawnPoint {
        get {
            return new Vector3(spawnPos.x + Random.Range(-spawnRadius, spawnRadius), 
                                spawnPos.y + Random.Range(-spawnRadius, spawnRadius), 
                                spawnPos.z + Random.Range(-spawnRadius, spawnRadius));
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(spawnPos, spawnRadius);
    }

}
