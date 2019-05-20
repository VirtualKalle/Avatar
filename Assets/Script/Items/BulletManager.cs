using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{

    [SerializeField] float velocity = 10f;
    private bool neutralized;
    [SerializeField] GameObject hitParticle;
    TrailRenderer trail;
    [SerializeField] Rigidbody rb;

    private void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();
    }

    void Start()
    {
        rb.velocity = transform.forward * velocity * AvatarGameManager.worldScale;
    }

    private void OnCollisionEnter(Collision collision)
    {

        EnemyHealth enemyHealth = collision.transform.GetComponentInParent<EnemyHealth>();
        MeshSlicer meshSlicer = collision.gameObject.GetComponentInParent<MeshSlicer>();
        if (enemyHealth != null)
        {

            enemyHealth.TakeDamage(40, transform.forward * 100);

            if (meshSlicer != null && enemyHealth.currentHealth <= 0)
            {
                List<Vector3> cutPlanes = new List<Vector3>();

                cutPlanes.Add(transform.TransformDirection(new Vector3(1, Random.Range(-0.2f, 0.2f), 0)));
                cutPlanes.Add(transform.TransformDirection(new Vector3(1, Random.Range(-1f, 1f), Random.Range(-0.2f, 0.2f))));
                cutPlanes.Add(transform.TransformDirection(new Vector3(Random.Range(-0.2f, 0.2f), 1, 0)));

                meshSlicer.TryCut(transform.position, cutPlanes);
            }
        }
        else if (meshSlicer != null)
        {
            List<Vector3> cutPlanes = new List<Vector3>();

            cutPlanes.Add(transform.TransformDirection(new Vector3(1, Random.Range(-0.2f, 0.2f), 0)));
            cutPlanes.Add(transform.TransformDirection(new Vector3(1, Random.Range(-1f, 1f), Random.Range(-0.2f, 0.2f))));
            cutPlanes.Add(transform.TransformDirection(new Vector3(Random.Range(-0.2f, 0.2f), 1, 0)));

            meshSlicer.TryCut(transform.position, cutPlanes);
        }


        End();
    }

    void End()
    {
        GameObject hit = Instantiate(hitParticle,transform.position,transform.rotation);
        hit.transform.localScale = transform.lossyScale;
        Destroy(hit, 1);
        Destroy(gameObject);
    }
}
