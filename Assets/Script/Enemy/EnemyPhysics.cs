using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPhysics : MonoBehaviour
{

    EnemyHealth m_enemyHealth;
    Rigidbody rb;
    [SerializeField] List<Rigidbody> bodyParts;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        bodyParts = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());

        for (int i = 0; i < bodyParts.Count; i++)
        {
            if (bodyParts[i].gameObject.GetInstanceID() == gameObject.GetInstanceID())
            {
                bodyParts.RemoveAt(i);
            }
        }
    }


    public void DeathPhysics(Vector3 force)
    {
        foreach (Rigidbody rb in bodyParts)
        {
            rb.GetComponentInChildren<Collider>().enabled = true;
            Rigidbody child_rb = rb.GetComponent<Rigidbody>();
            child_rb.isKinematic = false;
            child_rb.AddForce(force/3, ForceMode.Force);
        }

        Destroy(rb);
    }

}
