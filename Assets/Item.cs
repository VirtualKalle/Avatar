using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    public bool holstered = false;
    Coroutine HolsterCo;
    [SerializeField] Transform holsterParent;
    [SerializeField] Transform hand;
    public bool isMoving;
    [SerializeField] public Hand m_hand;




    public void Holster()
    {
        if (!holstered)
        {
            Debug.Log("Holster");
            //StopCoroutine(HolsterCo);

            if (!isMoving)
            {
                HolsterCo = StartCoroutine(MoveToPosition(holsterParent));
                holstered = true;
            }
        }
    }


    public void Unholster()
    {
        if (holstered)
        {
            Debug.Log("Unholster");
            //StopCoroutine(HolsterCo);
            if (!isMoving)
            {
                HolsterCo = StartCoroutine(MoveToPosition(hand));
                holstered = false;
            }
        }
    }

    IEnumerator MoveToPosition(Transform target)
    {
        Vector3 distance = (target.position - transform.position);
        transform.parent = null;
        isMoving = true;

        while ((target.position - transform.position).magnitude > 0.1)
        {
            transform.Translate((target.position - transform.position).normalized * Time.deltaTime, Space.World);
            transform.Rotate(-10, 0, 0);
            Debug.Log("Translate moving");
            yield return null;
        }

        while ((target.position - transform.position).magnitude > 0.02)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * 20);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * 20);
            Debug.Log("Lerp moving");
            yield return null;
        }

        transform.position = target.position;
        transform.parent = target;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        isMoving = false;

    }

}
