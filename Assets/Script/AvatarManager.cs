using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarManager : MonoBehaviour {

    [SerializeField] Transform handRightAvatar;
    [SerializeField] Transform handLeftAvatar;
    [SerializeField] Transform headAvatar;
    [SerializeField] Transform bodyAvatar;

    [SerializeField] Transform handRight;
    [SerializeField] Transform handLeft;


    [SerializeField] Transform head;

    //[SerializeField] Transform body;
    //Vector3 bodyRelativePosition;
    //Quaternion bodyRelativeRotation;


    // Use this for initialization
    void Start () {
		
	}

    private void Move()
    {
        //Debug.Log("MoVE" + OVRInput.Get(OVRInput.RawAxis2D.RThumbstick));
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).magnitude > 0.1f)
        {
            Debug.Log("Input thumpstick");
            //transform.Translate(new Vector3(0, 0, OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y) * Time.deltaTime, Space.Self);
            transform.Translate(new Vector3(OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x, 0, OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y) * Time.deltaTime, Space.World);

            //transform.Rotate(0, OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x * 2, 0, Space.Self);
        }
    }

    // Update is called once per frame
    void Update () {
        FollowTransform();
        Move();
        Twist();
    }

    void FollowTransform()
    {
        //headAvatar.localPosition = head.localPosition;
        headAvatar.localRotation = head.localRotation;

        bodyAvatar.localPosition = headAvatar.localPosition + new Vector3(0, -1f, 0);

        //handRightAvatar.localPosition = handRight.position - head.position + headAvatar.localPosition;
        handRightAvatar.position = headAvatar.TransformPoint(head.transform.InverseTransformPoint(handRight.position));
        handRightAvatar.localRotation = handRight.localRotation;

        handLeftAvatar.position = headAvatar.TransformPoint(head.transform.InverseTransformPoint(handLeft.position));
        handLeftAvatar.localRotation = handLeft.localRotation;
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
