using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarHealth : MonoBehaviour {

    private int health = 100;
    private int currentHealth;

    Animator m_animator;
    Rigidbody rb;
    [SerializeField] Slider healthBar;

    public static bool isDead;
    [SerializeField] bool takeDamage;

    public delegate void PlayerDelegate();
    public static event PlayerDelegate deathEvent;


    private void OnEnable()
    {
        deathEvent += Death;
    }

    private void OnDisable()
    {
        deathEvent -= Death;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        m_animator = GetComponentInChildren<Animator>();
    }

    void Start ()
    {
        currentHealth = health;
    }


    public void TakeDamage(int amount, Vector3 force)
    {
        if (isDead || !takeDamage)
            return;

        currentHealth -= amount;
        healthBar.value = currentHealth;
        rb.AddForce(force);
        
        if (currentHealth <= 0)
        {
            deathEvent();
        }
    }

    void Death()
    {
        isDead = true;
        m_animator.applyRootMotion = true;
        m_animator.SetBool("dead", true);
        GetComponent<Rigidbody>().isKinematic = true;
        Destroy(gameObject, 5f);
    }

}
