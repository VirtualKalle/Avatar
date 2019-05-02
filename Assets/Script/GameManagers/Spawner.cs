using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField] Transform spawnPointParent;
    Transform[] spawnPoints;
    float spawnTimeLeft = 5;
    float spawnTimeInterval = 5;

    [SerializeField] GameObject enemy;
    private int enemycount;

    // Use this for initialization
    void Start () {
        spawnPoints = spawnPointParent.GetComponentsInChildren<Transform>();

    }
	
	// Update is called once per frame
	void Update ()
    {

        //if (!FindObjectOfType<EnemyHealth>())
        if (FindObjectsOfType<EnemyHealth>().Length < 5 && !AvatarHealth.isDead)
        {
            SpawnCountDown();
        }
    }

    void SpawnCountDown()
    {
        spawnTimeLeft -= Time.deltaTime;

        if (spawnTimeLeft < 0)
        {
            Spawn();
            spawnTimeLeft = spawnTimeInterval;
        }
    }

    void Spawn()
    {
        int idx = Random.Range(0, spawnPoints.Length);
        Instantiate(enemy, spawnPoints[idx].position, spawnPoints[idx].rotation, transform.root);
        enemycount++;

    }

}
