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

    private void OnCollisionEnter(Collision collision)
    {
        m_spawner.Spawn(idx);
    }

}
