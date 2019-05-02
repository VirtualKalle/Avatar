using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Transform player;               // Reference to the player's position.
    //PlayerHealth playerHealth;      // Reference to the player's health.
    EnemyHealth enemyHealth;
    Animator m_animator;   // Reference to this enemy's health.
    NavMeshAgent nav;               // Reference to the nav mesh agent.
    Rigidbody rb;
    bool isDead;

    private void OnEnable()
    {
        AvatarHealth.deathEvent += Death;
        enemyHealth.deathEvent += Death;
    }

    private void OnDisable()
    {
        AvatarHealth.deathEvent -= Death;
        enemyHealth.deathEvent -= Death;
    }

    void Awake()
    {
        // Set up the references.
        player = GameObject.FindGameObjectWithTag("Avatar").transform;
        //playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        rb = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();

        nav = GetComponent<NavMeshAgent>();
    }


    private void Start()
    {
        ScaleNav();
        m_animator.SetBool("move", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Avatar"))
        {
            m_animator.SetBool("move", false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Avatar"))
        {
            m_animator.SetBool("move", true);
        }
    }

    void ScaleNav()
    {
        nav.acceleration *= AvatarGameManager.worldScale;
        nav.speed *= AvatarGameManager.worldScale;
        nav.stoppingDistance *= AvatarGameManager.worldScale;

    }

    void Update()
    {
        //If the enemy and the player have health left...
        if (enemyHealth.currentHealth > 0 && !AvatarHealth.isDead)
        {
            //... set the destination of the nav mesh agent to the player.
            nav.SetDestination(player.position);
        }
    }

    void Death()
    {
        m_animator.SetBool("move", false);
        nav.enabled = false;
        rb.isKinematic = true;
        rb.useGravity = false;
    }
}