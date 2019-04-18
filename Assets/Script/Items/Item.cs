using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemState { holstered, unholstered, holstering, unholstering }

public class Item : MonoBehaviour
{

    Coroutine HolsterCo;
    [SerializeField] Transform holsterParent;
    [SerializeField] Transform hand;
    [SerializeField] public Hand m_hand;
    public ItemState m_itemState;

    //Void variant
    Transform target;
    private float distance;

    private void Start()
    {
        if (transform.parent != holsterParent && transform.parent != hand)
        {
            Holster();
        }
    }


    public void Holster()
    {
        if (m_itemState == ItemState.unholstered)
        {
            target = holsterParent;
            distance = (target.position - transform.position).magnitude;
            transform.parent = null;
            m_itemState = ItemState.holstering;
        }
    }


    public void Unholster()
    {
        if (m_itemState == ItemState.holstered)
        {
            target = hand;
            distance = (target.position - transform.position).magnitude;
            transform.parent = null;
            m_itemState = ItemState.unholstering;
        }
    }

    private void Update()
    {
        if (m_itemState == ItemState.unholstering || m_itemState == ItemState.holstering)
        {
        MoveToPosition();
        }
    }

    void MoveToPosition()
    {

        if ((target.position - transform.position).magnitude > 0.01 * transform.localScale.magnitude)
        {

            if ((target.position - transform.position).magnitude > 0.1 * distance)
            {
                transform.Translate((target.position - transform.position) * 30 * distance * Time.deltaTime, Space.World);
                transform.Rotate((-360 * 4) * Time.deltaTime, 0, 0);
                //Debug.Log("Translate distance left" + (target.position - transform.position).magnitude + "of " + transform.name);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * distance * 100);
                transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * 20);
                //Debug.Log("Lerp distance left " + (target.position - transform.position).magnitude + "of " + transform.name);
            }

        }
        else if(transform.parent == null)
        {
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

    //#region enumerator

    //public void Holster()
    //{
    //    if (m_itemState == ItemState.unholstered)
    //    {
    //    m_itemState = ItemState.holstering;
    //    HolsterCo = StartCoroutine(MoveToPosition(holsterParent));
    //    }
    //}


    //public void Unholster()
    //{
    //    if (m_itemState == ItemState.holstered)
    //    {
    //    Debug.Log("Unholster");
    //    m_itemState = ItemState.unholstering;
    //    HolsterCo = StartCoroutine(MoveToPosition(hand));
    //    }
    //}

    //IEnumerator MoveToPosition(Transform target)
    //{
    //    float distance = (target.position - transform.position).magnitude;
    //    transform.parent = null;

    //    if (distance > 0.01 * transform.localScale.magnitude)
    //    {

    //        while ((target.position - transform.position).magnitude > 0.1 * distance)
    //        {
    //            transform.Translate((target.position - transform.position) * 5 * distance * Time.deltaTime, Space.World);
    //            transform.Rotate((-360 * 4) * distance * Time.deltaTime, 0, 0);
    //            Debug.Log("Translate distance left" + (target.position - transform.position).magnitude + "of " + transform.name);
    //            yield return null;
    //        }

    //        while ((target.position - transform.position).magnitude > 0.02 * distance)
    //        {
    //            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * distance * 20);
    //            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * 20);
    //            Debug.Log("Lerp distance left " + (target.position - transform.position).magnitude + "of " + transform.name);
    //            yield return null;
    //        }

    //    }

    //    transform.position = target.position;
    //    transform.parent = target;
    //    transform.localRotation = Quaternion.Euler(Vector3.zero);

    //    if (m_itemState == ItemState.holstering)
    //    {
    //        m_itemState = ItemState.holstered;
    //    }
    //    else if (m_itemState == ItemState.unholstering)
    //    {
    //        m_itemState = ItemState.unholstered;
    //    }

    //}
    //#endregion
}
