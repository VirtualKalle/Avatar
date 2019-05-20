using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemySettings", menuName = "EnemySettings", order = 1)]
public class EnemySettings : ScriptableObject
{
    [System.Serializable]
    public struct Level
    {
        public float speed;
        public int nrOfEnemies;
    }

    public Level[] levels;

}
