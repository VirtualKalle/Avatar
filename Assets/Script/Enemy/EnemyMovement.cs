using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Transform player;
    EnemyHealth enemyHealth;
    Animator m_animator;
    NavMeshAgent nav;
    Rigidbody rb;
    bool isDead;

    private void OnEnable()
    {
        AvatarHealth.deathEvent += Immobilze;
        enemyHealth.deathEvent += Immobilze;
    }

    private void OnDisable()
    {
        AvatarHealth.deathEvent -= Immobilze;
        enemyHealth.deathEvent -= Immobilze;
    }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Avatar").transform;
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
        if (enemyHealth.currentHealth > 0 && !AvatarHealth.isDead)
        {
            nav.SetDestination(player.position);
        }
    }

    void Immobilze()
    {
        m_animator.SetBool("move", false);
        nav.enabled = false;
        rb.isKinematic = true;
        rb.useGravity = false;
    }

}