using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Align : MonoBehaviour
{

    [SerializeField] Rigidbody go;
    public Vector3 force;

    [SerializeField] float forceScaler;
    [SerializeField] float torqueScaler;

    private float limitAngle = 80;

    private float UpAngleDiff;
    private float UpAngleLimitScaler;
    private float RightAngleDiff;
    private float RightAngleLimitScaler;
    private float ForwardAngleDiff;
    private float ForwardAngleLimitScaler;
    private float theta;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Force();
        TorqueUp();
        TorqueRight();
        TorqueForward();
        GravityTorque();
    }

    void Force()
    {
        force = (transform.position - go.transform.position) * forceScaler;
        go.AddForceAtPosition(force,transform.position);
    }

    void GravityTorque()
    {
        Vector3 g = Vector3.Cross(go.transform.position - go.worldCenterOfMass, new Vector3(0, -1, 0) - go.worldCenterOfMass);
        go.AddTorque(-((go.position - go.worldCenterOfMass).magnitude * go.mass * 9.82f * g));
    }

    void TorqueForward()
    {
        Vector3 x = Vector3.Cross(go.transform.forward.normalized, transform.forward.normalized);
        float theta = Mathf.Asin(x.magnitude);
        Vector3 w = x.normalized * theta;

        Quaternion q = go.transform.rotation * go.inertiaTensorRotation;

        //ForwardAngleDiff = Vector3.Angle(go.transform.forward, transform.forward);

        //if (ForwardAngleDiff >= limitAngle + 1)
        //{
        //    ForwardAngleLimitScaler = (ForwardAngleDiff - limitAngle);
        //}
        //else
        //{
        //    ForwardAngleLimitScaler = 1;
        //}

        Vector3 torque = q * Vector3.Scale(go.inertiaTensor, (Quaternion.Inverse(q) * w)) * torqueScaler /** ForwardAngleLimitScaler*/;

        go.AddTorque(torque, ForceMode.Impulse);

    }

    void TorqueUp()
    {
        Vector3 x = Vector3.Cross(go.transform.up.normalized, transform.up.normalized);
        float theta = Mathf.Asin(x.magnitude);
        Vector3 w = x.normalized * theta;

        Quaternion q = go.transform.rotation * go.inertiaTensorRotation;

        //UpAngleDiff = Vector3.Angle(go.transform.up, transform.up);


        //if (UpAngleDiff >= limitAngle + 1)
        //{
        //    UpAngleLimitScaler = (UpAngleDiff - limitAngle);
        //}
        //else
        //{
        //    UpAngleLimitScaler = 1;
        //}

        Vector3 torque = q * Vector3.Scale(go.inertiaTensor, (Quaternion.Inverse(q) * w)) * torqueScaler /** UpAngleLimitScaler*/;

        go.AddTorque(torque, ForceMode.Impulse);

    }

    void TorqueRight()
    {
        Vector3 x = Vector3.Cross(go.transform.right.normalized, transform.right.normalized);
        float theta = Mathf.Asin(x.magnitude);
        Vector3 w = x.normalized * theta;

        Quaternion q = go.transform.rotation * go.inertiaTensorRotation;

        //RightAngleDiff = Vector3.Angle(go.transform.right, transform.right);
        
        //if (RightAngleDiff >= limitAngle + 1)
        //{
        //    RightAngleLimitScaler = (RightAngleDiff - limitAngle);
        //}
        //else
        //{
        //    RightAngleLimitScaler = 1;
        //}

        Vector3 torque = q * Vector3.Scale(go.inertiaTensor, (Quaternion.Inverse(q) * w)) * torqueScaler /** RightAngleLimitScaler*/;

        go.AddTorque(torque, ForceMode.Impulse);

    }

    

    private Vector3 RectifyAngleDifference(Vector3 angdiff)
    {
        if (angdiff.x > 180) angdiff.x -= 360;
        if (angdiff.y > 180) angdiff.y -= 360;
        if (angdiff.z > 180) angdiff.z -= 360;
        return angdiff;
    }

}
