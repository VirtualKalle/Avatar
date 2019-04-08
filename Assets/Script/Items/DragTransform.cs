using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragTransform : MonoBehaviour {

    [SerializeField] Transform trans;
    [SerializeField] float speed = 5;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        trans.position = Vector3.Lerp(trans.position, transform.position, Time.deltaTime * speed);
        trans.rotation = Quaternion.Lerp(trans.rotation, transform.rotation, Time.deltaTime * speed);
	}
}
