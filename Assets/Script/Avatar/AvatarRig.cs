using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarRig : MonoBehaviour
{

    [SerializeField] Transform handRightAvatar;
    [SerializeField] Transform handLeftAvatar;
    [SerializeField] Transform headAvatar;
    [SerializeField] Transform bodyAvatar;

    [SerializeField] Transform handRight;
    [SerializeField] Transform handLeft;

    [SerializeField] Transform head;
    [SerializeField] Transform headRig;

    
    void Start()
    {
        if (handLeft == null)
            handLeft = GameObject.Find("LeftHandAnchor").transform;

        if (handRight == null)
            handRight = GameObject.Find("RightHandAnchor").transform;

        if (head == null)
            head = GameObject.Find("CenterEyeAnchor").transform;

    }

    void Update()
    {
        if (!AvatarGameManager.paused && !AvatarHealth.isDead)
        {
            FollowTransform();
        }
    }

    void FollowTransform()
    {
        Camera camera = Camera.main;
        headAvatar.rotation = head.rotation;
        headRig.localRotation = headAvatar.localRotation;

        bodyAvatar.localPosition = headAvatar.localPosition + new Vector3(0, -1f, 0);

        handRightAvatar.position = headAvatar.TransformPoint(head.transform.InverseTransformPoint(handRight.position));
        handRightAvatar.rotation = handRight.rotation;

        handLeftAvatar.position = headAvatar.TransformPoint(head.transform.InverseTransformPoint(handLeft.position));
        handLeftAvatar.rotation = handLeft.rotation;
    }
}