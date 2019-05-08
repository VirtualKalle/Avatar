using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsAvatarManager : MonoBehaviour {

    [SerializeField] Rigidbody handRightAvatar;
    [SerializeField] Rigidbody handLeftAvatar;

    [SerializeField] Transform handRight;
    [SerializeField] Transform handLeft;


    [SerializeField] Transform head;
    [SerializeField] float forceScaler;
    
	void Update ()
    {
        FollowTransform();
    }

    void FollowTransform()
    {
        Debug.Log(handRight.transform.position - handRightAvatar.transform.position);
        handRightAvatar.AddForce((handRight.transform.position - handRightAvatar.transform.position) * forceScaler);
        handRightAvatar.AddTorque((handRight.transform.rotation.eulerAngles - handRightAvatar.transform.rotation.eulerAngles) * forceScaler);
    }

}
