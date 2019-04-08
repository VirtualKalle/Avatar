using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularBlade : MonoBehaviour {

    [SerializeField] Renderer BladeRenderer;
    [SerializeField] Material red;
    [SerializeField] Material blue;
    [SerializeField] Collider bladeCollider;

    private float prevVelocity;
    private bool activeSword;
    private Vector3 lastPos;
    private Vector3 currentVelocity;
    private float currentVelocityMag;
    private float meanTimeLeft = 0.1f;
    private float meanTimeInterval = 0.1f;
    private float swordThreshold = 1f;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        MeanVelocity();
        CheckVelocity();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Swiftblade trigger enter " + other.transform.name);
        EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(10, currentVelocity * 1000);
        }
    }

    void MeanVelocity()
    {
        meanTimeLeft -= Time.deltaTime;

        if (meanTimeLeft < 0)
        {
            currentVelocity = (transform.position - lastPos) / Time.deltaTime;
            currentVelocityMag = currentVelocity.magnitude;
            lastPos = transform.position;
            meanTimeLeft = meanTimeInterval;
        }

    }

    void CheckVelocity()
    {
        if (currentVelocityMag > swordThreshold && !activeSword)
        {
            Debug.Log("red");
            BladeRenderer.material = red;
            bladeCollider.enabled = true;
            activeSword = true;
        }
        else if (currentVelocityMag < swordThreshold && activeSword)
        {
            Debug.Log("blue");
            BladeRenderer.material = blue;
            bladeCollider.enabled = false;
            activeSword = false;
        }

        lastPos = transform.position;
        
        

    }
}
