using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

    [SerializeField] Rigidbody rb;
    [SerializeField] float velocity = 10f;
    private bool neutralized;


    // Use this for initialization
    void Start () {
        rb.velocity = transform.forward * velocity;
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (neutralized)
        {
            return;
        }

        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
     

        if (enemyHealth)
        {
        enemyHealth.TakeDamage(10, transform.forward * 1000);
        }

        End();
    }

    void End()
    {
        neutralized = true;
        Destroy(gameObject, 2);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
