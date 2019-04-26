using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSword : MonoBehaviour
{
    Item m_item;
    [SerializeField] GameObject blade;
    Renderer BladeRenderer;
    [SerializeField] Material red;
    [SerializeField] Material blue;
    [SerializeField] Collider bladeCollider;

    private bool activeSword;
    private Vector3 lastPos;
    private Vector3 currentVelocity;
    private Vector3 lastBladeSliceDirection;
    private Vector3 currentBladeSliceDirection;
    private float currentVelocityMag;
    private float meanTimeLeft = 0.1f;
    private float meanTimeInterval = 0.05f;
    private float swordStartThreshold = 2f;
    private float swordStopThreshold = 0.5f;
    private float spawnTrailTimeLeft;
    private float spawnTrailInterval = 0.02f;
    [SerializeField] GameObject trailObject;
    GameObject trailObjectClone;
    Transform Avatar;
    private float maxForce = 1000;
    private float minForce = 100;
    [SerializeField] float scaleFactor = 1;

    [SerializeField] AudioClip swooshAudioClip;
    AudioSource m_audioSorce;

    // Use this for initialization
    void Start()
    {
        if (scaleFactor == 0)
        {
            scaleFactor = 1;
        }
        m_item = GetComponent<Item>();
        Avatar = FindObjectOfType<AvatarManager>().transform;
        BladeRenderer = blade.GetComponent<Renderer>();
        m_audioSorce = GetComponent<AudioSource>();
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
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();
            //Vector3 force = currentVelocity * 50;
            Vector3 force = transform.forward * 100;
            int damage = (int)currentVelocity.magnitude * 3;

            damage = damage > 100 ? 100 : damage;
            damage = damage < 100 ? 100 : damage;

            force = force.magnitude > maxForce ? force.normalized * maxForce : force;
            force = force.magnitude < minForce ? force.normalized * minForce : force;
            enemyHealth.TakeDamage(damage, force);

            MeshSlicer meshSlicer = other.GetComponentInParent<MeshSlicer>();
            if (meshSlicer != null && activeSword && enemyHealth.currentHealth <= 0)
            {
                meshSlicer.TryCut(transform.position, Vector3.Cross(transform.forward, currentBladeSliceDirection.normalized));
            }
        }
    }

    void MeanVelocity()
    {
        meanTimeLeft -= Time.unscaledDeltaTime;

        if (meanTimeLeft < 0)
        {
            //currentVelocity = (transform.position - Avatar.position - lastPos) / Time.unscaledDeltaTime;
            currentVelocity = (Avatar.InverseTransformPoint(transform.position) - lastPos) / Time.unscaledDeltaTime;

            currentVelocityMag = currentVelocity.magnitude;
            //Debug.Log("currentVelocity " + currentVelocity + "\n currentVelocityMag " + currentVelocity.magnitude);
            lastPos = Avatar.InverseTransformPoint(transform.position);

            currentBladeSliceDirection = (blade.transform.position - lastBladeSliceDirection) / Time.unscaledDeltaTime;
            lastBladeSliceDirection = blade.transform.position;

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
            m_audioSorce.PlayOneShot(swooshAudioClip);
            if (AvatarGameManager.bulletTime)
            {
                m_audioSorce.pitch = 0.5f;
            }
            else
            {
                m_audioSorce.pitch = 1f;
            }
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
            trailObjectClone.transform.localScale = transform.lossyScale * scaleFactor;
            Destroy(trailObjectClone, 0.2f);
            spawnTrailTimeLeft = spawnTrailInterval;
        }

    }


}
