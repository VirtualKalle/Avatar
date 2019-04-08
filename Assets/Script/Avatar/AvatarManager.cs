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



    // Update is called once per frame
    void Update () {
        FollowTransform();
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


}
