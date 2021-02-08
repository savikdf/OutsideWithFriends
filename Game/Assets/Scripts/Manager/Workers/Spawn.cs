using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public int spawnRadius = 3;
    private Vector3 spawnPos
    {
        get { return transform.position; }
    }

    public Vector3 RandomSpawnPoint
    {
        get
        {
            return new Vector3(spawnPos.x + Random.Range(-spawnRadius, spawnRadius),
                                spawnPos.y + Random.Range(-spawnRadius, spawnRadius),
                                spawnPos.z + Random.Range(-spawnRadius, spawnRadius));
        }
    }

    /// <summary>
    /// cirlce cut into 8ths 
    /// </summary>
    /// <param name="pointIndex"></param>
    /// <returns></returns>
    public Vector3 SpacedCircularSpawnPoint(int pointIndex)
    {
        float angle = Mathf.Deg2Rad * ((45 * -pointIndex) + 90);
        float z = spawnRadius * Mathf.Sin(angle);
        float x = spawnRadius * Mathf.Cos(angle);
        return new Vector3(x, transform.position.y, z);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(spawnPos, spawnRadius);
    }
}
