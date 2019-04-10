using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour {

    [SerializeField] Transform trans;
    public float speed = 5;
    [SerializeField] bool fix;
    [SerializeField] bool local;

    // Use this for initialization
    void Start () {
		
	}

    private void OnEnable()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update () {
        if (fix && local)
        {
            transform.localPosition = trans.position;
            transform.localRotation = trans.rotation;
        }
        else
        {
        transform.position = Vector3.Lerp(transform.position, trans.position, Time.deltaTime * speed);
        transform.rotation = Quaternion.Lerp(transform.rotation, trans.rotation, Time.deltaTime * speed);
        }
	}
}
