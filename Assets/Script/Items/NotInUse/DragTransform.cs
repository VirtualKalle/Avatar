using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragTransform : MonoBehaviour {

    [SerializeField] Transform trans;
    [SerializeField] float speed = 5;

	void Update () {
        trans.position = Vector3.Lerp(trans.position, transform.position, Time.deltaTime * speed);
        trans.rotation = Quaternion.Lerp(trans.rotation, transform.rotation, Time.deltaTime * speed);
	}
}
