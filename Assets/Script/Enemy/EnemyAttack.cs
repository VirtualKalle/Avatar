using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    private bool inReach;
    AvatarHealth avatarHealth;
    Animator m_animator;
    float attackTimeLeft;
    float attackTime = 1;
    AudioSource m_audioSource;

    [SerializeField] GameObject hitParticle;
    [SerializeField] Transform hitPoint;


    void Awake()
    {
        avatarHealth = FindObjectOfType<AvatarHealth>();
        m_animator = GetComponent<Animator>();
        m_audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Avatar"))
        {
            inReach = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Avatar"))
        {
            inReach = false;
        }
    }

    void AttackCooldown()
    {
        if (inReach)
        {
            attackTimeLeft -= Time.deltaTime;
            if (attackTimeLeft < 0)
            {
                m_animator.SetBool("attack", true);
                attackTimeLeft = attackTime;
            }
        }
    }

    void Attack()
    {
        if (inReach)
        {
            avatarHealth.TakeDamage(10, transform.forward * 10f);
        }
        m_animator.SetBool("attack", false);
        m_audioSource.pitch = Random.Range(0.8f, 1.2f);
        m_audioSource.Play();
        GameObject go = Instantiate(hitParticle);
        go.transform.position = hitPoint.position;
        go.transform.localScale = hitPoint.lossyScale;
        Destroy(go, 1);
    }


    void Update()
    {
        if (!AvatarHealth.isDead)
        {
            AttackCooldown();
        }
    }
}
