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
            Vector3 direction = Vector3.ProjectOnPlane(mainCamera.transform.forward, new Vector3(0, 1, 0));
            transform.eulerAngles = new Vector3(0, -Vector3.SignedAngle(direction, new Vector3(0, 0, 1), new Vector3(0, 1, 0)), 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();

    }
}
