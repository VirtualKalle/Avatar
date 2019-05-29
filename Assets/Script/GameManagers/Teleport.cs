using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{

    [SerializeField] Transform laserPointer;
    [SerializeField] Transform m_CameraRig;
    [SerializeField] GameObject teleportPointer;
    [SerializeField] LineRenderer m_lineRenderer;
    private bool isPointing;
    Camera m_Camera;

    void Start()
    {
        m_CameraRig = FindObjectOfType<OVRCameraRig>().transform;
        m_Camera = Camera.main;
    }

    void Update()
    {
        PointTeleporter();
    }

    void Twist()
    {
        float h1 = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x;
        float v1 = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y;

        if (h1 == 0f && v1 == 0f)
        {
            Vector3 curRot = teleportPointer.transform.localEulerAngles;
            Vector3 homeRot;
            if (curRot.y > 180f)
            {
                homeRot = new Vector3(0f, 359.999f, 0f);
            }
            else
            {
                homeRot = Vector3.zero;
            }
        }
        else
        {
            teleportPointer.transform.localEulerAngles = new Vector3(0f, Mathf.Atan2(h1, v1) * 180 / Mathf.PI, 0f);
        }
    }

    void TeleportPlayer()
    {
        m_CameraRig.position = laserPointer.position - Vector3.ProjectOnPlane(m_Camera.transform.position - m_CameraRig.position, Vector3.up);
    }

    void PointTeleporter()
    {
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstick))
        {

            if (!teleportPointer.activeSelf)
            {
                teleportPointer.SetActive(true);
            }

            if (!m_lineRenderer.enabled)
            {
                m_lineRenderer.enabled = true;
            }

            if (!isPointing)
            {
                isPointing = true;
            }

            teleportPointer.transform.position = laserPointer.position;
        }
        else if (isPointing && OVRInput.GetUp(OVRInput.Button.SecondaryThumbstick))
        {
            isPointing = false;
            teleportPointer.SetActive(false);
            m_lineRenderer.enabled = false;
            TeleportPlayer();
        }
    }

}

