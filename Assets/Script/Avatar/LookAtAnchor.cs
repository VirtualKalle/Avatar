using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtAnchor : MonoBehaviour
{

    Camera mainCamera;

    // Use this for initialization
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Rotate()
    {
        if (!AvatarGameManager.paused)
        {
            Vector3 direction = Vector3.ProjectOnPlane(mainCamera.transform.forward, Vector3.up);
            transform.eulerAngles = new Vector3(0, -Vector3.SignedAngle(direction, Vector3.forward, Vector3.up), 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }
}
