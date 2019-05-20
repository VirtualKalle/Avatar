using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    bool isDead;
    public bool spawning;

    public int startingHealth = 100;
    public int currentHealth;

    Rigidbody m_rigidbody;
    EnemyPhysics m_EnemyPhysics;
    [SerializeField] Slider healthBar;

    public delegate void EnemyDelegate();
    public event EnemyDelegate deathEvent;
    public static event EnemyDelegate destroyEvent;
    public static int levelKills;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_EnemyPhysics = GetComponent<EnemyPhysics>();
    }

    private void OnDestroy()
    {
        destroyEvent();
    }

    private void Start()
    {
        currentHealth = startingHealth;
    }

    void Update()
    {
        healthBar.transform.LookAt(Camera.main.transform);
    }

    public void TakeDamage(int amount, Vector3 force)
    {
        if (isDead || spawning)
            return;

        currentHealth -= amount;
        healthBar.value = currentHealth;
        m_rigidbody.AddForce(force);

        if (currentHealth <= 0)
        {
            Death();
            deathEvent();
            levelKills++;
            m_EnemyPhysics.DeathPhysics(force);
        }
    }


    void Death()
    {
        isDead = true;
        healthBar.gameObject.SetActive(false);
        GetComponent<Animator>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        Destroy(gameObject, 5f);
    }

}