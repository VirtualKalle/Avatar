using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeDamage : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit");
        EnemyHealth enemyHealth = collision.collider.GetComponentInParent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(10, collision.contacts[0].point);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
