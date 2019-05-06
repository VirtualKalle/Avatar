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
    private float relativeStartDistance;
    Vector3 stepVector;

    private void Start()
    {
        if (transform.parent != holsterParent && transform.parent != hand)
        {
            Holster();
        }

    }


    public void Holster()
    {
        if (m_itemState != ItemState.holstered)
        {
            target = holsterParent;
            transform.parent = transform.root;
            m_itemState = ItemState.holstering;
        }
    }


    public void Unholster()
    {
        if (m_itemState != ItemState.unholstered)
        {
            target = hand;
            transform.parent = transform.root;
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
        //stepVector = (target.position - transform.position).normalized * Time.unscaledDeltaTime * AvatarGameManager.worldScale;
        //relativeStartDistance = (target.position - transform.position).magnitude / AvatarGameManager.worldScale;
        stepVector = (target.position - transform.position).normalized * Time.unscaledDeltaTime * 1;
        relativeStartDistance = (target.position - transform.position).magnitude / 1;
        if (relativeStartDistance > 0.05)
        {

            if (relativeStartDistance > 0.1 && (target.position - transform.position).magnitude > stepVector.magnitude)
            {
                transform.Translate(stepVector * 3, Space.World);
                transform.Rotate((-360 * 4) * Time.deltaTime, 0, 0);
                //Debug.Log("Translate distance left" + (target.position - transform.position).magnitude + "of " + transform.name);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, target.position, Time.unscaledDeltaTime * 50);
                transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.unscaledDeltaTime * 20);
                //Debug.Log("Lerp distance left " + (target.position - transform.position).magnitude + "of " + transform.name);
            }

        }
        else if (transform.parent == transform.root || transform.parent == null)
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
}
