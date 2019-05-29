using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    private int enemycount;

    [SerializeField] Transform[] spawnPoints;

    [SerializeField] GameObject enemy;
    [SerializeField] EnemySettings enemySettings;


    public void Spawn(int idx, float speed)
    {
        GameObject enemyClone = Instantiate(enemy, spawnPoints[idx].position, spawnPoints[idx].rotation, transform.root);
        enemycount++;
        SetupEnemy(enemyClone, speed);

    }

    public void Spawn(float speed)
    {
        int idx = Random.Range(0, spawnPoints.Length);
        GameObject enemyClone = Instantiate(enemy, spawnPoints[idx].position, spawnPoints[idx].rotation, transform.root);
        enemycount++;
        SetupEnemy(enemyClone, speed);
    }

    void SetupEnemy(GameObject go, float speed)
    {
        go.GetComponent<NavMeshAgent>().speed = speed;
    }

}
