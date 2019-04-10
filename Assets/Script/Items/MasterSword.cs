using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSword : MonoBehaviour
{
    Item m_item;
    [SerializeField] Renderer BladeRenderer;
    [SerializeField] Material red;
    [SerializeField] Material blue;
    [SerializeField] Collider bladeCollider;
    [SerializeField] GameObject SlaveSword;
    GameObject slaveClone;

    private float prevVelocity;
    private bool activeSword;
    private Vector3 lastPos;
    private Vector3 currentVelocity;
    private float currentVelocityMag;
    private float meanTimeLeft = 0.1f;
    private float meanTimeInterval = 0.05f;
    private float swordStartThreshold = 1f;
    private float swordStopThreshold = 0.5f;
    private float spawnTrailTimeLeft;
    private float spawnTrailInterval = 0.02f;
    [SerializeField] GameObject trailObject;
    GameObject trailObjectClone;
    Transform Avatar;
    private float maxForce = 1000;

    // Use this for initialization
    void Start()
    {
        m_item = GetComponent<Item>();
        Avatar = FindObjectOfType<AvatarManager>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_item.m_itemState != ItemState.holstered)
        {
            MeanVelocity();
            CheckVelocity();
            if (activeSword)
            {
                SpawnTrail();

            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Swiftblade trigger enter " + other.transform.name);
        EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();
        if (enemyHealth != null)
        {
            Vector3 force = currentVelocity * 50;
            int damage = (int) currentVelocity.magnitude;
            force = force.magnitude > maxForce ? force.normalized * maxForce : force;
            enemyHealth.TakeDamage(damage, force);
        }
    }

    void MeanVelocity()
    {
        meanTimeLeft -= Time.deltaTime;

        if (meanTimeLeft < 0)
        {
            currentVelocity = (Avatar.InverseTransformPoint(transform.position) - lastPos) / Time.deltaTime;
            //Debug.Log("currentVelocity " + currentVelocity);

            currentVelocityMag = currentVelocity.magnitude;
            lastPos = Avatar.InverseTransformPoint(transform.position);
            meanTimeLeft = meanTimeInterval;
        }

    }

    void CheckVelocity()
    {
        if (currentVelocityMag > swordStartThreshold && !activeSword && m_item.m_itemState == ItemState.unholstered)
        {
            //Debug.Log("red");
            BladeRenderer.material = red;
            bladeCollider.enabled = true;
            activeSword = true;
        }
        else if (currentVelocityMag < swordStopThreshold && activeSword || m_item.m_itemState != ItemState.unholstered)
        {
            //Debug.Log("blue");
            BladeRenderer.material = blue;
            bladeCollider.enabled = false;
            activeSword = false;
        }

    }


    void SpawnTrail()
    {
        spawnTrailTimeLeft -= Time.unscaledDeltaTime;
        if (spawnTrailTimeLeft < 0 || (trailObjectClone != null && Vector3.Distance(trailObjectClone.transform.position, transform.position) > 0.05f))
        {
            trailObjectClone = Instantiate(trailObject, transform.position, transform.rotation);
            trailObjectClone.transform.localScale = transform.lossyScale;
            Destroy(trailObjectClone, 0.2f);
            spawnTrailTimeLeft = spawnTrailInterval;
        }

    }


}
