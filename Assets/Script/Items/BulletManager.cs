using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{

    [SerializeField] Rigidbody rb;
    [SerializeField] float velocity = 10f;
    private bool neutralized;
    TrailRenderer trail;


    private void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();
    }

    // Use this for initialization
    void Start()
    {
        rb.velocity = transform.forward * velocity * AvatarGameManager.worldScale;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (neutralized)
        {
            return;
        }

        EnemyHealth enemyHealth;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!collision.transform.parent.GetComponent<EnemyHealth>())
            {
                enemyHealth = collision.transform.parent.GetComponentInParent<EnemyHealth>();
            }
            else
            {

                enemyHealth = collision.transform.parent.GetComponent<EnemyHealth>();
            }

            enemyHealth.TakeDamage(40, (enemyHealth.transform.position - transform.position).normalized * 1000);
            MeshSlicer meshSlicer = collision.gameObject.GetComponentInParent<MeshSlicer>();

            if (meshSlicer != null && enemyHealth.currentHealth <= 0)
            {

                List<Vector3> cutPlanes = new List<Vector3>();

                cutPlanes.Add(transform.TransformDirection(new Vector3(1, Random.Range(-0.2f, 0.2f), 0)));
                cutPlanes.Add(transform.TransformDirection(new Vector3(1, Random.Range(-1f, 1f), Random.Range(-0.2f, 0.2f))));
                cutPlanes.Add(transform.TransformDirection(new Vector3(Random.Range(-0.2f, 0.2f), 1, 0)));

                meshSlicer.TryCut(transform.position, cutPlanes);
            }
        }

        End();
    }

    void End()
    {
        neutralized = true;
        Destroy(gameObject, 0.5f);
        trail.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
