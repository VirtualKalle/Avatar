using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Align : MonoBehaviour
{

    [SerializeField] Rigidbody go;
    public Vector3 torque;
    public Vector3 force;

    [SerializeField] string torqueStr;
    [SerializeField] Text textComponent1;
    [SerializeField] Text textComponent2;
    [SerializeField] float forceScaler;
    [SerializeField] float torqueScaler;

    Vector3 x;
    float theta;
    Vector3 w;
    Quaternion q;

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

    void TorqueUp()
    { 
        x = Vector3.Cross(go.transform.up.normalized, transform.up.normalized);
        theta = Mathf.Asin(x.magnitude);
        //w = x.normalized * theta / Time.fixedDeltaTime;
        w = x.normalized * theta;

        Quaternion q = go.transform.rotation * go.inertiaTensorRotation;
        torque = q * Vector3.Scale(go.inertiaTensor, (Quaternion.Inverse(q) * w)) * torqueScaler;

        go.AddTorque(torque, ForceMode.Impulse);

    }

    void TorqueRight()
    {
        x = Vector3.Cross(go.transform.right.normalized, transform.right.normalized);
        theta = Mathf.Asin(x.magnitude);
        //w = x.normalized * theta / Time.fixedDeltaTime;
        w = x.normalized * theta;

        Quaternion q = go.transform.rotation * go.inertiaTensorRotation;
        torque = q * Vector3.Scale(go.inertiaTensor, (Quaternion.Inverse(q) * w)) * torqueScaler;

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
