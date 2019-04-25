using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarManager : MonoBehaviour
{

    [SerializeField] Transform handRightAvatar;
    [SerializeField] Transform handLeftAvatar;
    [SerializeField] Transform headAvatar;
    [SerializeField] Transform bodyAvatar;

    [SerializeField] Transform handRight;
    [SerializeField] Transform handLeft;


    [SerializeField] Transform head;
    [SerializeField] Transform headRig;

    //[SerializeField] Transform body;
    //Vector3 bodyRelativePosition;
    //Quaternion bodyRelativeRotation;


    // Use this for initialization
    void Start()
    {
        if (handLeft == null)
            handLeft = GameObject.Find("LeftHandAnchor").transform;

        if (handRight == null)
            handRight = GameObject.Find("RightHandAnchor").transform;

        if (head == null)
            head = GameObject.Find("CenterEyeAnchor").transform;

    }



    // Update is called once per frame
    void Update()
    {
        FollowTransform();
    }

    void FollowTransform()
    {
        Camera camera = Camera.main;
        Vector3 direction = Vector3.ProjectOnPlane(headAvatar.transform.position - camera.transform.position, new Vector3(0, 1, 0));

        headAvatar.localRotation = head.localRotation;

        headRig.localRotation = headAvatar.localRotation;

        bodyAvatar.localPosition = headAvatar.localPosition + new Vector3(0, -1f, 0);

        handRightAvatar.position = headAvatar.TransformPoint(head.transform.InverseTransformPoint(handRight.position));
        handRightAvatar.RotateAround(headAvatar.position, new Vector3(0, 1, 0), -Vector3.SignedAngle(new Vector3(0, 0, 1), direction, new Vector3(0, 1, 0)));


        handRightAvatar.localRotation = handRight.localRotation;
        handRightAvatar.RotateAround(handRightAvatar.position, new Vector3(0, 1, 0), -Vector3.SignedAngle(new Vector3(0, 0, 1), direction, new Vector3(0, 1, 0)));


        handLeftAvatar.position = headAvatar.TransformPoint(head.transform.InverseTransformPoint(handLeft.position));
        handLeftAvatar.RotateAround(headAvatar.position, new Vector3(0, 1, 0), -Vector3.SignedAngle(new Vector3(0, 0, 1), direction, new Vector3(0, 1, 0)));


        handLeftAvatar.localRotation = handLeft.localRotation;
        handLeftAvatar.RotateAround(handLeftAvatar.position, new Vector3(0, 1, 0), -Vector3.SignedAngle(new Vector3(0, 0, 1), direction, new Vector3(0, 1, 0)));

    }
       
}
