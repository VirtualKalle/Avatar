using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight : MonoBehaviour
{
    Item item;
    [SerializeField] LineRenderer lr;
    [SerializeField] Transform muzzle;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.widthMultiplier *= AvatarGameManager.worldScale;
        item = GetComponent<Item>();
    }

    void Update()
    {
        if (item.m_itemState == ItemState.unholstered)
        {
            if (!lr.enabled)
            {
                lr.enabled = true;
            }

            LaserPoint();
        }
        else if (lr.enabled)
        {
            lr.enabled = false;
        }
    }

    void LaserPoint()
    {
        lr.SetPosition(1, muzzle.position + muzzle.forward * 1);
        lr.SetPosition(0, muzzle.transform.position);
    }
}

