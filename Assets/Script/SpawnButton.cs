using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButton : MonoBehaviour {

    Spawner m_spawner;
    [SerializeField] int idx;

    private void Awake()
    {
        m_spawner = GetComponentInParent<Spawner>();
    }

    // Use this for initialization
    void Start () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        m_spawner.Spawn(idx);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
