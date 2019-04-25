﻿using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;            // The amount of health the enemy starts the game with.
    public int currentHealth;                   // The current health the enemy has.
    public float sinkSpeed = 2.5f;              // The speed at which the enemy sinks through the floor when dead.
    public int scoreValue = 10;                 // The amount added to the player's score when the enemy dies.
    public AudioClip deathClip;                 // The sound to play when the enemy dies.
    Rigidbody rb;
    MeshSlicer[] m_meshSlicer;

    Animator anim;                              // Reference to the animator.
    AudioSource enemyAudio;                     // Reference to the audio source.
    ParticleSystem hitParticles;                // Reference to the particle system that plays when the enemy is damaged.
    Collider capsuleCollider;            // Reference to the capsule collider.
    bool isDead;                                // Whether the enemy is dead.
    bool isSinking;                             // Whether the enemy has started sinking through the floor.

    public delegate void EnemyDelegate(Vector3 force);
    public event EnemyDelegate deathEvent;
    [SerializeField] Slider healthBar;

    void Awake()
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
        capsuleCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        m_meshSlicer = GetComponentsInChildren<MeshSlicer>();

        // Setting the current health when the enemy first spawns.
        currentHealth = startingHealth;
    }

    private void OnEnable()
    {
        deathEvent += Death;
    }

    private void OnDisable()
    {
        deathEvent -= Death;
    }

    void Update()
    {

        healthBar.transform.LookAt(Camera.main.transform);

        // If the enemy should be sinking...
        if (isSinking)
        {
            // ... move the enemy down by the sinkSpeed per second.
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(int amount, Vector3 force)
    {
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
            deathEvent(force);
        }
    }


    void Death(Vector3 force)
    {
        // The enemy is dead.
        isDead = true;

        // Turn off health bar.
        healthBar.gameObject.SetActive(false);


        // Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
        GetComponent<Rigidbody>().isKinematic = true;


        // After 2 seconds destory the enemy.
        Destroy(gameObject, 5f);

    }

}