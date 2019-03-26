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
        Debug.Log("MoVE" + OVRInput.Get(OVRInput.RawAxis2D.RThumbstick));
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).magnitude > 0.1f)
        {
            Debug.Log("Input thumpstick");
            transform.Translate(new Vector3(0, 0, OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y) * Time.deltaTime, Space.Self);

            transform.Rotate(0, OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x * 10,0, Space.Self);
        }
    }

    // Update is called once per frame
    void Update () {
        FollowTransform();
        Move();
    }

    void FollowTransform()
    {
        //headAvatar.localPosition = head.position;
        //headAvatar.localRotation = head.localRotation;

        //bodyAvatar.localPosition = headAvatar.localPosition + new Vector3(0,-1f,0);

        //handRightAvatar.localPosition = handRight.position - head.position + headAvatar.localPosition;
        handRightAvatar.localPosition = handRight.localPosition + headAvatar.localPosition;
        handRightAvatar.localRotation = handRight.localRotation;

        handLeftAvatar.localPosition = handLeft.position - head.position + headAvatar.localPosition;
        handLeftAvatar.localRotation = handLeft.rotation;
    }

}
