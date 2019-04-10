using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;
using System;

public struct TransformRecording
{
    public Vector3 position;
    public Quaternion rotation;
    public bool shoot;

    public TransformRecording(Vector3 position, Quaternion rotation, bool shoot) : this()
    {
        this.position = position;
        this.rotation = rotation;
        this.shoot = shoot;
    }
}

public class GunManager : MonoBehaviour
{

    Item item;
    public float shootCoolDownTimeLeft { get; private set; }
    public WarpManager m_warpManager;

    [SerializeField] GameObject bullet;
    [SerializeField] GameObject muzzle;

    private bool isGrabbed;
    private bool shootCoolDown;
    private float shootCoolDownTime = 1f;

    // Use this for initialization
    void Start()
    {
        item = GetComponent<Item>();
    }

    void OnTriggerShoot()
    {
        if (/*isGrabbed*/true)
        {
            //Debug.Log("OnTriggerShoot Start");
            if (item.m_hand == Hand.Left && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0.1f)
            {
                //Debug.Log("Shoot left");
                TryShoot();
            }
            else if (item.m_hand == Hand.Right && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.1f)
            {
                //Debug.Log("Shoot right");
                TryShoot();
            }
            //Debug.Log("OnTriggerShoot End");
        }
    }
    
    void TryShoot()
    {
        if (shootCoolDownTimeLeft <= 0)
        {
            if (GameManager.bulletTime)
            {
                m_warpManager.QueueAction();
                Shoot(muzzle.transform.position, muzzle.transform.rotation);
            }
            else
            {
            Shoot(muzzle.transform.position, muzzle.transform.rotation);
            }
            shootCoolDownTimeLeft = shootCoolDownTime;
        }

    }

    public void Shoot(Vector3 pos, Quaternion rot)
    {
        GameObject bulletClone = Instantiate(bullet, pos, rot);
    }

    // Update is called once per frame
    void Update()
    {

        if (item.m_itemState == ItemState.unholstered)
        {
        //CheckGrab();
        OnTriggerShoot();
        }

        ShootCountDown();
    }

    private void ShootCountDown()
    {
        if (shootCoolDownTimeLeft > 0)
        {
            shootCoolDownTimeLeft -= Time.unscaledDeltaTime;
        }
    }
       
}
