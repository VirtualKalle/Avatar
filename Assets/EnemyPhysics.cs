using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPhysics : MonoBehaviour {

    [SerializeField] GameObject[] bodyParts;
    EnemyHealth m_enemyHealth;


	// Use this for initialization
	void Start ()
    {
        m_enemyHealth = GetComponent<EnemyHealth>();
        m_enemyHealth.deathEvent += Death;
    }

    private void OnDisable()
    {
        m_enemyHealth.deathEvent -= Death;
    }

    void Death()
    {
        Destroy(GetComponent<Rigidbody>());
        foreach (GameObject go in bodyParts)
        {
            go.AddComponent<Rigidbody>();
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
