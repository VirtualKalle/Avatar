using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSword : MonoBehaviour
{
    private bool activeSword;
    private bool isGrown;
    private Vector3 lastPos;
    private Vector3 currentVelocity;
    private Vector3 lastBladePosition;
    private Vector3 currentBladeSliceDirection;
    private float currentVelocityMag;
    private float swordStartThreshold = 4f;
    private float swordStopThreshold = 0.3f;
    private float spawnTrailTimeLeft;
    private float spawnTrailInterval = 0.02f;
    private float minForce = 100;
    private float maxForce = 1000;
    private float grownScale = 2f;
    [SerializeField] float scaleFactor = 1;

    [SerializeField] Material red;
    [SerializeField] Material blue;

    [SerializeField] GameObject blade;
    [SerializeField] GameObject trailObject;
    GameObject trailObjectClone;
    [SerializeField] Collider bladeCollider;
    Transform Avatar;
    AudioSource m_audioSorce;
    Renderer BladeRenderer;

    Item m_item;
    [SerializeField] ParticleSystem m_particles;
    [SerializeField] AudioClip swooshAudioClip;

    void Start()
    {
        if (scaleFactor == 0)
        {
            scaleFactor = 1;
        }
        m_item = GetComponent<Item>();
        Avatar = FindObjectOfType<AvatarRig>().transform;
        BladeRenderer = blade.GetComponent<Renderer>();
        m_audioSorce = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (m_item.m_itemState != ItemState.holstered && !AvatarGameManager.paused)
        {
            MeanVelocity();
            CheckVelocity();
            if (activeSword)
            {
                //SpawnTrail();
            }
        }

        if (m_item.m_itemState == ItemState.unholstered)
        {
            Grow();
        }
        else
        {
            Shrink();
        }

    }

    void Grow()
    {
        if (!isGrown)
        {
            transform.localScale *= grownScale;
            isGrown = true;
        }
    }

    void Shrink()
    {
        if (isGrown)
        {
            transform.localScale /= grownScale;
            isGrown = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && activeSword)
        {
            EnemyHealth enemyHealth = other.GetComponentInParent<EnemyHealth>();
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
        currentVelocity = (Avatar.InverseTransformPoint(transform.position) - lastPos) / Time.unscaledDeltaTime;
        currentVelocityMag = currentVelocity.magnitude;
        lastPos = Avatar.InverseTransformPoint(transform.position);
        currentBladeSliceDirection = (blade.transform.position - lastBladePosition) / Time.unscaledDeltaTime;
        lastBladePosition = blade.transform.position;
    }

    void CheckVelocity()
    {
        if (currentVelocityMag > swordStartThreshold && !activeSword && m_item.m_itemState == ItemState.unholstered)
        {
            BladeRenderer.material = red;
            bladeCollider.enabled = true;
            activeSword = true;
            m_audioSorce.PlayOneShot(swooshAudioClip);
            m_particles.Play();

            if (AvatarGameManager.bulletTime)
            {
                m_audioSorce.pitch = 0.5f;
            }
            else
            {
                m_audioSorce.pitch = 1f;
            }
        }
        else if ((currentVelocityMag < swordStopThreshold || m_item.m_itemState != ItemState.unholstered) && activeSword)
        {
            m_particles.Stop();
            BladeRenderer.material = blue;
            bladeCollider.enabled = false;
            activeSword = false;
        }
    }

    void SpawnTrail()
    {
        spawnTrailTimeLeft -= Time.unscaledDeltaTime;
        if (spawnTrailTimeLeft < 0 || (trailObjectClone != null && Vector3.Distance(trailObjectClone.transform.position, transform.position) > 0.02f))
        {
            trailObjectClone = Instantiate(trailObject, transform.position, transform.rotation);
            trailObjectClone.transform.localScale = transform.lossyScale * scaleFactor;
            Destroy(trailObjectClone, 0.1f * Time.timeScale);
            spawnTrailTimeLeft = spawnTrailInterval;
        }

    }

}
