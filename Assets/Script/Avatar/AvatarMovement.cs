using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Move();
        Twist();
    }

    private void Move()
    {
        //Debug.Log("MoVE" + OVRInput.Get(OVRInput.RawAxis2D.RThumbstick));
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).magnitude > 0.1f)
        {
            Debug.Log("Input thumpstick");

            Vector3 fwd = Vector3.ProjectOnPlane((transform.position - Camera.main.transform.position), Vector3.up).normalized;
            transform.Translate(OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y * fwd * Time.deltaTime, Space.World);


            Vector3 right = Vector3.Cross((transform.position - Camera.main.transform.position), Vector3.up).normalized;
            transform.Translate(-OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x * right * Time.deltaTime, Space.World);
            //transform.Translate(new Vector3(OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x, 0, OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y) * Time.deltaTime, Space.World);

        }
    }

    void Twist()
    {
        //h1 = CrossPlatformInputManager.GetAxis("Horizontal"); // set as your inputs 
        float h1 = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x; // set as your inputs 
        float v1 = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y;

        if (h1 == 0f && v1 == 0f)
        { // this statement allows it to recenter once the inputs are at zero 
            Vector3 curRot = transform.localEulerAngles; // the object you are rotating
            Vector3 homeRot;
            if (curRot.y > 180f)
            { // this section determines the direction it returns home 
                //Debug.Log(curRot.y);
                homeRot = new Vector3(0f, 359.999f, 0f); //it doesnt return to perfect zero 
            }
            else
            {                                                                      // otherwise it rotates wrong direction 
                homeRot = Vector3.zero;
            }
            //transform.localEulerAngles = Vector3.Slerp(curRot, homeRot, Time.deltaTime * 2);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0f, Mathf.Atan2(h1, v1) * 180 / Mathf.PI, 0f); // this does the actual rotaion according to inputs
        }
    }

}
