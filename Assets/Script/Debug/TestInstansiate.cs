using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInstansiate : MonoBehaviour
{

    [SerializeField] GameObject prefab;
    Collider m_collider;

    void Start()
    {
        m_collider = GetComponent<Collider>();
    }


    void asdf()
    {
        m_collider.enabled = false;
        Instantiate(prefab, Vector3.zero, transform.rotation);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            asdf();
        }
    }
}
