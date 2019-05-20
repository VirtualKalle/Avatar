using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelManager : MonoBehaviour
{
    private bool newLevelCountDown;
    private bool spawnEnemies;

    float newLevelTimeLeft;
    float newLevelTime = 5;

    float spawnTimeLeft;
    float spawnInterval = 3;

    public static int level;
    int spawnedEnemies;

    [SerializeField] EnemySettings enemySettings;
    [SerializeField] GameObject UI;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI value;

    Spawner m_spawner;

    private void OnEnable()
    {
        EnemyHealth.destroyEvent += CheckEnemiesLeft;
    }

    private void OnDisable()
    {
        EnemyHealth.destroyEvent -= CheckEnemiesLeft;
    }

    private void Awake()
    {
        m_spawner = FindObjectOfType<Spawner>();
    }

    private void Start()
    {
        level = 0;
        spawnedEnemies = 0;
        newLevelTimeLeft = newLevelTime;
        spawnTimeLeft = spawnInterval;
        //spawnEnemies = true;
    }

    private void Update()
    {
        NewLevelCountDown();
        SpawnCountDown();
    }

    private void CheckEnemiesLeft()
    {
        if (EnemyHealth.levelKills == enemySettings.levels[level].nrOfEnemies)
        {
            if (level >= enemySettings.levels.Length - 1)
            {
                MissionComplete();
            }
            else
            {
                newLevelCountDown = true;
                UI.SetActive(true);
            }
        }
    }

    private void MissionComplete()
    {
        UI.SetActive(true);
        text.text = "Mission complete";
    }

    void NewLevelCountDown()
    {
        if (newLevelCountDown)
        {
            newLevelTimeLeft -= Time.deltaTime;
            newLevelTimeLeft = Mathf.Max(newLevelTimeLeft, 0);
            text.text = "Level " + (level + 2) + " in:\n" + ((int)newLevelTimeLeft).ToString();

            if (newLevelTimeLeft == 0)
            {
                newLevelCountDown = false;
                newLevelTimeLeft = newLevelTime;
                UI.SetActive(false);
                NextLevel();
            }
        }
    }

    public void StartLevel()
    {
        spawnEnemies = true;
        EnemyHealth.levelKills = 0;
        spawnedEnemies = 0;
        UI.SetActive(false);
    }

    void NextLevel()
    {
        spawnEnemies = true;
        level++;
        EnemyHealth.levelKills = 0;
        spawnedEnemies = 0;
    }

    void SpawnCountDown()
    {
        if (spawnEnemies)
        {
            spawnTimeLeft -= Time.deltaTime;

            if (spawnTimeLeft < 0 && spawnedEnemies < enemySettings.levels[level].nrOfEnemies)
            {
                m_spawner.Spawn(enemySettings.levels[level].speed);
                spawnedEnemies++;
                spawnTimeLeft = spawnInterval;
            }
            else if (spawnedEnemies == enemySettings.levels[level].nrOfEnemies)
            {
                spawnEnemies = false;
            }
        }
    }




}
