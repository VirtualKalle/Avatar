using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum ItemState { holstered, unholstered, holstering, unholstering}

public class Item : MonoBehaviour
{
    
    Coroutine HolsterCo;
    [SerializeField] Transform holsterParent;
    [SerializeField] Transform hand;
    [SerializeField] public Hand m_hand;
    public ItemState m_itemState;



    public void Holster()
    {
            if (m_itemState == ItemState.unholstered)
            {
                m_itemState = ItemState.holstering;
                HolsterCo = StartCoroutine(MoveToPosition(holsterParent));
            }
    }


    public void Unholster()
    {
        if (m_itemState == ItemState.holstered)
        {
            Debug.Log("Unholster");
            m_itemState = ItemState.unholstering;
            HolsterCo = StartCoroutine(MoveToPosition(hand));
        }
    }

    IEnumerator MoveToPosition(Transform target)
    {
        Vector3 distance = (target.position - transform.position);
        transform.parent = null;

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

        if (m_itemState == ItemState.holstering)
        {
            m_itemState = ItemState.holstered;
        }
        else if (m_itemState == ItemState.unholstering)
        {
            m_itemState = ItemState.unholstered;
        }

    }

}
