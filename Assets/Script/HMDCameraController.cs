using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMDCameraController : MonoBehaviour {

    private bool rotated;
    [SerializeField] Transform avatar;
    Vector3 Threshold = new Vector3(1,1,1);
    float rotateThreshold = 0.1f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Rotate();
        Move();
    }

    private void Move()
    {
        if (avatar.position.x - transform.position.x > Threshold.x)
        {
            //Debug.Log("Move camera right");
            transform.position = new Vector3(avatar.position.x - Threshold.x, transform.position.y, transform.position.z);
        }
        else if (avatar.position.x - transform.position.x < -Threshold.x)
        {
            //Debug.Log("Move camera left");
            transform.position = new Vector3(avatar.position.x + Threshold.x, transform.position.y, transform.position.z);
        }


        if (avatar.position.z - transform.position.z > Threshold.z)
        {
            //Debug.Log("Move camera fwd");
            transform.position = new Vector3(transform.position.x , transform.position.y, avatar.position.z - Threshold.z);
        }
        else if (avatar.position.z - transform.position.z < -Threshold.z)
        {
            //Debug.Log("Move camera bwd");
            transform.position = new Vector3(transform.position.x, transform.position.y, avatar.position.z + Threshold.z);
        }
    }

    void Rotate()
    {

        float h1 = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x; // set as your inputs 

        if ((h1 > rotateThreshold || Input.GetKey(KeyCode.E)) && !rotated) //Rotate right
        {
            transform.RotateAround(avatar.position, Vector3.up, 90);
            rotated = true;
        }
        else if ((h1 < -rotateThreshold || Input.GetKey(KeyCode.Q)) && !rotated)
        {
            //transform.Rotate(Vector3.up, -90);
            transform.RotateAround(avatar.position, Vector3.up, -90);
            rotated = true;
        }
        else if (h1 > -rotateThreshold && h1 < rotateThreshold && !Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Q) && rotated)
        {
            rotated = false;
        }
    }

}
