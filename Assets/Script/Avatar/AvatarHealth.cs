using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarHealth : MonoBehaviour {

    private int health = 100;
    private int currentHealth;
    [SerializeField] Slider healthBar;
    Rigidbody rb;
    public static bool isDead;
    [SerializeField] bool takeDamage;

    public delegate void PlayerDelegate();
    public static event PlayerDelegate deathEvent;
    Animator m_animator;

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

    // Use this for initialization
    void Start ()
    {
        currentHealth = health;
    }


    public void TakeDamage(int amount, Vector3 force)
    {
        if (!takeDamage)
        {
            return;
        }
        // If the enemy is dead...
        if (isDead)
            // ... no need to take damage so exit the function.
            return;

        // Play the hurt sound effect.
        //enemyAudio.Play();

        // Reduce the current health by the amount of damage sustained.
        currentHealth -= amount;
        healthBar.value = currentHealth;

        //Apply strike force
        rb.AddForce(force);

        //Debug.Log("<color=red>Damage force </color>" + force);

        // Set the position of the particle system to where the hit was sustained.
        //hitParticles.transform.position = hitPoint;

        // And play the particles.
        //hitParticles.Play();

        // If the current health is less than or equal to zero...
        if (currentHealth <= 0)
        {
            // ... the enemy is dead.
            deathEvent();
        }
    }


    void Death()
    {
        // The enemy is dead.
        isDead = true;

        // Turn off health bar.
        m_animator.applyRootMotion = true;
        m_animator.SetBool("dead", true);

        // Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
        GetComponent<Rigidbody>().isKinematic = true;


        // After 2 seconds destory the enemy.
        Destroy(gameObject, 5f);

    }
    // Update is called once per frame
    void Update () {
		
	}
}
