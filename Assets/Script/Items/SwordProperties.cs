using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordProperties : MonoBehaviour
{

    Rigidbody rb;
    [SerializeField] Transform centerOfMass;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;
	}
	
}