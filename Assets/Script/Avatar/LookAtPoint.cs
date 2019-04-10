using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPoint : MonoBehaviour {
    private Camera mainCamera;

    // Use this for initialization
    void Start () {
        mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {

        //MoveToPoint();

    }

    void MoveToPoint()
    {
        RaycastHit hit;
        Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 100);
        transform.position = hit.point;
    }

}

