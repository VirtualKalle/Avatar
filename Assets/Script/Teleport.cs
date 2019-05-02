using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{


    [SerializeField] Transform laserPointer;
    [SerializeField] Transform m_CameraRig;
    [SerializeField] GameObject teleportPointer;
    private bool isPointing;
    private float joystickThreshold = 0.5f;
    Vector3 fwdDirection;
    Vector3 rightDirection;
    Vector3 direction;
    Camera m_Camera;

    // Use this for initialization
    void Start()
    {
        m_CameraRig = FindObjectOfType<OVRCameraRig>().transform;
        m_Camera = Camera.main;
    }


    void GetDirection()
    {
        //Vector3 fwdDirection = Vector3.Project((laserPointer.position - m_CameraRig.position), new Vector3(0, 1, 0)).normalized;
        fwdDirection = Vector3.ProjectOnPlane((laserPointer.position - m_Camera.transform.position), new Vector3(0, 1, 0)).normalized;
        Debug.Log("laserPointer.position " + laserPointer.position + ", m_CameraRig.position " + m_Camera.transform.position + ", fwdDirection " + fwdDirection);
        rightDirection = Vector3.Cross(fwdDirection, new Vector3(0, 1, 0)).normalized;
        Vector2 input = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
        direction = -input.x * rightDirection + input.y * fwdDirection;
    }

    void Twist()
    {
        //h1 = CrossPlatformInputManager.GetAxis("Horizontal"); // set as your inputs 
        float h1 = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x; // set as your inputs 
        float v1 = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y;

        if (h1 == 0f && v1 == 0f)
        { // this statement allows it to recenter once the inputs are at zero 
            Vector3 curRot = teleportPointer.transform.localEulerAngles; // the object you are rotating
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
            teleportPointer.transform.localEulerAngles = new Vector3(0f, Mathf.Atan2(h1, v1) * 180 / Mathf.PI, 0f); // this does the actual rotaion according to inputs
        }
    }

    void TeleportPlayer()
    {
        m_CameraRig.position = laserPointer.position - Vector3.ProjectOnPlane(m_Camera.transform.position - m_CameraRig.position, Vector3.up);
        float rotation = Vector3.Angle(direction, fwdDirection);
        //m_CameraRig.RotateAround(m_Camera.transform.position, Vector3.up, rotation);
    }

    void PointTeleporter()
    {
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstick))
        {

            if (!teleportPointer.activeSelf)
            {
                teleportPointer.SetActive(true);
            }

            if (!isPointing)
            {
                isPointing = true;
            }

            teleportPointer.transform.position = laserPointer.position;

            //Debug.Log(GetRotation());
            //GetDirection();
            //teleportPointer.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else if (isPointing && OVRInput.GetUp(OVRInput.Button.SecondaryThumbstick))
        {
            isPointing = false;
            teleportPointer.SetActive(false);
            TeleportPlayer();

        }
    }

    // Update is called once per frame
    void Update()
    {
        PointTeleporter();
    }
}

