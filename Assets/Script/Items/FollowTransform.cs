using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{

    [SerializeField] bool fix;
    [SerializeField] bool local;

    public float speed = 5;

    [SerializeField] Transform trans;

    private void OnEnable()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        if (fix && local)
        {
            transform.localPosition = trans.position;
            transform.localRotation = trans.rotation;
        }
        else if (local)
        {
            transform.position = Vector3.Lerp(transform.position, trans.position, Time.deltaTime * speed);
            transform.rotation = Quaternion.Lerp(transform.rotation, trans.rotation, Time.deltaTime * speed);
        }
        else if (fix)
        {
            transform.position = trans.position;
            transform.rotation = trans.rotation;
        }
    }
}
