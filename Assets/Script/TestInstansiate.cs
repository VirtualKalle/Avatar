using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInstansiate : MonoBehaviour {

    [SerializeField] GameObject prefab;
    Collider m_collider;
	// Use this for initialization
	void Start () {
        m_collider = GetComponent<Collider>();

    }
	

    void asdf()
    {
        m_collider.enabled = false;
        GameObject leftGO = Instantiate(prefab,Vector3.zero, transform.rotation);
    }

	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.W))
        {
            asdf();
        }
    }
}
