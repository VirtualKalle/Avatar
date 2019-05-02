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


    // Use this for initialization
    void Awake()
    {
        avatarHealth = FindObjectOfType<AvatarHealth>();
        m_animator = GetComponent<Animator>();
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
    }

    // Update is called once per frame
    void Update()
    {
        if (!AvatarHealth.isDead)
        {
            AttackCooldown();
        }

    }
}
