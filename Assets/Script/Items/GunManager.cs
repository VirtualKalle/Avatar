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
    private bool hasShot;
    float shootThreshold = 0.5f;
    [SerializeField] AudioClip gunShotAudioClip;
    AudioSource m_audioSorce;

    // Use this for initialization
    void Start()
    {
        item = GetComponent<Item>();
        m_audioSorce = GetComponent<AudioSource>();
    }

    void OnTriggerShoot()
    {
        //Debug.Log("OnTriggerShoot Start");
        if (item.m_hand == Hand.Left && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > shootThreshold && !hasShot)
        {
            //Debug.Log("Shoot left");
            TryShoot();
        }
        else if (item.m_hand == Hand.Right && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > shootThreshold && !hasShot)
        {
            //Debug.Log("Shoot right");
            TryShoot();
        }
        else if (
                (item.m_hand == Hand.Left && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) < shootThreshold ||
                item.m_hand == Hand.Right && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) < shootThreshold) &&
                hasShot)
        {
            hasShot = false;
        }
        //Debug.Log("OnTriggerShoot End");
    }

    void TryShoot()
    {
        if (!hasShot)
        {
            Shoot(muzzle.transform.position, muzzle.transform.rotation);
            
            m_audioSorce.PlayOneShot(gunShotAudioClip);
            if (AvatarGameManager.bulletTime)
            {
                m_audioSorce.pitch = 0.5f;
            }
            else
            {
                m_audioSorce.pitch = 1f;
            }
            hasShot = true;
        }
    }

    public void Shoot(Vector3 pos, Quaternion rot)
    {
        GameObject bulletClone = Instantiate(bullet, pos, rot);
        bulletClone.transform.localScale *= transform.lossyScale.magnitude;
    }

    // Update is called once per frame
    void Update()
    {

        if (item.m_itemState == ItemState.unholstered)
        {
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
