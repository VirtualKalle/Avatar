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

    void Death(Vector3 force)
    {
        Destroy(GetComponent<Rigidbody>());
        foreach (GameObject go in bodyParts)
        {
            go.GetComponent<Collider>().enabled = true;
            Rigidbody rb = go.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(force / 3);

        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
