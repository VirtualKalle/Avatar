using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Torque : MonoBehaviour {

    [SerializeField] Rigidbody go;
    [SerializeField] Rigidbody target;
    public Vector3 torque;
    [SerializeField] string torqueStr;
    [SerializeField] Text textComponent1;
    [SerializeField] Text textComponent2;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Torque2();

    }

    void Torque2()
    {
        Vector3 x = Vector3.Cross(go.transform.rotation.eulerAngles.normalized, target.transform.rotation.eulerAngles.normalized);
        float theta = Mathf.Asin(x.magnitude);
        Vector3 w = x.normalized * theta / Time.fixedDeltaTime;

        Quaternion q = go.transform.rotation * go.inertiaTensorRotation;
        torque = q * Vector3.Scale(go.inertiaTensor, (Quaternion.Inverse(q) * w));

        go.AddTorque(torque, ForceMode.Impulse);
    }

    void Torque1()
    {

        torque = RectifyAngleDifference(target.transform.eulerAngles) - RectifyAngleDifference(go.transform.eulerAngles);
    go.AddTorque(torque);
        textComponent1.text = "Target angle " + target.transform.eulerAngles.ToString();
        textComponent2.text = "Angle diff" + torque.ToString();
    }


    private Vector3 RectifyAngleDifference(Vector3 angdiff)
    {
        if (angdiff.x > 180) angdiff.x -= 360;
        if (angdiff.y > 180) angdiff.y -= 360;
        if (angdiff.z > 180) angdiff.z -= 360;
        return angdiff;
    }

}
