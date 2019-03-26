using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordProperties : MonoBehaviour {

    Rigidbody rb;
    [SerializeField] Transform centerOfMass;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
