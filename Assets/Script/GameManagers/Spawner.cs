using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    float spawnTimeLeft = 5;
    float spawnTimeInterval = 5;

    private int enemycount;

    [SerializeField] Transform[] spawnPoints;

    [SerializeField] GameObject enemy;

    void SpawnCountDown()
    {
        spawnTimeLeft -= Time.deltaTime;

        if (spawnTimeLeft < 0)
        {
            Spawn(Random.Range(1, spawnPoints.Length));
            spawnTimeLeft = spawnTimeInterval;
        }
    }

    public void Spawn(int idx)
    {
        Instantiate(enemy, spawnPoints[idx].position, spawnPoints[idx].rotation, transform.root);
        enemycount++;
    }

}
